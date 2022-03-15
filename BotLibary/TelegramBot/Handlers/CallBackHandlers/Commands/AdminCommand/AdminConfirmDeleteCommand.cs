using BotLibary.TelegramBot.CallBackData;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands.AdminCommand
{
    class AdminConfirmDeleteCommand : ICallBackCommand
    {
        DataBaseConnector context;
        BotOptions options;
        CallBack callBack;
        DataBase.Models.User admin;
        public AdminConfirmDeleteCommand(DataBase.Models.User admin, DataBaseConnector context, BotOptions options, CallBack callBack)
        {
            this.admin = admin;
            this.context = context;
            this.options = options;
            this.callBack = callBack;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            var app = await context.db.FindAppointmentAsync(callBack.EntityId);
            var user = await context.db.FindUserAsync(callBack.UserId);
            var day = await context.db.FindDayAsync(app.Day);
            await bot.SendTextMessageAsync(admin.ChatId, $"Удалена запись", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
            if (user != null)
            {
                await bot.SendTextMessageAsync(user.ChatId, $"Запись на {day.DayOfWeek} {day.Date}.{day.MonthNumber}\n" +
                $"на время - {app.AppointmentTime} -Отменена");
            }
            await context.db.DeleteAppAsync(app);
            return null;
        }
    }
}
