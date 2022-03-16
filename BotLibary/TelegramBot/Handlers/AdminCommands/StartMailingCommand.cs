
using BotLibary.TelegramBot.Handlers.Commands;
using DataBase.Database;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.AdminCommands
{
    class StartMailingCommand : ICommand
    {
        private DataBase.Models.User admin;
        private DataBaseConnector context;
        public StartMailingCommand(DataBase.Models.User admin, DataBaseConnector context)
        {
            this.admin = admin;
            this.context = context;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            admin.Status = "Mailing";
            await context.db.Updater.UpdateUserAsync(admin);
            await bot.SendTextMessageAsync(admin.ChatId, "Следующее записанное сообщение будет отправленно. Может быть текст, фото, голосовое и т.д.");
            return null;
        }
    }
}
