using BotLibary.TelegramBot.CallBackData;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using DataBase.Models;
using Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands.AdminCommand
{
    class AdminChoiseDayWeekEndCommand : ICallBackCommand
    {
        DataBaseConnector context;
        BotOptions options;
        DataBase.Models.User admin;
        CallBack callBack;
        public AdminChoiseDayWeekEndCommand(DataBase.Models.User admin, DataBaseConnector context, 
                                            BotOptions options, CallBack callBack)
        {
            this.admin = admin;
            this.context = context;
            this.options = options;
            this.callBack = callBack;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            Day day = await context.db.FindDayAsync(callBack.EntityId);
            if (!day.IsWorkDay)
            {
                day.IsWorkDay = true;
                await context.db.UpdateDayAsync(day);
                await bot.SendTextMessageAsync(admin.ChatId, $"День {day.Date}.{day.MonthNumber} теперь рабочий день\n", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                return null;
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
                return null;
            }
            if (AllEmpty)
            {
                day.IsWorkDay = false;
                await context.db.UpdateDayAsync(day);
                await bot.SendTextMessageAsync(admin.ChatId, $"День {day.Date}.{day.MonthNumber} теперь выходной\n", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                return null;
            }
            return null;
        }
    }
}
