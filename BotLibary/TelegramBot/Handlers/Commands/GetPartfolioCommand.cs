using BotLibary.Events;
using Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotLibary.TelegramBot.Handlers.Commands
{
    class GetPartfolioCommand : ICommand
    {
        private BotOptions options;
        private DataBase.Models.User currentUser;
        public event ChangesLog consoleMessage;
        public GetPartfolioCommand(DataBase.Models.User currentUser, BotOptions options, ChangesLog consoleMessage)
        {
            this.consoleMessage += (string text) => consoleMessage?.Invoke(text);
            this.currentUser = currentUser;
            this.options = options;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            foreach (var photoURL in options.personalConfig.Partfolio)
            {
                try
                {
                    await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(System.IO.File.OpenRead(photoURL)));
                }
                catch
                {
                    consoleMessage?.Invoke($"Не загрузилось порфолио");
                }
                
            }
            return null;
        }
    }
}
