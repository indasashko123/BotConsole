using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Requests;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using DataBase.Database;
using DataBase.Database.Context.MySQL;
using DataBase.Database.Interface;
using System.Threading;
using Options;
using DataBase.Models;
using BotLibary.Events;
using Telegram.Bot.Types.InputFiles;
using BotLibary.CallBackData;
using BotLibary.Interfaces;

namespace BotLibary
{
    
    public class Bot : AbstractBot, IBot
    {             
        public Bot(BotOptions options)
        {
            this.options = options;
            botConfig = options.botConfig;
            personalConfig = options.personalConfig;
            this.bot = new TelegramBotClient(botConfig.token);

            bot.OnMessage += new OnMessageListener(this).onMessage;
            bot.OnCallbackQuery += new OnQueryListener(this).OnQuery;

            dateFunction = new DateFunction();
            context = new DataBaseConnector(new SQLContext(botConfig.dataBaseName));

            adminMessage += (async(EventArgsNotification e) => { await bot.SendTextMessageAsync(e.Admin,e.Text, replyMarkup:e?.Keyboard); });
        }

        
       
        /// <summary>
        ///  Запуск бота
        /// </summary>
        public  void  BotStart()
        {
            bot.StartReceiving();
            Task.Run(() => StartUpdateDays());
            Task.Run(() => StartNotificationAsync()); 
        }
        /// <summary>
        /// Остановка бота
        /// </summary>
        public  void BotStop()
        {
            bot.StopReceiving();
        }
        
    }
}
