using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace BotLibary.TelegramBot.Handlers.Commands
{
    public interface ICommand
    {
        public Task<Message> ReturnCommand(ITelegramBotClient bot, Message message);
    }
}
