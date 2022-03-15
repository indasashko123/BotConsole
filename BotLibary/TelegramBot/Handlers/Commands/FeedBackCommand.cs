using BotLibary.TelegramBot.ReplyMarkups;
using Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.Commands
{
    class FeedBackCommand : ICommand
    {
        DataBase.Models.User currentUser;
        BotOptions options;
        public FeedBackCommand(DataBase.Models.User currentUser, BotOptions options)
        {
            this.currentUser = currentUser;
            this.options = options;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            await bot.SendTextMessageAsync(currentUser.ChatId, options.personalConfig.Messages["FEEDBACK"]);
            return await bot.SendTextMessageAsync(currentUser.ChatId, "👇", replyMarkup: KeyBoards.GetInstagrammButton(options));  
        }
    }
}
