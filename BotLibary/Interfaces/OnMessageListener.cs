using BotLibary.Events;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;

namespace BotLibary.Interfaces
{
    class OnMessageListener : AbstractBot
    {
        public OnMessageListener(Bot bot)
        {
            this.ConnectBotData(bot);  
        }
        internal virtual async void onMessage(object sender, MessageEventArgs e)
        {
            if (e.Message != null && e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text && !string.IsNullOrEmpty(e.Message.Text))
            {
                DataBase.Models.User admin = await context.db.FindAdminAsync();
                DataBase.Models.User currentUser = await context.db.FindUserAsync(e.Message.Chat.Id);
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
                        await bot.SendTextMessageAsync(currentUser.ChatId, "Для записи необходимо выбрать месяц", replyMarkup: KeyBoards.GetMonthButtons(await context.db.GetMonthsAsync(), Codes.UserChoise, currentUser));
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
                    if (e.Message.Text == "/reg" + botConfig.password && admin == null)
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
                        await bot.SendTextMessageAsync(admin.ChatId, "Отправить это сообщение всем пользователям?", replyMarkup: KeyBoards.GetConfirmKeyboard(0, options, Codes.AdminMailingConfirm, 0));
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
    }
}
