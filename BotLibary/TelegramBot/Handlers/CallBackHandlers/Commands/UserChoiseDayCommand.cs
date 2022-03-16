using BotLibary.TelegramBot.CallBackData;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using DataBase.Models;
using Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands
{
    class UserChoiseDayCommand : ICallBackCommand
    {
        DataBaseConnector context;
        CallBack callBack;
        DataBase.Models.User currentUser;
        BotOptions options;
        string MessageText;
        string Code;
        public UserChoiseDayCommand(DataBase.Models.User currentUser, 
                                    DataBaseConnector context, 
                                    BotOptions options,
                                    CallBack callBack,
                                    string MessageText,
                                    string Code)
        {
            this.currentUser = currentUser;
            this.context = context;
            this.options = options;
            this.callBack = callBack;
            this.MessageText = MessageText;
            this.Code = Code;
        }

        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            Day day = await context.db.Reader.FindDayAsync(callBack.EntityId);
            if (!day.IsWorkDay && !currentUser.IsAdmin)
            {
                return await bot.SendTextMessageAsync(currentUser.ChatId, 
                                               options.personalConfig.Messages["ISWEEKENDFORUSER"],
                                               replyMarkup: KeyBoards.GetStartKeyboard(options));
                
            }
            List<Appointment> apps = await context.db.Reader.FindAppointmentsAsync(callBack.EntityId);
            return await bot.SendTextMessageAsync(currentUser.ChatId, MessageText, 
                                           replyMarkup: KeyBoards.GetAppointmentKeyboard(apps, options, 
                                                                                         Code,currentUser.UserId));
        }
    }
}
