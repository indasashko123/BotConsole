﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using DataBase.Database;
using DataBase.Database.Context.MySQL;
using Options;
using DataBase.Models;
using Telegram.Bot.Types.InputFiles;
using BotLibary.Bots.Interfaces;
using BotLibary.Bots.Events;
using BotLibary.Bots.Masters.Keyboards;
using BotLibary.Bots.Masters.CallBackData;
using System.IO;
using Options.MasterBotConfig;

namespace BotLibary.Bots.Masters
{
    
    public class MasterBot : AbstractMasterBot, IBot
    {
        

        public MasterBot(BotOptions<MasterBotConfig, PersonalMasterBotConfig> options)
        {
            this.options = options;
            botConfig = options.botConfig;
            personalConfig = options.personalConfig;
            this.BotName = new BotName(botConfig.Name, botConfig.CustomerName, botConfig.Direction);
            dateFunction = new DateFunction();
            context = new DataBaseConnector(new SQLContext(botConfig.DataBaseName));
            this.bot = new TelegramBotClient(botConfig.Token);
            bot.OnMessage += onMessage;
            bot.OnCallbackQuery += OnQuery;
            bot.Timeout = new TimeSpan(0, 10, 0);
            ImageManager = new ImageManager();       
           AdminLog += (async (EventArgsNotification e) => { await bot.SendTextMessageAsync(e.Admin, e.Text, replyMarkup: e?.Keyboard); });
        }
        private protected virtual async void onMessage(object sender, MessageEventArgs e)
        {
            if (e.Message != null && (e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text || !string.IsNullOrEmpty(e.Message.Text)))
            {
                DataBase.Models.User admin = await context.db.FindAdminAsync();
                DataBase.Models.User currentUser = await context.db.FindUserAsync(e.Message.Chat.Id);
               // ConsoleMessage?.Invoke(currentUser == null ? $"\nПользователь с chatId {e.Message.Chat.Id} не найден\n" : $"\nПользователь с chatId {e.Message.Chat.Id} уже заходил\n");
                if (currentUser == null)
                {
                    Log?.Invoke($"Создаем нового пользователся с ChatID {e.Message.Chat.Id}\n");
                    currentUser = await context.db.CreateNewUserAsync(e.Message.From.Username, e.Message.From.FirstName, e.Message.From.LastName, e.Message.Chat.Id);
                    return;
                }
                if (!currentUser.IsAdmin)
                {
                    if (e.Message.Text == "/start" || e.Message.Text == personalConfig.Buttons["ABOUT"])
                    {
                        try
                        {
                            await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(System.IO.File.OpenRead(personalConfig.Paths["GREETING"])), caption: personalConfig.Messages["USERGREETING"], replyMarkup: UserKeyboard.GetStartKeyboard(options));                           
                        }
                        catch(Exception ex)
                        {
                            Log?.Invoke($"\n\n !!!Возникла ошибка {BotName.Name}  "+ ex.Message +"\n\n");
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
                        await bot.SendTextMessageAsync(currentUser.ChatId, "Для записи необходимо выбрать месяц", replyMarkup: CommonKeyboard.GetMonthButtons(await context.db.GetMonthsAsync(), Codes.UserChoise, currentUser));
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["PRICE"])
                    {
                        try
                        {
                            await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(System.IO.File.OpenRead(personalConfig.Paths["PRICE"])), replyMarkup: UserKeyboard.GetStartKeyboard(options));
                        }
                        catch (Exception ex)
                        {
                            Log?.Invoke($"\n\n !!!Возникла ошибка {BotName.Name}  " + ex.Message + "\n\n");
                        }
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["FEEDBACK"])
                    {
                        await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["FEEDBACK"]);
                        await bot.SendTextMessageAsync(currentUser.ChatId, "👇", replyMarkup: UserKeyboard.GetInstagrammButton(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["MYWORKS"])
                    {
                        foreach (var photoURL in personalConfig.Partfolio)
                        {
                            try
                            {
                                await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(System.IO.File.OpenRead(photoURL)));
                            }
                            catch(Exception ex)
                            {
                                Log?.Invoke($"\n\n !!!Возникла ошибка {BotName.Name}  " + ex.Message + "\n\n");
                            }
                        }
                        return;
                    }            
                    if (e.Message.Text == personalConfig.Buttons["LOCATION"])
                    {
                        await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["LOCATION"]);
                        await bot.SendLocationAsync(currentUser.ChatId, personalConfig.Latitude, personalConfig.Longitude, replyMarkup: UserKeyboard.GetStartKeyboard(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["LINK"])
                    {
                        await bot.SendTextMessageAsync(currentUser.ChatId, "👇", replyMarkup: UserKeyboard.GetLinkButtons(options));
                        return;
                    }
                    if (e.Message.Text == "/reg" + botConfig.Password && admin == null)
                    {
                        await dateFunction.CreateMonthsAsync();
                        await context.db.AddMonthAsync(dateFunction.CurrentMonth);
                        await context.db.CreateDaysAsync(dateFunction.CurrentDay, dateFunction.CurrentMonth, dateFunction.DayNames);
                        List<Day> daysCurrentMonth = await context.db.FindDaysAsync(dateFunction.CurrentMonth.MonthId);
                        foreach (Day day in daysCurrentMonth)
                        {
                            for (int i = 0; i < botConfig.appointmentStandartCount; i++)
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
                            Log?.Invoke($"Ошибка при регистрации");
                            return;
                        }
                        await AdminLog?.Invoke(new EventArgsNotification(admin.ChatId, "Зарегестрированно!", AdminKeyboard.GetKeyboardAdmin(options)));

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
                                    await bot.SendTextMessageAsync(admin.ChatId, $"Добавилась запись на {app.AppointmentTime}", replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
                                    return;
                                }
                                else
                                {
                                    await context.db.DeleteAppAsync(app);
                                    app = null;
                                    await bot.SendTextMessageAsync(admin.ChatId, $" запись удалена", replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
                                    return;
                                }                                  
                            }
                            else
                            {
                                await bot.SendTextMessageAsync(admin.ChatId, $"Запись не добавилась. Ошибка ", replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
                                return;
                            }
                        }
                        if (admin.Status.Split('/')[0] == "Mailing")
                        {
                            lastMessage = e.Message;
                            admin.Status = "";
                            await context.db.UpdateUserAsync(admin);
                            await bot.SendTextMessageAsync(admin.ChatId, "Отправить это сообщение всем пользователям?", replyMarkup: AdminKeyboard.GetConfirmKeyboard(0, options, Codes.AdminMailingConfirm, 0));
                            return;
                        }
                        if (admin.Status.Split('/')[0] == "AddExample" && e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
                        {
                            admin.Status = "";
                            await context.db.UpdateUserAsync(admin);
                            string fileId = e.Message.Photo[e.Message.Photo.Length - 1].FileId;            
                            ImageManager.AddWorkExample?.Invoke(new EventArgsUpdate(this, e.Message.Photo[e.Message.Photo.Length-1].FileId));
                            return;                          
                        }
                    }                     
                    if (e.Message.Text == "/start")
                    {
                        await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["ADMINGREETING"], replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["ADDAPP"])
                    {
                        List<Month> month = await context.db.GetMonthsAsync();
                        await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["ADMINSELECTMONTH"], replyMarkup: CommonKeyboard.GetMonthButtons(month, Codes.AdminAdd, currentUser));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["DELAPP"])
                    {
                        List<Month> months = await context.db.GetMonthsAsync();
                        await bot.SendTextMessageAsync(admin?.ChatId, personalConfig.Messages["ADMINSELECTMONTH"], replyMarkup: CommonKeyboard.GetMonthButtons(months, Codes.AdminDelete, admin));
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
                        await bot.SendTextMessageAsync(admin.ChatId, "Это все пользователи", replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["MAKEWEEKEND"])
                    {
                        List<Month> months = await context.db.GetMonthsAsync();
                        await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINSELECTMONTH"], replyMarkup: CommonKeyboard.GetMonthButtons(months, Codes.AdminWeekEnd, admin));
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
                                $"Записался {user.ToString()}", replyMarkup: AdminKeyboard.GetConfirmKeyboard(app.AppointmentId, options, Codes.AdminConfirm, user.UserId));
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
                                $"Записался {user.ToString()}", replyMarkup: AdminKeyboard.GetCanccelButton(app.AppointmentId, options, Codes.AdminCancel, user.UserId));
                        }
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["OPTIONS"])
                    {
                        await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINOPTIONS"], replyMarkup: AdminKeyboard.GetOptionsKeyboard(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["MAILING"])
                    {
                        admin.Status = "Mailing";
                        await context.db.UpdateUserAsync(admin);
                        await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINMAILING"]);
                        return;
                    }
                    //                   |
                    // TODO: Check that \|/
                    // Настройки         |
                    if (e.Message.Text == personalConfig.AdminButtons["ADDEXAMPLE"])
                    {
                        admin.Status = "AddExample";
                        await context.db.UpdateUserAsync(admin);
                        await bot.SendTextMessageAsync(admin.ChatId, $"Отправте следующим сообщением фото, которое нужно добавить в портфолио");
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["DELETEEXAMPLE"])
                    if (e.Message.Text == personalConfig.AdminButtons["GREETING"])
                    if (e.Message.Text == personalConfig.AdminButtons["PRICE"])

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
            CallBackDataMasters data = await CallBackDataMasters.GetDataAsync(e.CallbackQuery.Data);
            if (currentUser == null)
            {
                Log?.Invoke($"Пользователь NULL при нажатии кнопки {e.CallbackQuery.Data}");
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
                        await bot.SendTextMessageAsync(chatId, personalConfig.Messages["CHOSEDAY"], replyMarkup: CommonKeyboard.GetDaysButton(days, options, Codes.UserChoise, currentUser));
                        return;
                    }
                    if (data.Stage == Stage.Day && data.Action == CallBackData.Action.Choise)
                    {
                        Day day = await context.db.FindDayAsync(data.EntityId);
                        if (!day.IsWorkDay)
                        {
                            await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["ISWEEKENDFORUSER"], replyMarkup: UserKeyboard.GetStartKeyboard(options));
                            return;
                        }
                        List<Appointment> apps = await context.db.FindAppointmentsAsync(data.EntityId);
                        await bot.SendTextMessageAsync(chatId, personalConfig.Messages["CHOSEAPP"], replyMarkup: CommonKeyboard.GetAppointmentKeyboard(apps, options, Codes.UserChoise, currentUser.UserId));
                        return;
                    }
                    if (data.Stage == Stage.Appointment && data.Action == CallBackData.Action.Choise)
                    {
                        Appointment app = await context.db.FindAppointmentAsync(data.EntityId);
                        if (!app.IsEmpty)
                        {
                            await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["SORRYTAKEN"], replyMarkup: UserKeyboard.GetStartKeyboard(options));
                            return;
                        }
                        app.IsConfirm = false;
                        app.IsEmpty = false;
                        app.User = currentUser.UserId;
                        Day day = await context.db.FindDayAsync(app.Day);
                        string message = $"{personalConfig.Messages["NEWAPP"]}\n " +
                            $"{currentUser.FirstName} {currentUser.LastName} @{currentUser.Username}\n в {day.DayOfWeek}, {day.Date}.{day.MonthNumber} числа\n на время - {app.AppointmentTime}";
                        await context.db.UpdateAppAsync(app);
                        await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["WAITCINFIRM"], replyMarkup: UserKeyboard.GetStartKeyboard(options));
                        await bot.SendTextMessageAsync(admin.ChatId, message, replyMarkup:AdminKeyboard.GetConfirmKeyboard(app.AppointmentId, options, Codes.AdminConfirm, currentUser.UserId));
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
                            await bot.SendTextMessageAsync(confirmedUser.ChatId, personalConfig.Messages["YOURAPPCONFIRM"], replyMarkup: UserKeyboard.GetStartKeyboard(options));
                        }
                        if (data.Stage == Stage.No)
                        {
                            Appointment app = await context.db.FindAppointmentAsync(data.EntityId);
                            app.IsEmpty = true;
                            app.User = 0;
                            await context.db.UpdateAppAsync(app);
                            var confirmedUser = await context.db.FindUserAsync(data.UserId);
                            await bot.SendTextMessageAsync(confirmedUser.ChatId, personalConfig.Messages["YOURAPPNOTCONFIRM"], replyMarkup: UserKeyboard.GetStartKeyboard(options));
                        }
                    }
                    if (data.Action == CallBackData.Action.Add)
                    {
                        if (data.Stage == CallBackData.Stage.Month)
                        {
                            List<Day> days = await context.db.FindDaysAsync(data.EntityId);
                            var user = await context.db.FindUserAsync(data.UserId);
                            await bot.SendTextMessageAsync(user.ChatId, personalConfig.Messages["ADMINCHOISEDAY"], replyMarkup: CommonKeyboard.GetDaysButton(days, options, Codes.AdminAdd, admin));
                            return;
                        }
                        if (data.Stage == CallBackData.Stage.Day)
                        {
                            Appointment app = await context.db.AddAppointmentAsync(data.EntityId);
                            admin.Status = $"AddApp/{app.AppointmentId}";
                            await context.db.UpdateUserAsync(admin);
                            await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["WRITEAPPTIME"], replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
                            return;
                        }
                    }
                    if (data.Action == CallBackData.Action.Delete)
                    {
                        if (data.Stage == Stage.Month)
                        {
                            List<Day> days = await context.db.FindDaysAsync(data.EntityId);
                            await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINCHOISEDAY"], replyMarkup: CommonKeyboard.GetDaysButton(days, options, Codes.AdminDelete, admin));
                            return;
                        }
                        if (data.Stage == Stage.Day)
                        {
                            List<Appointment> apps = await context.db.FindAppointmentsAsync(data.EntityId);
                            await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINCHOISEAPP"], replyMarkup: CommonKeyboard.GetAppointmentKeyboard(apps, options, Codes.AdminDelete, admin.UserId));
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
                                replyMarkup: AdminKeyboard.GetConfirmKeyboard(app.AppointmentId, options, Codes.AdminDelete, userIDtoSend));
                            return;
                        }
                        if (data.Stage == Stage.Yes)
                        {
                            var app = await context.db.FindAppointmentAsync(data.EntityId);
                            var user = await context.db.FindUserAsync(data.UserId);
                            var day = await context.db.FindDayAsync(app.Day);
                            await bot.SendTextMessageAsync(admin.ChatId, $"Удалена запись", replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
                            await bot.SendTextMessageAsync(user.ChatId, $"Запись на {day.DayOfWeek} {day.Date}.{day.MonthNumber}\n" +
                                $"на время - {app.AppointmentTime} -Отменена");
                            await context.db.DeleteAppAsync(app);
                        }
                        if (data.Stage == Stage.No)
                        {
                            await bot.SendTextMessageAsync(admin.ChatId, "Отменено", replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
                        }

                    }
                    if (data.Action == CallBackData.Action.WeekEnd)
                    {
                        if (data.Stage == Stage.Month)
                        {
                            List<Day> days = await context.db.FindDaysAsync(data.EntityId);
                            await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINCHOISEDAY"], replyMarkup: CommonKeyboard.GetDaysButton(days, options, Codes.AdminWeekEnd, admin));
                            return;
                        }
                        if (data.Stage == Stage.Day)
                        {
                            Day day = await context.db.FindDayAsync(data.EntityId);
                            if (!day.IsWorkDay)
                            {
                                day.IsWorkDay = true;
                                await context.db.UpdateDayAsync(day);
                                await bot.SendTextMessageAsync(admin.ChatId, $"День {day.Date}.{day.MonthNumber} теперь рабочий день\n", replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
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
                                                $" и {(appointment.IsConfirm ? "подтверждена " : "не подтверждена ")}.";
                                            await bot.SendTextMessageAsync(admin.ChatId, message);
                                        }
                                    }
                                    await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["WEEKENDWARNING"], replyMarkup: AdminKeyboard.GetConfirmKeyboard(day.DayId, options, Codes.AdminWeekEnd, admin.UserId));
                                    return;
                                }
                            }
                            if (AllEmpty)
                            {
                                day.IsWorkDay = false;
                                await context.db.UpdateDayAsync(day);
                                await bot.SendTextMessageAsync(admin.ChatId, $"День {day.Date}.{day.MonthNumber} теперь выходной\n", replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
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
                            await bot.SendTextMessageAsync(admin.ChatId, $"День {day.Date}.{day.MonthNumber} теперь выходной\n", replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
                            return;
                        }
                        if (data.Stage == Stage.No)
                        {
                            await bot.SendTextMessageAsync(admin.ChatId, "Отменено", replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
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
                        await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINAPPISCANCEL"], replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
                        var user = await context.db.FindUserAsync(data.UserId);
                        await bot.SendTextMessageAsync(user.ChatId, personalConfig.Messages["USERAPPISCANCEL"]);
                        return;
                    }
                    if (data.Action == CallBackData.Action.Mailing)
                    {
                        if (lastMessage == null)
                        {
                            await bot.SendTextMessageAsync(admin.ChatId, "Не выбрано сообщение", replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
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
                            await bot.SendTextMessageAsync(admin.ChatId, "Отменено", replyMarkup: AdminKeyboard.GetKeyboardAdmin(options));
                        }
                        lastMessage = null;
                        return;
                    }
                }
            }
            if (data.Error == "404")
            {
                Log?.Invoke($"Ошибка 404 {e.CallbackQuery.Data}");
                await bot.SendTextMessageAsync(chatId, "Какая-то ошибка(( попробуйте еще раз", replyMarkup: UserKeyboard.GetStartKeyboard(options));
            }
            if (data.Error == "405")
            {
                Log?.Invoke($"Ошибка 405 {e.CallbackQuery.Data}");
                await bot.SendTextMessageAsync(chatId, "Ошибка парсинга данных(( попробуйте еще раз", replyMarkup: UserKeyboard.GetStartKeyboard(options));
            }
            
        }

        public  void  BotStart()
        {
            Log?.Invoke($"Бот {BotName.Name} запущен");
            bot.StartReceiving();
            Task.Run(() => StartUpdateDays());
            Task.Run(() => StartNotificationAsync()); 
        }
        public  void BotStop()
        {
            bot.StopReceiving();
        }     
        public BotName GetName()
        {
            return this.BotName;
        }        
        public async Task<Telegram.Bot.Types.File> GetFileAsync(string field)
        {
            return await bot.GetFileAsync(field);
        }
        public async Task DownLoadFileAsync(string filePath, Stream destination)
        {
            await bot.DownloadFileAsync(filePath, destination);
        }
    }
}