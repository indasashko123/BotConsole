using BotLibary.TelegramBot.Handlers.Commands;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using DataBase.Models;
using Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.AdminCommands
{
    class LookConfirmCommand : ICommand
    {
        private DataBase.Models.User admin;
        private DataBaseConnector context;
        private BotOptions options;
        bool target;
        public LookConfirmCommand(DataBase.Models.User admin, DataBaseConnector context, BotOptions options, bool target)
        {
            this.admin = admin;
            this.context = context;
            this.options = options;
            this.target = target;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            List<Appointment> apps = await context.db.FindConfirmAppointmentsAsync(target);
            if (apps == null || apps.Count == 0)
            {
                return await bot.SendTextMessageAsync(admin.ChatId, "Записей нет");
                
            }
            foreach (Appointment app in apps)
            {
                DataBase.Models.User user = await context.db.FindUserAsync(app.User);
                Day day = await context.db.FindDayAsync(app.Day);
                if (target)
                {
                await bot.SendTextMessageAsync(
                    admin.ChatId,
                    $"Запись на {day.Date}.{day.MonthNumber} на время {app.AppointmentTime}\n" +
                    $"Записался {user}",
                    replyMarkup: KeyBoards.GetCanccelButton(app.AppointmentId, options, Codes.AdminCancel, user.UserId));
                }
                else
                {
                await bot.SendTextMessageAsync(
                    admin.ChatId, 
                    $"Запись на {day.Date}.{day.MonthNumber} на время {app.AppointmentTime}\n" +
                    $"Записался {user}",
                    replyMarkup: KeyBoards.GetConfirmKeyboard(app.AppointmentId, options, Codes.AdminConfirm, user.UserId));
                }                
            }
            return null;
        }
    }
}
