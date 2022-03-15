using BotLibary.Events;
using BotLibary.TelegramBot.Handlers.Commands;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.AdminCommands
{
    class AdminAction
    {
        private Bot bot;
        public event ChangesLog consoleMessage;
        public event AdminMessage adminMessage;
        public AdminAction(Bot bot, ChangesLog consoleMessage, AdminMessage adminMessage)
        {
            this.consoleMessage += (string text) => consoleMessage?.Invoke(text);
            this.adminMessage += (EventArgsNotification e) => adminMessage?.Invoke(e);
            this.bot = bot;
        }
        public async Task<Message> GetAction(DataBase.Models.User admin, string data, ITelegramBotClient botClient, Message message)
        {
            if (!string.IsNullOrWhiteSpace(admin.Status))
            {
                if (admin.Status.Split('/')[0] == "AddApp")
                {
                    return await new AddAppointmentTimeCommand(admin,bot.context,bot.options, data).ReturnCommand(botClient, message);
                }
                if (admin.Status.Split('/')[0] == "Mailing")
                {
                    return await new CreateMailingCommand(bot, admin, bot.options, bot.context).ReturnCommand(botClient, message);
                }                
            }
            if (data == "/start")
            {
                return await new AdminGreatingCommad(bot.options, admin).ReturnCommand(botClient, message);
            }
            if (data == bot.personalConfig.AdminButtons["ADDAPP"] 
                || data == bot.personalConfig.AdminButtons["DELAPP"]
                || data == bot.personalConfig.AdminButtons["MAKEWEEKEND"])
            {
                string code = bot.personalConfig.AdminButtons["ADDAPP"] == data ? Codes.AdminAdd : Codes.AdminDelete;
                code = bot.personalConfig.AdminButtons["MAKEWEEKEND"] == data ? Codes.AdminWeekEnd : code;
                return await new MonthChoiseCommand(admin, bot.context, code).ReturnCommand(botClient, message);
            }
            if (data == bot.personalConfig.AdminButtons["ALLUSERS"])
            {
                return await new ShowAllUsersCommand(admin,bot.context, bot.options).ReturnCommand(botClient, message);
            }
            if (data == bot.personalConfig.AdminButtons["LOOKNOTCONFIRM"]
                || data == bot.personalConfig.AdminButtons["LOOKCONFIRM"])
            {
                bool target = data == bot.personalConfig.AdminButtons["LOOKNOTCONFIRM"] ? false : true;
                return await new LookConfirmCommand(admin, bot.context, bot.options, target).ReturnCommand(botClient, message);
            }
            if (data == bot.personalConfig.AdminButtons["MAILING"])
            {
                return await new StartMailingCommand(admin, bot.context).ReturnCommand(botClient, message);
            }
            return await new UnknownCommand().ReturnCommand(botClient, message);
        }
     }
}
