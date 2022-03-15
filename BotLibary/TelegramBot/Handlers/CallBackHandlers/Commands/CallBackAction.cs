using BotLibary.Events;
using BotLibary.TelegramBot.CallBackData;
using BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands.AdminCommand;
using BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands.ErrorCommand;
using BotLibary.TelegramBot.ReplyMarkups;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands
{
    internal class CallBackAction
    {
        private Bot bot;
        public event ChangesLog consoleMessage;
        public event AdminMessage adminMessage;
        public CallBackAction(Bot bot, ChangesLog consoleMessage, AdminMessage adminMessage)
        {
            this.adminMessage += (EventArgsNotification e) => adminMessage?.Invoke(e);
            this.consoleMessage += (string text) => consoleMessage?.Invoke(text);
            this.bot = bot;
        }
        public async Task<Message> GetAction (DataBase.Models.User currentUser, DataBase.Models.User admin, 
                                              CallBack callBack, ITelegramBotClient botClient, 
                                              CallbackQuery callbackQuery)
        {
            if (callbackQuery.Data.ToUpper().Contains("PHONE"))
            {
                string phone =callbackQuery.Data.Split('/')[1];
                return await new SendPhoneCommand(bot.BotName.CustomerName, phone, currentUser).ReturnCommand(botClient, callbackQuery);
            }
            if (callBack.Error == "0" && !callBack.EmptyButton)
            {
                if (callBack.UserRole == UserRole.User)
                {
                    if (callBack.Stage == Stage.Month && callBack.Action == CallBackData.Action.Choise)
                    {
                        return await new UserChoiseMonthCommand(bot.context, currentUser,
                                                                callBack, bot.options, 
                                                                bot.options.personalConfig.Messages["CHOSEDAY"], 
                                                                Codes.UserChoise).
                        ReturnCommand(botClient, callbackQuery);                       
                    }
                    if (callBack.Stage == Stage.Day && callBack.Action == CallBackData.Action.Choise)
                    {
                        return await new UserChoiseDayCommand(currentUser, bot.context,
                                                              bot.options, callBack,
                                                              bot.options.personalConfig.Messages["CHOSEAPP"],
                                                              Codes.UserChoise).
                          ReturnCommand(botClient, callbackQuery);
                    }
                    if (callBack.Stage == Stage.Appointment && callBack.Action == CallBackData.Action.Choise)
                    {
                        return await new UserChoiseAppointmentCommand(admin,currentUser,
                                                                     bot.context,bot.options, 
                                                                     callBack).
                        ReturnCommand(botClient, callbackQuery);                        
                    }
                }
                if (callBack.UserRole == UserRole.Admin)
                {
                    if (callBack.Action == CallBackData.Action.Confirm)
                    {
                        if (callBack.Stage == Stage.Yes)
                        {
                            return await new AdminConfirmCommand(admin,
                                                                 bot.context,
                                                                 bot.options,
                                                                 callBack).
                            ReturnCommand(botClient, callbackQuery);
                        }
                        if (callBack.Stage == Stage.No)
                        {
                            return await new AdminNotConfirmCommand(admin,bot.context,bot.options,callBack).  
                                ReturnCommand(botClient, callbackQuery);
                        }
                    }
                    if (callBack.Action == CallBackData.Action.Add)
                    {
                        if (callBack.Stage == CallBackData.Stage.Month)
                        {
                            return await new UserChoiseMonthCommand(bot.context, admin,
                                                                     callBack, bot.options,
                                                                    "Выберите день, в который нужно добавить запись",
                                                                    Codes.AdminAdd).
                            ReturnCommand(botClient, callbackQuery);
                        }
                        if (callBack.Stage == CallBackData.Stage.Day)
                        {
                            return await new AdminChoiseDayAddAppCommand(admin, bot.context,
                                                                         bot.options,callBack).
                            ReturnCommand(botClient, callbackQuery);                 
                        }
                    }
                    if (callBack.Action == CallBackData.Action.Delete)
                    {
                        if (callBack.Stage == Stage.Month)
                        {
                            return await new UserChoiseMonthCommand(
                                                                    bot.context,
                                                                    admin,
                                                                    callBack,
                                                                    bot.options,
                                                                    "Выберите день, в котором нужно удалить запись",
                                                                    Codes.AdminDelete
                                                                    ).
                            ReturnCommand(botClient, callbackQuery);
                        }
                        if (callBack.Stage == Stage.Day)
                        {
                            return await new UserChoiseDayCommand(admin,bot.context,
                                                                  bot.options, callBack,
                                                                  $"Выбирите запись для удаления", 
                                                                  Codes.AdminDelete).
                                ReturnCommand(botClient, callbackQuery);                    
                        }
                        if (callBack.Stage == Stage.Appointment)
                        {
                            return await new AdminChoiseAppDeleteCommand(admin, bot.context,
                                                                         bot.options, callBack).
                                ReturnCommand(botClient, callbackQuery);                   
                        }
                        if (callBack.Stage == Stage.Yes)
                        {
                            return await new AdminConfirmDeleteCommand(admin, bot.context,
                                                                         bot.options, callBack).
                                ReturnCommand(botClient, callbackQuery);
                        }
                        if (callBack.Stage == Stage.No)
                        { 
                           return await botClient.SendTextMessageAsync(admin.ChatId, "Запись не будет удалена", replyMarkup: KeyBoards.GetKeyboardAdmin(bot.options));
                        }
                    }
                    if (callBack.Action == CallBackData.Action.WeekEnd)
                    {
                        if (callBack.Stage == Stage.Month)
                        {
                            return await new UserChoiseMonthCommand(bot.context,admin, 
                                                                    callBack,bot.options,
                                                                    "Выбирите день для выходного",Codes.AdminWeekEnd
                                                                    ).
                                ReturnCommand(botClient, callbackQuery);
                        }
                        if (callBack.Stage == Stage.Day)
                        {
                            return await new AdminChoiseDayWeekEndCommand(admin, bot.context,
                                                                          bot.options, callBack).
                                ReturnCommand(botClient, callbackQuery);
                        }
                        if (callBack.Stage == Stage.Yes)
                        {
                            return await new AdminConfirmWeekEndCommand(admin, bot.context,
                                                                        bot.options, callBack).
                                ReturnCommand(botClient, callbackQuery);                       
                        }
                        if (callBack.Stage == Stage.No)
                        {
                            return await botClient.SendTextMessageAsync(admin.ChatId, "Отменено", replyMarkup: KeyBoards.GetKeyboardAdmin(bot.options));                         
                        }
                    }
                    if (callBack.Action == CallBackData.Action.CancelApp)
                    {
                        return await new AdminCancelAppCommand(admin, bot.context,
                                                               bot.options, callBack).
                            ReturnCommand(botClient, callbackQuery);
                    }
                    if (callBack.Action == CallBackData.Action.Mailing)
                    {
                        return await new AdminMailingCommand(admin, bot.context,
                                                             bot.options, bot.lastMessage, callBack)
                            .ReturnCommand(botClient, callbackQuery); 
                    }
                }
            }
            if (callBack.Error == "404")
            {
                consoleMessage?.Invoke($"Ошибка 404 {callBack}");
                return await new ErrorNotFoundCommand(currentUser, bot.options).
                                                      ReturnCommand(botClient, callbackQuery);                            
            }
            if (callBack.Error == "405")
            {
                consoleMessage?.Invoke($"Ошибка 405 {callBack}");
                return await new ErrorParsingCommand(currentUser, bot.options).
                                                      ReturnCommand(botClient, callbackQuery);
           }
            return await botClient.SendTextMessageAsync(currentUser.ChatId, "Неизвестная команда");
        }
    }
}
