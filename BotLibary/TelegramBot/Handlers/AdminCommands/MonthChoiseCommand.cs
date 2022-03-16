using BotLibary.TelegramBot.Handlers.Commands;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.AdminCommands
{
    class MonthChoiseCommand : ICommand
    {
        DataBaseConnector context;
        DataBase.Models.User admin;
        string code;
        public MonthChoiseCommand(DataBase.Models.User admin, DataBaseConnector context, string code)
        {
            this.admin = admin;
            this.context = context;
            this.code = code;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            List<DataBase.Models.Month> month = await context.db.Reader.GetMonthsAsync();
            return await bot.SendTextMessageAsync(admin.ChatId, $"Выбирите месяц", replyMarkup: KeyBoards.GetMonthButtons(month, code, admin));  
        }
    }
}
