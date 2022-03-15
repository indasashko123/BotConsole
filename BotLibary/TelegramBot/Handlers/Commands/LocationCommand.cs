using BotLibary.TelegramBot.ReplyMarkups;
using Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.Commands
{
    class LocationCommand : ICommand
    {
        private DataBase.Models.User currentUser;
        private BotOptions options;
        public LocationCommand(DataBase.Models.User currentUser, BotOptions options)
        {
            this.options = options;
            this.currentUser = currentUser;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            await bot.SendTextMessageAsync(currentUser.ChatId, options.personalConfig.Messages["LOCATION"]);
            return await bot.SendLocationAsync(currentUser.ChatId, options.personalConfig.Latitude, options.personalConfig.Longitude, replyMarkup: KeyBoards.GetStartKeyboard(options));   
        }
    }
}
