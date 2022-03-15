using BotLibary.Events;
using BotLibary.TelegramBot.ReplyMarkups;
using Options;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotLibary.TelegramBot.Handlers.Commands
{
    internal class GreatingCommand : ICommand
    {
        DataBase.Models.User currentUser;
        public event ChangesLog consoleMessage;
        BotOptions options;
        public GreatingCommand(DataBase.Models.User currentUser,BotOptions options, ChangesLog consoleMessage)
        {
            this.consoleMessage += (string text) => consoleMessage?.Invoke(text);
            this.currentUser = currentUser;
            this.options = options;
        }
        public async Task<Message> ReturnCommand (ITelegramBotClient bot, Message message)
        {
            try
            {
                return await bot.SendPhotoAsync(
                    currentUser.ChatId, 
                    new InputOnlineFile(System.IO.File.OpenRead(options.personalConfig.Paths["GREETING"])), 
                    caption: options.personalConfig.Messages["USERGREETING"], 
                    replyMarkup: KeyBoards.GetStartKeyboard(options));
            }
            catch (Exception ex)
            {
              consoleMessage?.Invoke($"\n\n !!!Возникла ошибка при отправке фото {options.botConfig.Name}  " + ex.Message + "\n\n");
            }
            return null;
        }
    }
}
