using BotLibary.TelegramBot.ReplyMarkups;
using Options;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.Commands
{
    class LinkCommand : ICommand
    {
        private DataBase.Models.User currentUser;
        private BotOptions options;
        public LinkCommand(DataBase.Models.User currentUser, BotOptions options)
        {
            this.options = options;
            this.currentUser = currentUser;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            return await bot.SendTextMessageAsync(currentUser.ChatId, "👇", replyMarkup: KeyBoards.GetLinkButtons(options));        
        }
    }
}
