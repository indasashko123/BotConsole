using BotLibary.Events;
using BotLibary.TelegramBot.ReplyMarkups;
using Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotLibary.TelegramBot.Handlers.Commands
{
    class PriceCommand : ICommand
    {
        private DataBase.Models.User currentUser;
        private BotOptions options;
        public event ChangesLog consoleMessage;
        public PriceCommand(DataBase.Models.User currentUser, BotOptions options, ChangesLog consoleMessage)
        {
            this.consoleMessage += (string text) => consoleMessage?.Invoke(text);
            this.options = options;
            this.currentUser = currentUser;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            try
            {
                return await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(System.IO.File.OpenRead(options.personalConfig.Paths["PRICE"])), replyMarkup: KeyBoards.GetStartKeyboard(options));
            }
            catch (Exception ex)
            {
              consoleMessage?.Invoke($"\n\n !!!Возникла ошибка {options.botConfig.Name}  " + ex.Message + "\n\n");
            }
            return null;
        }
    }
}
