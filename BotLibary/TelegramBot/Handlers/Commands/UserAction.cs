using BotLibary.Events;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.Commands
{
    class UserAction
    {
        private Bot bot;
        public event ChangesLog consoleMessage;
        public event AdminMessage adminMessage;
        public UserAction(Bot bot, ChangesLog consoleMessage, AdminMessage adminMessage)
        {
            this.adminMessage += (EventArgsNotification e) => adminMessage?.Invoke(e);
            this.consoleMessage += (string text) => consoleMessage?.Invoke(text);
            this.bot = bot;
        }
        public async Task<Message> GetAction(DataBase.Models.User currentUser,DataBase.Models.
                                             User admin,string data, ITelegramBotClient botClient,
                                             Message message )
        {
            if (data == "/start" || data == bot.personalConfig.Buttons["ABOUT"])
            {
                return await new GreatingCommand(currentUser, bot.options, consoleMessage).ReturnCommand(botClient,message);
            }
            if(data == bot.personalConfig.Buttons["APPOINTMENT"])
            {
                return await new AppointmentCommand(admin,currentUser,bot.options,bot.context).ReturnCommand(botClient, message);
            }
            if(data == bot.personalConfig.Buttons["PRICE"])
            {
                return await new PriceCommand(currentUser, bot.options, consoleMessage).ReturnCommand(botClient, message);
            }
            if(data == bot.personalConfig.Buttons["FEEDBACK"])
            {
                return await new FeedBackCommand(currentUser, bot.options).ReturnCommand(botClient, message);
            }
            if(data == bot.personalConfig.Buttons["MYWORKS"])
            {
                return await new GetPartfolioCommand(currentUser, bot.options, consoleMessage).ReturnCommand(botClient, message);
            }
            if(data == bot.personalConfig.Buttons["LOCATION"])
            {
                return await new LocationCommand(currentUser, bot.options).ReturnCommand(botClient, message);
            }
            if (data == bot.personalConfig.Buttons["LINK"])
            {
                return await new LinkCommand(currentUser, bot.options).ReturnCommand(botClient, message);
            }
            if (data == "/reg" + bot.botConfig.password && admin == null)
            {
                return await new RegistrationCommand(
                    bot.dateFunction,
                    bot.context,
                    bot.options,
                    currentUser,
                    admin,
                    consoleMessage,
                    adminMessage
                    ).ReturnCommand(botClient, message);
            }
            return await new UnknownCommand().ReturnCommand(botClient, message);
        }
        
    }
}
