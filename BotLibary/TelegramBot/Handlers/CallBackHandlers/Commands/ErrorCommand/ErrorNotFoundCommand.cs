using BotLibary.TelegramBot.ReplyMarkups;
using Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands.ErrorCommand
{
    class ErrorNotFoundCommand : ICallBackCommand
    {
        DataBase.Models.User currentUser;
        BotOptions options;
        public ErrorNotFoundCommand(DataBase.Models.User currentUser, BotOptions options)
        {
            this.currentUser = currentUser;
            this.options = options;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            return await bot.SendTextMessageAsync(currentUser.ChatId, "Какая-то ошибка(( попробуйте еще раз", replyMarkup: KeyBoards.GetStartKeyboard(options));
        }
    }
}
