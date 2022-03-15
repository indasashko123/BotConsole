﻿using BotLibary.TelegramBot.CallBackData;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using DataBase.Models;
using Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands.AdminCommand
{
    class AdminConfirmCommand : ICallBackCommand
    {
        DataBaseConnector context;
        CallBack callBack;
        BotOptions options;
        DataBase.Models.User admin;
        public AdminConfirmCommand(DataBase.Models.User admin, DataBaseConnector context, BotOptions options, CallBack callBack)
        {
            this.admin = admin;
            this.context = context;
            this.options = options;
            this.callBack = callBack;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            Appointment app = await context.db.FindAppointmentAsync(callBack.EntityId);
            app.IsConfirm = true;
            await context.db.UpdateAppAsync(app);
            var confirmedUser = await context.db.FindUserAsync(callBack.UserId);
            await bot.SendTextMessageAsync(confirmedUser.ChatId, 
                                           options.personalConfig.Messages["YOURAPPCONFIRM"],
                                           replyMarkup: KeyBoards.GetStartKeyboard(options));
            await bot.SendTextMessageAsync(admin.ChatId,
                $"Запись на время {app.AppointmentTime} подтверждена пользователю {confirmedUser.FirstName}");
            return null;
        }
    }
}
