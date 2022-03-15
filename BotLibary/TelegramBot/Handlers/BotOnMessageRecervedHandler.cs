using BotLibary.Events;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using BotLibary.TelegramBot.Handlers.Commands;
using BotLibary.TelegramBot.Handlers.AdminCommands;

namespace BotLibary.TelegramBot.Handlers
{
    internal class BotOnMessageRecervedHandler
    {
        private Bot bot;
        public event ChangesLog consoleMessage; 
        public event AdminMessage adminMessage;
        public BotOnMessageRecervedHandler(Bot bot, ChangesLog consoleMessage, AdminMessage adminMessage)
        {
            this.bot = bot;
            this.consoleMessage += (string text) => consoleMessage?.Invoke(text);
            this.adminMessage += (EventArgsNotification e) => adminMessage?.Invoke(e);
        }
        internal async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {      
            DataBase.Models.User admin = await bot.context.db.FindAdminAsync();
            DataBase.Models.User currentUser = await bot.context.db.FindUserAsync(message.Chat.Id);
            if (currentUser == null)
            {
               currentUser = await bot.context.db.CreateNewUserAsync(message.From.Username, message.From.FirstName, message.From.LastName, message.Chat.Id);
               consoleMessage?.Invoke($"Создали нового пользователя {currentUser} с ChatId {currentUser.ChatId}");
            }
            if (message.Type == MessageType.Text)
            {
                if (currentUser != admin && !currentUser.IsAdmin)
                {
                    var action = await new UserAction(bot, consoleMessage, adminMessage).
                        GetAction(currentUser, admin, message.Text!,
                        botClient, message);
                }
                else
                {
                    var action = await new AdminAction(bot, consoleMessage, adminMessage).
                        GetAction(admin, message.Text!,
                        botClient, message);
                }
            }
            else
            {
                if (admin != null && admin.Status == "Mailing")
                {
                    await new CreateMailingCommand(bot, admin, bot.options, bot.context).ReturnCommand(botClient, message);
                }
            }

        }
    }
}
