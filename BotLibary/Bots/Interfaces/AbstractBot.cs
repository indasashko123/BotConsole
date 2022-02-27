
using DataBase.Database;
using Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.Bots.Interfaces
{
    public abstract class AbstractBot
    {       
        protected internal TelegramBotClient bot;
        protected internal Message lastMessage;       
        protected internal DataBaseConnector context;
        protected internal BotName BotName {get;set;}
        


    }
}
