using BotLibary.TelegramBot.CallBackData;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using DataBase.Models;
using Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands.AdminCommand
{
    class AdminChoiseAppDeleteCommand : ICallBackCommand
    {
        DataBaseConnector context;
        BotOptions options;
        CallBack callBack;
        DataBase.Models.User admin;
        public AdminChoiseAppDeleteCommand(DataBase.Models.User admin, DataBaseConnector context, BotOptions options, CallBack callBack)
        {
            this.admin = admin;
            this.context = context;
            this.options = options;
            this.callBack = callBack;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            Appointment app = await context.db.Reader.FindAppointmentAsync(callBack.EntityId);
            int userIDtoSend = app.User;
            if (app.IsEmpty)
            {
                userIDtoSend = admin.UserId;
            }
            return await bot.SendTextMessageAsync(admin.ChatId, "Подтверждение удаления записи",
                replyMarkup: KeyBoards.GetConfirmKeyboard(app.AppointmentId, options, Codes.AdminDelete, userIDtoSend));
            
        }
    }
}
