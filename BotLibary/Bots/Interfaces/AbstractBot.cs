using BotLibary.Bots.Events;
using DataBase.Database;
using DataBase.Database.Context.MySQL;
using DataBase.Models;
using Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.Bots.Interfaces
{
    public abstract class AbstractBot
    {
        public AbstractBot()
        {
        }        
        protected internal TelegramBotClient bot;
        protected internal BotOptions options;
        protected internal BotConfig botConfig;
        protected internal PersonalConfig personalConfig;
        protected internal Message lastMessage;       
        protected internal DataBaseConnector context;
        protected internal BotName BotName {get;set;}
        


    }
}
