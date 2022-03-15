using BotLibary.TelegramBot.Handlers.Commands;
using DataBase.Database;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using BotLibary.TelegramBot.ReplyMarkups;
using Options;

namespace BotLibary.TelegramBot.Handlers.AdminCommands
{
    class CreateMailingCommand : ICommand
    {
        Bot client;
        DataBase.Models.User admin;
        DataBaseConnector context;
        BotOptions options;
        public CreateMailingCommand(Bot botClient, DataBase.Models.User admin, BotOptions options, DataBaseConnector context)
        {
            client = botClient;
            this.admin = admin;
            this.context = context;
            this.options = options;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            client.lastMessage = message;
            admin.Status = "";
            await context.db.UpdateUserAsync(admin);
            return await bot.SendTextMessageAsync(admin.ChatId, "Отправить это сообщение всем пользователям?", replyMarkup: KeyBoards.GetConfirmKeyboard(0, options, Codes.AdminMailingConfirm, 0));
           
        }
    }
}
