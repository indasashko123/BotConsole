using BotLibary.TelegramBot.CallBackData;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using DataBase.Models;
using Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands
{
    class UserChoiseMonthCommand : ICallBackCommand
    {
        DataBaseConnector context;
        DataBase.Models.User currentUser;
        CallBack callBack;
        BotOptions options;
        string MessageText;
        string Code;
        public UserChoiseMonthCommand(DataBaseConnector context, DataBase.Models.User currentUser, 
                                      CallBack callBack, BotOptions options, string MessageText, string Code)
        {
            this.context = context;
            this.currentUser = currentUser;
            this.callBack = callBack;
            this.options = options;
            this.MessageText = MessageText;
            this.Code = Code;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            List<Day> days = await context.db.Reader.FindDaysAsync(callBack.EntityId);
            return await bot.SendTextMessageAsync(currentUser.ChatId, MessageText, 
                                                  replyMarkup: KeyBoards.GetDaysButton(days, options, 
                                                                                       Code, currentUser));        
        }
    }
}
