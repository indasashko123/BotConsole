using BotLibary.CallBackData;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace BotLibary.Interfaces
{
    class OnQueryListener : AbstractBot
    {
        public OnQueryListener(Bot bot)
        {
            this.ConnectBotData(bot);
        }
        internal virtual async void OnQuery(object sender, CallbackQueryEventArgs e)
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
                            await bot.SendTextMessageAsync(data.UserId, personalConfig.Messages["ADMINCHOISEDAY"], replyMarkup: KeyBoards.GetDaysButton(days, options, Codes.AdminAdd, admin));
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
                            await bot.SendTextMessageAsync(admin.ChatId, $"Удалена запись", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
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
                                                $" и {(appointment.IsConfirm ? "подтверждена " : "не подтверждена ")}.";
                                            await bot.SendTextMessageAsync(admin.ChatId, message);
                                        }
                                    }
                                    await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["WEEKENDWARNING"], replyMarkup: KeyBoards.GetConfirmKeyboard(day.DayId, options, Codes.AdminWeekEnd, admin.UserId));
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
                        await bot.SendTextMessageAsync(admin.ChatId, personalConfig.Messages["ADMINAPPISCANCEL"], replyMarkup: KeyBoards.GetKeyboardAdmin(options));
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
    }
}
