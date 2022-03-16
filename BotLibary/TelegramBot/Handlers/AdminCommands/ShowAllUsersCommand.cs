using BotLibary.TelegramBot.Handlers.Commands;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace BotLibary.TelegramBot.Handlers.AdminCommands
{
    class ShowAllUsersCommand : ICommand
    {
        private DataBase.Models.User admin;
        private DataBaseConnector context;
        private BotOptions options;
        public ShowAllUsersCommand(DataBase.Models.User admin, DataBaseConnector context, BotOptions options)
        {
            this.admin = admin;
            this.context = context;
            this.options = options;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            List<DataBase.Models.User> users = await context.db.Reader.FindUsersAsync();
            foreach (var user in users)
            {
                string Message = $"Пользователь {user.FirstName} {user.LastName} @{user.Username}\n";
                await bot.SendTextMessageAsync(admin.ChatId, Message);
            }
            return await bot.SendTextMessageAsync(admin.ChatId, "Это все пользователи", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
            
        }
    }
}
