
using BotLibary.TelegramBot.Handlers.Commands;
using BotLibary.TelegramBot.ReplyMarkups;
using Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.AdminCommands
{
    class AdminGreatingCommad : ICommand
    {
        BotOptions options;
        DataBase.Models.User admin;
        public AdminGreatingCommad(BotOptions options, DataBase.Models.User admin)
        {
            this.options = options;
            this.admin = admin;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            return await bot.SendTextMessageAsync(admin.ChatId, $"Привет, мастер {admin.FirstName}", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
            
        }
    }
}
