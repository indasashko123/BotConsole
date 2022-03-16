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
    class AdminChoiseDayAddAppCommand : ICallBackCommand
    {
        DataBaseConnector context;
        DataBase.Models.User admin;
        CallBack callBack;
        BotOptions options;
        public AdminChoiseDayAddAppCommand(DataBase.Models.User admin, DataBaseConnector context, BotOptions options, CallBack callBack)
        {
            this.admin = admin;
            this.context = context;
            this.options = options;
            this.callBack = callBack;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            Appointment app = await context.db.Creater.AddAppointmentAsync(callBack.EntityId);
            admin.Status = $"AddApp/{app.AppointmentId}";
            await context.db.Updater.UpdateUserAsync(admin);
            await bot.SendTextMessageAsync(admin.ChatId, $"Напишите время приема.\n Не более 5 символов", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
            return null;
        }
    }
}
