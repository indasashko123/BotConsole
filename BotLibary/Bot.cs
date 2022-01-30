using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Requests;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using DataBase.Database;
using DataBase.Database.Context.MySQL;
using DataBase.Database.Interface;
using System.Threading;
using Options;
using DataBase.Models;
using BotLibary.Events;
using Telegram.Bot.Types.InputFiles;
using BotLibary.CallBackData;

namespace BotLibary
{
    
    public class Bot
    {
        public ChangesLog ConsoleMessage { get; set; }
        public AdminMessage adminMessage { get; set; }
        public readonly TelegramBotClient bot; 
        private readonly BotOptions options;
        private readonly BotConfig botConfig;
        private readonly PersonalConfig personalConfig;
       
        private Message lastMessage;
        private DateFunction dateFunction;
        private readonly DataBaseConnector context;

        public Bot(BotOptions options)
        {
            this.options = options;
            botConfig = options.botConfig;
            personalConfig = options.personalConfig;
            this.bot = new TelegramBotClient(botConfig.token);
            bot.OnMessage += onMessage;
            bot.OnCallbackQuery += OnQuery;
            dateFunction = new DateFunction();
            context = new DataBaseConnector(new SQLContext(botConfig.dataBaseName));
            adminMessage += (async(EventArgsNotification e) => { await bot.SendTextMessageAsync(e.Admin,e.Text, replyMarkup:e?.Keyboard); });
        }
        /// <summary>
        /// Обработка входящей команды или кнопки клавиатуры.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void onMessage(object sender, MessageEventArgs e)
        {
            if (e.Message != null && e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text && !string.IsNullOrEmpty(e.Message.Text))
            {
                DataBase.Models.User admin = await context.db.FindAdminAsync();
                DataBase.Models.User currentUser =await context.db.FindUserAsync(e.Message.Chat.Id);
                ConsoleMessage?.Invoke(currentUser == null ? $"\nПользователь с chatId {e.Message.Chat.Id} не найден\n" : $"\nПользователь с chatId {e.Message.Chat.Id} уже заходил\n");
                if (currentUser == null)
                {
                    ConsoleMessage?.Invoke($"Создаем нового пользователся с ChatID {e.Message.Chat.Id}\n");
                    currentUser = await context.db.CreateNewUserAsync(e.Message.From.Username, e.Message.From.FirstName, e.Message.From.LastName, e.Message.Chat.Id);
                    return;
                }
                if (!currentUser.IsAdmin)
                {
                    if (e.Message.Text == "/start")
                    {
                        await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(personalConfig.Paths["GREETING"]));
                        await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["USERGREETING"], replyMarkup: KeyBoards.GetStartKeyboard(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["APPOINTMENT"])
                    {
                        await bot.SendTextMessageAsync(currentUser.ChatId, "Для записи необходимо выбрать месяц", replyMarkup: KeyBoards.GetMonthButtons(await context.db.GetMonthsAsync(), Codes.UserChoise,currentUser));
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["PRICE"])
                    {
                        await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(personalConfig.Paths["PRICE"]), replyMarkup: KeyBoards.GetStartKeyboard(options));
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
                            await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(photoURL));
                        }
                        return;
                    }                  
                    if (e.Message.Text == personalConfig.Buttons["ABOUT"])
                    {
                        await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["ABOUT"], replyMarkup: KeyBoards.GetStartKeyboard(options));
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
                    if (e.Message.Text == "/reg"+botConfig.password && admin == null)
                    {
                        await dateFunction.CreateMonthsAsync();
                        await context.db.AddMonthAsync(dateFunction.CurrentMonth);                       
                        await context.db.CreateDaysAsync(dateFunction.CurrentDay, dateFunction.CurrentMonth, dateFunction.DayNames);
                        List<Day> daysCurrentMonth = await context.db.FindDaysAsync(dateFunction.CurrentMonth.MonthId);
                        foreach (Day day in daysCurrentMonth)
                        {
                            for (int i = 0; i<botConfig.appointmentStandartCount; i++)
                            {
                                Appointment app = new Appointment(botConfig.appointmentStandartTimes[i],day.DayId);
                                await context.db.AddAppointmentAsync(app);
                            }
                        }
                        await context.db.AddMonthAsync(dateFunction.NextMonth);
                        await context.db.CreateDaysAsync(1, dateFunction.NextMonth, dateFunction.DayNames);
                        List<Day> daysNextMonth = await context.db.FindDaysAsync(dateFunction.NextMonth.MonthId);
                        foreach (Day day in daysNextMonth)
                        {
                            for (int i = 0; i < botConfig.appointmentStandartCount; i++)
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
                        await adminMessage?.Invoke(new EventArgsNotification(admin.ChatId, "Зарегестрированно!"));
                    }
                    //else
                    await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["UNKNOWN"]);
                    return;
                }
                if (currentUser.IsAdmin)
                {
                    if (admin.Status.Split('/')[0] == "AddApp")
                    {

                        int appId = 0;
                        if (Int32.TryParse(admin.Status.Split('/')[1], out appId))
                        {
                            admin.Status = "";
                            await context.db.UpdateUserAsync(admin);
                            Appointment app = await context.db.FindAppointmentAsync(appId);
                            if (e.Message.Text == "Отмена")
                            {
                                await context.db.DeleteAppAsync(app);
                                app = null;
                                await bot.SendTextMessageAsync(admin.ChatId, $" запись удалена", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                                return;
                            }
                            app.AppointmentTime = e.Message.Text;
                            await context.db.UpdateAppAsync(app);
                            await bot.SendTextMessageAsync(admin.ChatId, $"Добавилась запись на {app.AppointmentTime}", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                            return;
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
                        await bot.SendTextMessageAsync(admin.ChatId, "Отправить это сообщение всем пользователям?", replyMarkup: KeyBoards.GetConfirmKeyboard(0,options,Codes.AdminMailingConfirm,0));
                        return;
                    }
                    if (e.Message.Text == "/start")
                    {
                        await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["ADMINGREETING"], replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["ADDAPP"])
                    {
                        List<Month> month = await context.db.GetMonthsAsync();
                        await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["ADMINSELECTMONTH"], replyMarkup: KeyBoards.GetMonthButtons(month, Codes.AdminAdd, currentUser));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["DELAPP"])
                    {
                        List<Month> months = await context.db.GetMonthsAsync();
                        await bot.SendTextMessageAsync(admin?.ChatId, personalConfig.Messages["ADMINSELECTMONTH"], replyMarkup: KeyBoards.GetMonthButtons(months, Codes.AdminDelete, admin));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["ALLUSERS"])
                    {
                        List<DataBase.Models.User> users = await context.db.FindUsersAsync();
                        foreach (var user in users)
                        {
                            string message = $"Пользователь {user.FirstName} {user.LastName} {user.Username}\n";
                            await bot.SendTextMessageAsync(admin.ChatId, message);
                        }
                        await bot.SendTextMessageAsync(admin.ChatId, "Это все пользователи", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["MAKEWEEKEND"])
                    {
                        List<Month> months = await context.db.GetMonthsAsync();
                        await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINSELECTMONTH"], replyMarkup: KeyBoards.GetMonthButtons(months, Codes.AdminWeekEnd, admin));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["LOOKNOTCONFIRM"])
                    {
                        List<Appointment> apps = await context.db.FindConfirmAppointmentsAsync(false);
                        foreach (Appointment app in apps)
                        {
                            DataBase.Models.User user = await context.db.FindUserAsync(app.User); 
                            Day day = await context.db.FindDayAsync(app.Day);
                            await bot.SendTextMessageAsync(admin.ChatId, $"Запись на {day.Date}.{day.MonthNumber} на время {app.AppointmentTime}\n" +
                                $"Записался {user.ToString()}", replyMarkup: KeyBoards.GetConfirmKeyboard(app.AppointmentId, options, Codes.AdminConfirm, user.UserId));
                        }
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["LOOKNOTCONFIRM"])
                    {
                        List<Appointment> apps = await context.db.FindConfirmAppointmentsAsync(false);
                        foreach (Appointment app in apps)
                        {
                            DataBase.Models.User user = await context.db.FindUserAsync(app.User);
                            Day day = await context.db.FindDayAsync(app.Day);
                            await bot.SendTextMessageAsync(admin.ChatId, $"Запись на {day.Date}.{day.MonthNumber} на время {app.AppointmentTime}\n" +
                                $"Записался {user.ToString()}", replyMarkup: KeyBoards.GetConfirmKeyboard(app.AppointmentId, options, Codes.AdminConfirm, user.UserId));
                        }
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["LOOKCONFIRM"])
                    {
                        List<Appointment> apps = await context.db.FindConfirmAppointmentsAsync(true);
                        foreach (Appointment app in apps)
                        {
                            DataBase.Models.User user = await context.db.FindUserAsync(app.User);
                            Day day = await context.db.FindDayAsync(app.Day);
                            await bot.SendTextMessageAsync(admin.ChatId, $"Запись на {day.Date}.{day.MonthNumber} на время {app.AppointmentTime}\n" +
                                $"Записался {user.ToString()}", replyMarkup: KeyBoards.GetCanccelButton(app.AppointmentId, options, Codes.AdminCancel, user.UserId));
                        }          
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["MAILING"])
                    {
                        admin.Status = "Mailing";
                        await context.db.UpdateUserAsync(admin);
                        await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINMAILING"]);
                    }
                }
            }
        }

        /// <summary>
        /// Обработка кнопки на экране.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnQuery(object sender, CallbackQueryEventArgs e)
        {
            DataBase.Models.User admin = await context.db.FindAdminAsync();
            long chatId = e.CallbackQuery.Message.Chat.Id;
            DataBase.Models.User currentUser = await context.db.FindUserAsync(chatId);
            CallBackData.CallBackData data =await CallBackData.CallBackData.GetDataAsync(e.CallbackQuery.Data);
            if (currentUser == null)
            {
                ConsoleMessage?.Invoke($"Пользователь NULL при нажатии кнопки {e.CallbackQuery.Data}");
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
                        string message = $"{personalConfig.Messages["NEWAPP"]}\n " +
                            $"{currentUser.FirstName} {currentUser.LastName} {currentUser.Username}\n в {day.DayOfWeek}, {day.Date}.{day.MonthNumber} числа\n на время - {app.AppointmentTime}";
                        await context.db.UpdateAppAsync(app);
                        await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["WAITCINFIRM"], replyMarkup: KeyBoards.GetStartKeyboard(options));
                        await bot.SendTextMessageAsync(admin.ChatId, message, replyMarkup: KeyBoards.GetConfirmKeyboard(app.AppointmentId,options, Codes.AdminConfirm, currentUser.UserId));
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
                            await bot.SendTextMessageAsync(data.UserId, personalConfig.Messages["YOURAPPCONFIRM"], replyMarkup: KeyBoards.GetStartKeyboard(options));
                        }
                        if (data.Stage == Stage.No)
                        {
                            Appointment app = await context.db.FindAppointmentAsync(data.EntityId);
                            app.IsEmpty = true;
                            app.User = 0;
                            await context.db.UpdateAppAsync(app);
                            await bot.SendTextMessageAsync(data.UserId, personalConfig.Messages["YOURAPPNOTCONFIRM"], replyMarkup: KeyBoards.GetStartKeyboard(options));
                        }
                    }
                   if (data.Action == CallBackData.Action.Add)
                    {
                        if (data.Stage == CallBackData.Stage.Month)
                        {
                            List<Day> days = await context.db.FindDaysAsync(data.EntityId);
                            await bot.SendTextMessageAsync(data.UserId, personalConfig.Messages["ADMINCHOISEDAY"], replyMarkup: KeyBoards.GetDaysButton(days,options,Codes.AdminAdd, admin));
                            return;
                        }
                        if (data.Stage == CallBackData.Stage.Day)
                        {
                            Appointment app = await context.db.AddAppointmentAsync(data.EntityId);
                            admin.Status = $"AddApp/{app.AppointmentId}";
                            await context.db.UpdateUserAsync(admin);
                            await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["WRITEAPPTIME"], replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                            return;
                        }
                    }
                   if (data.Action == CallBackData.Action.Delete)
                    {
                        if (data.Stage == Stage.Month)
                        {
                            List<Day> days = await context.db.FindDaysAsync(data.EntityId);
                            await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINCHOISEDAY"], replyMarkup: KeyBoards.GetDaysButton(days, options, Codes.AdminDelete, admin));
                            return;
                        }
                        if (data.Stage == Stage.Day)
                        {
                            List<Appointment> apps = await context.db.FindAppointmentsAsync(data.EntityId);
                            await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINCHOISEAPP"], replyMarkup: KeyBoards.GetAppointmentKeyboard(apps, options, Codes.AdminDelete, admin.UserId));
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
                                await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINCONFIRM"],
                                    replyMarkup: KeyBoards.GetConfirmKeyboard(app.AppointmentId, options, Codes.AdminDelete, userIDtoSend));                            
                            return;
                        }
                        if (data.Stage == Stage.Yes)
                        {
                            var app = await context.db.FindAppointmentAsync(data.EntityId);
                            var user = await context.db.FindUserAsync(data.UserId);
                            var day = await context.db.FindDayAsync(app.Day);
                            await bot.SendTextMessageAsync(admin.ChatId,$"Удалена запись", replyMarkup:KeyBoards.GetKeyboardAdmin(options));
                            await bot.SendTextMessageAsync(user.ChatId, $"Запись на {day.DayOfWeek} {day.Date}.{day.MonthNumber}\n" +
                                $"на время - {app.AppointmentTime} -Отменена");
                            await context.db.DeleteAppAsync(app);
                        }
                        if (data.Stage == Stage.No)
                        {
                            await bot.SendTextMessageAsync(admin.ChatId, "Отменено", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                        }

                    }
                   if (data.Action == CallBackData.Action.WeekEnd)
                    {
                        if (data.Stage == Stage.Month)
                        {
                            List<Day> days = await context.db.FindDaysAsync(data.EntityId);
                            await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINCHOISEDAY"], replyMarkup: KeyBoards.GetDaysButton(days, options, Codes.AdminWeekEnd, admin));
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
                                    foreach (Appointment appointment in apps)
                                    {
                                        
                                        if (!appointment.IsEmpty)
                                        {
                                            var user = await context.db.FindUserAsync(appointment.User);
                                            string message = $"Запись на время {appointment.AppointmentTime} занята пользователем {user.FirstName} {user.LastName}\n" +
                                                $" и {(appointment.IsConfirm? "подтверждена ":"не подтверждена ")}.";
                                            await bot.SendTextMessageAsync(admin.ChatId, message);
                                        }
                                    }
                                    await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["WEEKENDWARNING"], replyMarkup: KeyBoards.GetConfirmKeyboard(day.DayId,options,Codes.AdminWeekEnd, admin.UserId));
                                    return;
                                }
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
                            foreach(Appointment app in apps)
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
                        await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINAPPISCANCEL"], replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                        var user = await context.db.FindUserAsync(data.UserId);
                        await bot.SendTextMessageAsync(user.ChatId, personalConfig.Messages["USERAPPISCANCEL"]);
                        return;
                    }
                   if (data.Action == CallBackData.Action.Mailing)
                    {
                        if (lastMessage == null)
                        {
                            await bot.SendTextMessageAsync(admin.ChatId, "Не выбрано сообщение", replyMarkup:KeyBoards.GetKeyboardAdmin(options));
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
        public void BotStart()
        {
            bot.StartReceiving();
            Task.Run(() => UpdateDays());
            Task.Run(() => StartNotificationAsync());
        }
        /// <summary>
        /// Остановка бота
        /// </summary>
        public void BotStop()
        {
            bot.StopReceiving();
        }
        protected virtual async void UpdateDays()
        {
            ConsoleMessage?.Invoke("start update");
            while (true)
            {
                Thread.Sleep(1000);
                ConsoleMessage?.Invoke("Check update Day");
                await CheckUpdateAsync();
            }
        }
        protected virtual async Task CheckUpdateAsync()
        {
            await Task.Run(() => CheckUpdate());
            
        }
        protected virtual async void CheckUpdate()
        {
            if (DateTime.Now.Day > dateFunction.CurrentDay)
            {
                await dateFunction.IncreementDayAsync();
                if (dateFunction.CurrentDay == 1)
                {
                    List<Month> pastMonth = await context.db.GetMonthsAsync(new int[]
                        {dateFunction.CurrentMonth.MonthId,
                        dateFunction.NextMonth.MonthId});
                    List<Appointment> pastAppointment = new List<Appointment>();
                    List<Day> pastDays = new List<Day>();
                    if (pastMonth != null)
                    {
                        foreach(Month month in pastMonth)
                        {
                            pastDays.AddRange(await context.db.FindDaysAsync(month.MonthId));
                        }
                        foreach (Day day in pastDays)
                        {
                            pastAppointment.AddRange(await context.db.FindAppointmentsAsync(day.DayId));
                        }
                        await context.db.DeleteAppointmentsAsync(pastAppointment);
                        await context.db.DeleteDaysAsync(pastDays);
                        await context.db.DeleteMonthsAsync(pastMonth);
                    }
                    await context.db.AddMonthAsync(dateFunction.NextMonth);
                    await context.db.CreateDaysAsync(dateFunction.CurrentDay, dateFunction.NextMonth,  dateFunction.DayNames);
                    List<Day> daysCurrentMonth = await context.db.FindDaysAsync(dateFunction.CurrentMonth.MonthId);
                    foreach (Day day in daysCurrentMonth)
                    {
                        for (int i = 0; i < botConfig.appointmentStandartCount; i++)
                        {
                            Appointment app = new Appointment(botConfig.appointmentStandartTimes[i], day.DayId);
                            await context.db.AddAppointmentAsync(app);
                        }
                    }
                }
            }
        }

        protected virtual async void StartNotificationAsync()
        {
            ConsoleMessage?.Invoke("Start notification");
            while(true)
            {
                Thread.Sleep(3000000);
                ConsoleMessage?.Invoke("Check notification");
                await CheckNotificationAsync();

            }
        }
        protected virtual async void CheckNotification()
        {
            DateTime timeNow = DateTime.Now;
            if (timeNow.Hour <= 19 && timeNow.Hour >= 18)
            {
                List<Day> days = await context.db.FindDaysAsync(dateFunction.CurrentMonth.MonthId);
                days = days.OrderBy(day => day.Date).ToList();
                Day firstDay = days.FirstOrDefault();
                List<Appointment> apps = await context.db.FindAppointmentsAsync(firstDay.DayId);
                var admin = await context.db.FindAdminAsync();
                foreach (Appointment app in apps)
                {
                    if (app.IsConfirm)
                    {
                        var user = await context.db.FindUserAsync(app.User);
                        await bot.SendTextMessageAsync(user.ChatId, personalConfig.Messages["NOTIFICATION"]+"\n "+$"на время {app.AppointmentTime}");
                        await bot.SendTextMessageAsync(admin.ChatId, $"{user.FirstName} {user.LastName} {user.Username} записан на завтра на время {app.AppointmentTime}");
                    }
                }
            }
        }
        protected virtual async Task CheckNotificationAsync()
        {
            await Task.Run(() => CheckNotification());
        }
    }
}
