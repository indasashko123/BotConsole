using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands
{
    interface ICallBackCommand
    {
        public Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery);
    }
}
