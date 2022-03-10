using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using DataBase.Database;
using DataBase.Database.Context.MySQL;
using Options;
using DataBase.Models;
using BotLibary.Events;
using Telegram.Bot.Types.InputFiles;
using BotLibary.CallBackData;
using BotLibary.Interfaces;

namespace BotLibary
{
    
    public class Bot : AbstractBot, IBot
    {       
        public Bot(BotOptions options)
        {
            this.options = options;
            botConfig = options.botConfig;
            personalConfig = options.personalConfig;
            BotName = new BotName(botConfig.Name, botConfig.CustomerName, botConfig.Direction);
            dateFunction = new DateFunction();
            context = new DataBaseConnector(new SQLContext(botConfig.dataBaseName));
            bot = new TelegramBotClient(botConfig.token);
            bot.OnMessage += onMessage;
            bot.OnCallbackQuery += OnQuery;
            bot.Timeout = new TimeSpan(0, 10, 0);
            imageManager = new ImageManager();

            adminMessage += (async (EventArgsNotification e) => { await bot.SendTextMessageAsync(e.Admin, e.Text, replyMarkup: e?.Keyboard); });
        }
        private protected virtual async void onMessage(object sender, MessageEventArgs e)
        {
            if (e.Message != null && (e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text || !string.IsNullOrEmpty(e.Message.Text)))
            {
                User admin = await context.db.FindAdminAsync();
                User currentUser = await context.db.FindUserAsync(e.Message.Chat.Id);
                ConsoleMessage?.Invoke(currentUser == null ? $"\nПользователь с chatId {e.Message.Chat.Id} не найден\n" : $"\nПользователь с chatId {e.Message.Chat.Id} уже заходил\n");
                if (currentUser == null)
                {
                    ConsoleMessage?.Invoke($"Создаем нового пользователся с ChatID {e.Message.Chat.Id}\n");
                    await context.db.CreateNewUserAsync(e.Message.From.Username, e.Message.From.FirstName, e.Message.From.LastName, e.Message.Chat.Id);
                    return;
                }
                if (!currentUser.IsAdmin)
                {
                    if (e.Message.Text == "/start" || e.Message.Text == personalConfig.Buttons["ABOUT"])
                    {
                        try
                        {
                            await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(System.IO.File.OpenRead(personalConfig.Paths["GREETING"])), caption: personalConfig.Messages["USERGREETING"], replyMarkup: KeyBoards.GetStartKeyboard(options));                           
                        }
                        catch(Exception ex)
                        {
                            ConsoleMessage?.Invoke($"\n\n !!!Возникла ошибка {BotName.Name}  "+ ex.Message +"\n\n");
                        }
                         return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["APPOINTMENT"])
                    {
                        if (admin == null)
                        {
                            await bot.SendTextMessageAsync(currentUser.ChatId, "Бот пока не активирован");
                            return;
                        }
                        await bot.SendTextMessageAsync(currentUser.ChatId, "Для записи необходимо выбрать месяц", replyMarkup: KeyBoards.GetMonthButtons(await context.db.GetMonthsAsync(), Codes.UserChoise, currentUser));
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["PRICE"])
                    {
                        try
                        {
                            await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(System.IO.File.OpenRead(personalConfig.Paths["PRICE"])), replyMarkup: KeyBoards.GetStartKeyboard(options));
                        }
                        catch (Exception ex)
                        {
                            ConsoleMessage?.Invoke($"\n\n !!!Возникла ошибка {BotName.Name}  " + ex.Message + "\n\n");
                        }
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["FEEDBACK"])
                    {
                        await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["FEEDBACK"]);
                        await bot.SendTextMessageAsync(currentUser.ChatId, "👇", replyMarkup: KeyBoards.GetInstagrammButton(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["MYWORKS"])
                    {
                        foreach (var photoURL in personalConfig.Partfolio)
                        {
                            await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(System.IO.File.OpenRead(photoURL)));
                        }
                        return;
                    }            
                    if (e.Message.Text == personalConfig.Buttons["LOCATION"])
                    {
                        await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["LOCATION"]);
                        await bot.SendLocationAsync(currentUser.ChatId, personalConfig.Latitude, personalConfig.Longitude, replyMarkup: KeyBoards.GetStartKeyboard(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["LINK"])
                    {
                        await bot.SendTextMessageAsync(currentUser.ChatId, "👇", replyMarkup: KeyBoards.GetLinkButtons(options));
                        return;
                    }
                    if (e.Message.Text == "/reg" + botConfig.password && admin == null)
                    {
                        await dateFunction.CreateMonthsAsync();
                        await context.db.AddMonthAsync(dateFunction.CurrentMonth);
                        await context.db.CreateDaysAsync(dateFunction.CurrentDay, dateFunction.CurrentMonth, dateFunction.DayNames);
                        List<Day> daysCurrentMonth = await context.db.FindDaysAsync(dateFunction.CurrentMonth.MonthId);
                        foreach (Day day in daysCurrentMonth)
                        {
                            for (int i = 0; i < botConfig.appointmentStandartTimes.Count; i++)
                            {
                                Appointment app = new Appointment(botConfig.appointmentStandartTimes[i], day.DayId);
                                await context.db.AddAppointmentAsync(app);
                            }
                        }
                        await context.db.AddMonthAsync(dateFunction.NextMonth);
                        await context.db.CreateDaysAsync(1, dateFunction.NextMonth, dateFunction.DayNames);
                        List<Day> daysNextMonth = await context.db.FindDaysAsync(dateFunction.NextMonth.MonthId);
                        foreach (Day day in daysNextMonth)
                        {
                            for (int i = 0; i < botConfig.appointmentStandartTimes.Count; i++)
                            {
                                Appointment app = new Appointment(botConfig.appointmentStandartTimes[i], day.DayId);
                                await context.db.AddAppointmentAsync(app);
                            }
                        }
                        currentUser.IsAdmin = true;
                        await context.db.UpdateUserAsync(currentUser);
                        admin = await context.db.FindAdminAsync();
                        if (admin == null)
                        {
                            ConsoleMessage?.Invoke($"Ошибка при регистрации");
                            return;
                        }
                        await adminMessage?.Invoke(new EventArgsNotification(admin.ChatId, "Зарегестрированно!", KeyBoards.GetKeyboardAdmin(options)));

                        return;
                    }
                    //else
                    await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["UNKNOWN"]);
                    return;
                }
                if (currentUser.IsAdmin)
                {
                    if (admin.Status != null)
                    {
                        if (admin.Status.Split('/')[0] == "AddApp")
                        {

                            int appId = 0;
                            if (Int32.TryParse(admin.Status.Split('/')[1], out appId))
                            {
                                admin.Status = "";
                                await context.db.UpdateUserAsync(admin);
                                Appointment app = await context.db.FindAppointmentAsync(appId);
                                if (e.Message.Text.Length == 5)
                                {
                                    app.AppointmentTime = e.Message.Text;
                                    await context.db.UpdateAppAsync(app);
                                    await bot.SendTextMessageAsync(admin.ChatId, $"Добавилась запись на {app.AppointmentTime}", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                                    return;
                                }
                                else
                                {
                                    await context.db.DeleteAppAsync(app);
                                    app = null;
                                    await bot.SendTextMessageAsync(admin.ChatId, $" запись удалена", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                                    return;
                                }                                  
                            }
                            else
                            {
                                await bot.SendTextMessageAsync(admin.ChatId, $"Запись не добавилась. Ошибка ", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                                return;
                            }
                        }
                        if (admin.Status.Split('/')[0] == "Mailing")
                        {
                            lastMessage = e.Message;
                            admin.Status = "";
                            await context.db.UpdateUserAsync(admin);
                            await bot.SendTextMessageAsync(admin.ChatId, "Отправить это сообщение всем пользователям?", replyMarkup: KeyBoards.GetConfirmKeyboard(0, options, Codes.AdminMailingConfirm, 0));
                            return;
                        }
                        if (admin.Status.Split('/')[0] == "AddExample" && e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
                        {
                            admin.Status = "";
                            await context.db.UpdateUserAsync(admin);
                            string fileId = e.Message.Photo[e.Message.Photo.Length - 1].FileId;            
                            imageManager.AddWorkExample?.Invoke(new EventArgsUpdate(this, e.Message.Photo[e.Message.Photo.Length-1].FileId));                            
                        }
                    }                     
                    if (e.Message.Text == "/start")
                    {
                        await bot.SendTextMessageAsync(currentUser.ChatId, $"Привет, мастер {admin.FirstName}", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["ADDAPP"])
                    {
                        List<Month> month = await context.db.GetMonthsAsync();
                        await bot.SendTextMessageAsync(currentUser.ChatId, $"Выбирите месяц", replyMarkup: KeyBoards.GetMonthButtons(month, Codes.AdminAdd, currentUser));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["DELAPP"])
                    {
                        List<Month> months = await context.db.GetMonthsAsync();
                        await bot.SendTextMessageAsync(admin?.ChatId, $"Выбирите месяц", replyMarkup: KeyBoards.GetMonthButtons(months, Codes.AdminDelete, admin));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["ALLUSERS"])
                    {
                        List<DataBase.Models.User> users = await context.db.FindUsersAsync();
                        foreach (var user in users)
                        {
                            string message = $"Пользователь {user.FirstName} {user.LastName} @{user.Username}\n";
                            await bot.SendTextMessageAsync(admin.ChatId, message);
                        }
                        await bot.SendTextMessageAsync(admin.ChatId, "Это все пользователи", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["MAKEWEEKEND"])
                    {
                        List<Month> months = await context.db.GetMonthsAsync();
                        await bot.SendTextMessageAsync(admin.ChatId, $"Выбирите месяц месяц", replyMarkup: KeyBoards.GetMonthButtons(months, Codes.AdminWeekEnd, admin));
                        return;
                    }       
                    if (e.Message.Text == personalConfig.AdminButtons["LOOKNOTCONFIRM"])
                    {
                        List<Appointment> apps = await context.db.FindConfirmAppointmentsAsync(false);
                        if (apps == null || apps.Count == 0)
                        {
                            await bot.SendTextMessageAsync(admin.ChatId, "Не подтвержденных записей нет");
                            return;
                        }
                        foreach (Appointment app in apps)
                        {
                            DataBase.Models.User user = await context.db.FindUserAsync(app.User);
                            Day day = await context.db.FindDayAsync(app.Day);
                            await bot.SendTextMessageAsync(admin.ChatId, $"Запись на {day.Date}.{day.MonthNumber} на время {app.AppointmentTime}\n" +
                                $"Записался {user}", replyMarkup: KeyBoards.GetConfirmKeyboard(app.AppointmentId, options, Codes.AdminConfirm, user.UserId));
                        }
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["LOOKCONFIRM"])
                    {
                        List<Appointment> apps = await context.db.FindConfirmAppointmentsAsync(true);
                        if (apps == null || apps.Count == 0)
                        {
                            await bot.SendTextMessageAsync(admin.ChatId, "Подтвержденных записей нет");
                            return;
                        }
                        foreach (Appointment app in apps)
                        {
                            DataBase.Models.User user = await context.db.FindUserAsync(app.User);
                            Day day = await context.db.FindDayAsync(app.Day);
                            await bot.SendTextMessageAsync(admin.ChatId, $"Запись на {day.Date}.{day.MonthNumber} на время {app.AppointmentTime}\n" +
                                $"Записался {user}", replyMarkup: KeyBoards.GetCanccelButton(app.AppointmentId, options, Codes.AdminCancel, user.UserId));
                        }
                        return;
                    }
                    // Настройки
                    if (e.Message.Text == personalConfig.AdminButtons["OPTIONS"])
                    {
                        await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINOPTIONS"], replyMarkup: KeyBoards.GetOptionsKeyboard(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["MAILING"])
                    {
                        admin.Status = "Mailing";
                        await context.db.UpdateUserAsync(admin);
                        await bot.SendTextMessageAsync(admin.ChatId, "Следующее записанное сообщение будет отправленно. Может быть текст, фото, голосовое и т.д.");
                        return;
                    }

                    // Настройки
                    //if (e.Message.Text == personalConfig.AdminButtons["ADDEXAMPLE"])
                    //{
                    //    admin.Status = "AddExample";
                    //    await context.db.UpdateUserAsync(admin);
                    //    await bot.SendTextMessageAsync(admin.ChatId, $"Отправте следующим сообщением фото, которое нужно добавить в портфолио");
                    //}
                    //if (e.Message.Text == personalConfig.AdminButtons["DELETEEXAMPLE"])
                    //if (e.Message.Text == personalConfig.AdminButtons["GREETING"])
                    //if (e.Message.Text == personalConfig.AdminButtons["PRICE"])

                    await bot.SendTextMessageAsync(admin.ChatId, "Неизвестная команда");
                    return;
                }
            }
        }
        private protected virtual async void OnQuery(object sender, CallbackQueryEventArgs e)
        {
            DataBase.Models.User admin = await context.db.FindAdminAsync();
            long chatId = e.CallbackQuery.Message.Chat.Id;
            DataBase.Models.User currentUser = await context.db.FindUserAsync(chatId);
            CallBackData.CallBackData data = await CallBackData.CallBackData.GetDataAsync(e.CallbackQuery.Data);
            if (currentUser == null)
            {
                ConsoleMessage?.Invoke($"Пользователь NULL при нажатии кнопки {e.CallbackQuery.Data}");
                return;
            }
            if (e.CallbackQuery.Data.ToUpper().Contains("PHONE"))
            {
                string[] phone = e.CallbackQuery.Data.Split('/');
                await bot.SendContactAsync(chatId, phone[1], BotName.CustomerName);
                return;
            }
            if (data.Error == "0" && !data.EmptyButton)
            {
                if (data.UserRole == UserRole.User)
                {
                    if (data.Stage == Stage.Month && data.Action == CallBackData.Action.Choise)
                    {
                        List<Day> days = await context.db.FindDaysAsync(data.EntityId);
                        await bot.SendTextMessageAsync(chatId, personalConfig.Messages["CHOSEDAY"], replyMarkup: KeyBoards.GetDaysButton(days, options, Codes.UserChoise, currentUser));
                        return;
                    }
                    if (data.Stage == Stage.Day && data.Action == CallBackData.Action.Choise)
                    {
                        Day day = await context.db.FindDayAsync(data.EntityId);
                        if (!day.IsWorkDay)
                        {
                            await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["ISWEEKENDFORUSER"], replyMarkup: KeyBoards.GetStartKeyboard(options));
                            return;
                        }
                        List<Appointment> apps = await context.db.FindAppointmentsAsync(data.EntityId);
                        await bot.SendTextMessageAsync(chatId, personalConfig.Messages["CHOSEAPP"], replyMarkup: KeyBoards.GetAppointmentKeyboard(apps, options, Codes.UserChoise, currentUser.UserId));
                        return;
                    }
                    if (data.Stage == Stage.Appointment && data.Action == CallBackData.Action.Choise)
                    {
                        Appointment app = await context.db.FindAppointmentAsync(data.EntityId);
                        if (!app.IsEmpty)
                        {
                            await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["SORRYTAKEN"], replyMarkup: KeyBoards.GetStartKeyboard(options));
                            return;
                        }
                        app.IsConfirm = false;
                        app.IsEmpty = false;
                        app.User = currentUser.UserId;
                        Day day = await context.db.FindDayAsync(app.Day);
                        string message = $"Новая запись - \n " +
                            $"{currentUser.FirstName} {currentUser.LastName} @{currentUser.Username}\n в {day.DayOfWeek}, {day.Date}.{day.MonthNumber} числа\n на время - {app.AppointmentTime}";
                        await context.db.UpdateAppAsync(app);
                        await bot.SendTextMessageAsync(currentUser.ChatId, "Заявка принята. Ожидайте подтверждения мастера", replyMarkup: KeyBoards.GetStartKeyboard(options));
                        await bot.SendTextMessageAsync(admin.ChatId, message, replyMarkup: KeyBoards.GetConfirmKeyboard(app.AppointmentId, options, Codes.AdminConfirm, currentUser.UserId));
                        return;
                    }
                }
                if (data.UserRole == UserRole.Admin)
                {
                    if (data.Action == CallBackData.Action.Confirm)
                    {
                        if (data.Stage == Stage.Yes)
                        {
                            Appointment app = await context.db.FindAppointmentAsync(data.EntityId);
                            app.IsConfirm = true;
                            await context.db.UpdateAppAsync(app);
                            var confirmedUser = await context.db.FindUserAsync(data.UserId);
                            await bot.SendTextMessageAsync(confirmedUser.ChatId, personalConfig.Messages["YOURAPPCONFIRM"], replyMarkup: KeyBoards.GetStartKeyboard(options));
                            await bot.SendTextMessageAsync(admin.ChatId, $"Запись на время {app.AppointmentTime} подтверждена пользователю {confirmedUser.FirstName}");
                        }
                        if (data.Stage == Stage.No)
                        {
                            Appointment app = await context.db.FindAppointmentAsync(data.EntityId);
                            app.IsEmpty = true;
                            app.User = 0;
                            await context.db.UpdateAppAsync(app);
                            var confirmedUser = await context.db.FindUserAsync(data.UserId);
                            await bot.SendTextMessageAsync(confirmedUser.ChatId, personalConfig.Messages["YOURAPPNOTCONFIRM"], replyMarkup: KeyBoards.GetStartKeyboard(options));
                            await bot.SendTextMessageAsync(admin.ChatId, $"Запись на время {app.AppointmentTime} подтверждена пользователю {confirmedUser.FirstName}");
                        }
                    }
                    if (data.Action == CallBackData.Action.Add)
                    {
                        if (data.Stage == CallBackData.Stage.Month)
                        {
                            List<Day> days = await context.db.FindDaysAsync(data.EntityId);
                            var user = await context.db.FindUserAsync(data.UserId);
                            await bot.SendTextMessageAsync(user.ChatId, "Выберите день, в который нужно добавить запись", replyMarkup: KeyBoards.GetDaysButton(days, options, Codes.AdminAdd, admin));
                            return;
                        }
                        if (data.Stage == CallBackData.Stage.Day)
                        {
                            Appointment app = await context.db.AddAppointmentAsync(data.EntityId);
                            admin.Status = $"AddApp/{app.AppointmentId}";
                            await context.db.UpdateUserAsync(admin);
                            await bot.SendTextMessageAsync(admin.ChatId, $"Напишите время приема.\n Не более 5 символов", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                            return;
                        }
                    }
                    if (data.Action == CallBackData.Action.Delete)
                    {
                        if (data.Stage == Stage.Month)
                        {
                            List<Day> days = await context.db.FindDaysAsync(data.EntityId);
                            await bot.SendTextMessageAsync(admin.ChatId, "Выберите день, в котором нужно удалить запись", replyMarkup: KeyBoards.GetDaysButton(days, options, Codes.AdminDelete, admin));
                            return;
                        }
                        if (data.Stage == Stage.Day)
                        {
                            List<Appointment> apps = await context.db.FindAppointmentsAsync(data.EntityId);
                            await bot.SendTextMessageAsync(admin.ChatId, $"Выбирите запись для удаления", replyMarkup: KeyBoards.GetAppointmentKeyboard(apps, options, Codes.AdminDelete, admin.UserId));
                            return;
                        }
                        if (data.Stage == Stage.Appointment)
                        {
                            Appointment app = await context.db.FindAppointmentAsync(data.EntityId);
                            int userIDtoSend = app.User;
                            if (app.IsEmpty)
                            {
                                userIDtoSend = admin.UserId;
                            }
                            await bot.SendTextMessageAsync(admin.ChatId, "Подтверждение удаления записи",
                                replyMarkup: KeyBoards.GetConfirmKeyboard(app.AppointmentId, options, Codes.AdminDelete, userIDtoSend));
                            return;
                        }
                        if (data.Stage == Stage.Yes)
                        {
                            var app = await context.db.FindAppointmentAsync(data.EntityId);
                            var user = await context.db.FindUserAsync(data.UserId);
                            var day = await context.db.FindDayAsync(app.Day);
                            await bot.SendTextMessageAsync(admin.ChatId, $"Удалена запись", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                            if (user != null)
                            {
                                await bot.SendTextMessageAsync(user.ChatId, $"Запись на {day.DayOfWeek} {day.Date}.{day.MonthNumber}\n" +
                                $"на время - {app.AppointmentTime} -Отменена");
                            }    
                            await context.db.DeleteAppAsync(app);
                        }
                        if (data.Stage == Stage.No)
                        {
                            await bot.SendTextMessageAsync(admin.ChatId, "Запись не будет удалена", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                        }

                    }
                    if (data.Action == CallBackData.Action.WeekEnd)
                    {
                        if (data.Stage == Stage.Month)
                        {
                            List<Day> days = await context.db.FindDaysAsync(data.EntityId);
                            await bot.SendTextMessageAsync(admin.ChatId,"Выбирите день для выходного" , replyMarkup: KeyBoards.GetDaysButton(days, options, Codes.AdminWeekEnd, admin));
                            return;
                        }
                        if (data.Stage == Stage.Day)
                        {
                            Day day = await context.db.FindDayAsync(data.EntityId);
                            if (!day.IsWorkDay)
                            {
                                day.IsWorkDay = true;
                                await context.db.UpdateDayAsync(day);
                                await bot.SendTextMessageAsync(admin.ChatId, $"День {day.Date}.{day.MonthNumber} теперь рабочий день\n", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                                return;
                            }
                            List<Appointment> apps = await context.db.FindAppointmentsAsync(day.DayId);
                            bool AllEmpty = true;
                            foreach (Appointment app in apps)
                            {
                                if (!app.IsEmpty)
                                {
                                AllEmpty = false;
                                var user = await context.db.FindUserAsync(app.User);
                                string message = $"Запись на время {app.AppointmentTime} занята пользователем {user.FirstName} {user.LastName}\n" +
                                     $" и {(app.IsConfirm ? "подтверждена " : "не подтверждена ")}.";
                                await bot.SendTextMessageAsync(admin.ChatId, message);
                                }
                            }
                            if (!AllEmpty)
                            {
                                await bot.SendTextMessageAsync(admin.ChatId, "Продолжить делать день выходным?\n Имеет не свободные записи. В случае подтверждения они будут отменены", 
                                    replyMarkup: KeyBoards.GetConfirmKeyboard(day.DayId, options, Codes.AdminWeekEnd, admin.UserId));
                                return;
                            } 
                            if (AllEmpty)
                            {
                                day.IsWorkDay = false;
                                await context.db.UpdateDayAsync(day);
                                await bot.SendTextMessageAsync(admin.ChatId, $"День {day.Date}.{day.MonthNumber} теперь выходной\n", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                                return;
                            }

                        }
                        if (data.Stage == Stage.Yes)
                        {
                            List<Appointment> apps = await context.db.FindAppointmentsAsync(data.EntityId);
                            Day day = await context.db.FindDayAsync(data.EntityId);
                            foreach (Appointment app in apps)
                            {
                                if (!app.IsEmpty)
                                {
                                    DataBase.Models.User user = await context.db.FindUserAsync(app.User);
                                    await bot.SendTextMessageAsync(user.ChatId, personalConfig.Messages["USERAPPISCANCEL"]);
                                }
                            }
                            day.IsWorkDay = false;
                            await context.db.UpdateDayAsync(day);
                            await bot.SendTextMessageAsync(admin.ChatId, $"День {day.Date}.{day.MonthNumber} теперь выходной\n", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                            return;
                        }
                        if (data.Stage == Stage.No)
                        {
                            await bot.SendTextMessageAsync(admin.ChatId, "Отменено", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                            return;
                        }
                    }
                    if (data.Action == CallBackData.Action.CancelApp)
                    {
                        Appointment app = await context.db.FindAppointmentAsync(data.EntityId);
                        app.IsConfirm = false;
                        app.User = 0;
                        app.IsEmpty = true;
                        await context.db.UpdateAppAsync(app);
                        await bot.SendTextMessageAsync(admin.ChatId, $"Запись на время {app.AppointmentTime} отменена", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                        var user = await context.db.FindUserAsync(data.UserId);
                        await bot.SendTextMessageAsync(user.ChatId, personalConfig.Messages["USERAPPISCANCEL"]);
                        return;
                    }
                    if (data.Action == CallBackData.Action.Mailing)
                    {
                        if (lastMessage == null)
                        {
                            await bot.SendTextMessageAsync(admin.ChatId, "Не выбрано сообщение", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                            return;
                        }
                        admin.Status = "";
                        await context.db.UpdateUserAsync(admin);
                        if (data.Stage == Stage.Yes)
                        {
                            var users = await context.db.FindUsersAsync();
                            foreach (var user in users)
                            {
                                await bot.ForwardMessageAsync(user.ChatId, admin.ChatId, lastMessage.MessageId);
                            }
                        }
                        else
                        {
                            await bot.SendTextMessageAsync(admin.ChatId, "Отменено", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                        }
                        lastMessage = null;
                        return;
                    }
                }
            }
            if (data.Error == "404")
            {
                ConsoleMessage?.Invoke($"Ошибка 404 {e.CallbackQuery.Data}");
                await bot.SendTextMessageAsync(chatId, "Какая-то ошибка(( попробуйте еще раз", replyMarkup: KeyBoards.GetStartKeyboard(options));
            }
            if (data.Error == "405")
            {
                ConsoleMessage?.Invoke($"Ошибка 405 {e.CallbackQuery.Data}");
                await bot.SendTextMessageAsync(chatId, "Ошибка парсинга данных(( попробуйте еще раз", replyMarkup: KeyBoards.GetStartKeyboard(options));
            }
            
        }

        /// <summary>
        ///  Запуск бота
        /// </summary>
        public  void  BotStart()
        {
            bot.StartReceiving();
            Task.Run(() => StartUpdateDays());
            Task.Run(() => StartNotificationAsync());
            ConsoleMessage?.Invoke($"Бот {BotName.Name} запущен");
        }
        /// <summary>
        /// Остановка бота
        /// </summary>
        public  void BotStop()
        {
            bot.StopReceiving();
            ConsoleMessage?.Invoke($"Бот {BotName.Name} остановлен");
        }     
       
    }
}
