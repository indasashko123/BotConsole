﻿using BotLibary.TelegramBot.CallBackData;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using DataBase.Models;
using Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands.AdminCommand
{
    class AdminChoiseMonthCommand : ICallBackCommand
    {
        DataBaseConnector context;
        CallBack callBack;
        BotOptions options;
        DataBase.Models.User admin;
        public AdminChoiseMonthCommand(DataBase.Models.User admin, DataBaseConnector context, BotOptions options, CallBack callBack)
        {
            this.admin = admin;
            this.context = context;
            this.options = options;
            this.callBack = callBack;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            List<Day> days = await context.db.Reader.FindDaysAsync(callBack.EntityId);
            var user = await context.db.Reader.FindUserAsync(callBack.UserId);
            return await bot.SendTextMessageAsync(user.ChatId, "Выберите день, в который нужно добавить запись", 
                                           replyMarkup: KeyBoards.GetDaysButton(days, options, Codes.AdminAdd, admin));        
        }
    }
}