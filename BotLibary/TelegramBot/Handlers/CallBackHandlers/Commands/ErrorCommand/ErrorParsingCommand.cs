using BotLibary.TelegramBot.ReplyMarkups;
using Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands.ErrorCommand
{
    class ErrorParsingCommand : ICallBackCommand
    {
        DataBase.Models.User currentUser;
        BotOptions options;
        public ErrorParsingCommand(DataBase.Models.User currentUser, BotOptions options)
        {
            this.currentUser = currentUser;
            this.options = options;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            return await bot.SendTextMessageAsync(currentUser.ChatId,
                                                  "Ошибка парсинга данных(( попробуйте еще раз", 
                                                  replyMarkup: KeyBoards.GetStartKeyboard(options));
        }
    }
}
