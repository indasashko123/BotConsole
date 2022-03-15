using BotLibary.TelegramBot.Handlers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands
{
    class SendPhoneCommand : ICallBackCommand
    {
        string phone;
        string name;
        DataBase.Models.User currentUser;
        public SendPhoneCommand(string name, string phone, DataBase.Models.User currentUser)
        {
            this.phone = phone;
            this.name = name;
            this.currentUser = currentUser;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            return await bot.SendContactAsync(currentUser.ChatId, phone, name);     
        }
    }
}
