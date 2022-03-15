
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
    class AdminCancelAppCommand : ICallBackCommand
    {
        DataBaseConnector context;
        DataBase.Models.User admin;
        CallBack callback;
        BotOptions options;
        public AdminCancelAppCommand(DataBase.Models.User admin, DataBaseConnector context, BotOptions options, CallBack callBack)
        {
            this.admin = admin;
            this.context = context;
            this.options = options;
            callback = callBack;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            Appointment app = await context.db.FindAppointmentAsync(callback.EntityId);
            app.IsConfirm = false;
            app.User = 0;
            app.IsEmpty = true;
            await context.db.UpdateAppAsync(app);
            await bot.SendTextMessageAsync(admin.ChatId,
                                           $"Запись на время {app.AppointmentTime} отменена", 
                                           replyMarkup: KeyBoards.GetKeyboardAdmin(options));
            var user = await context.db.FindUserAsync(callback.UserId);
            await bot.SendTextMessageAsync(user.ChatId, 
                                           options.personalConfig.Messages["USERAPPISCANCEL"]);
            return null;
        }
    }
}
