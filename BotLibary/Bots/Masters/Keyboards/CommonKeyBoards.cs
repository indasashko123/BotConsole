using DataBase.Models;
using Options;
using Options.MasterBotConfig;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotLibary.Bots.Masters.Keyboards
{
    internal  class CommonKeyboard
    {
       
        /// <summary>
        /// Инлайн клавиатура для выбора месяца.
        /// </summary>
        /// <param name="months">Список месяцев в БД</param>
        /// <param name="code">Код для дальнейшей логики из класса Codes</param>
        /// <param name="user">Пользователь вызвавший клавиатуру</param>
        /// <returns></returns>
        internal protected static IReplyMarkup GetMonthButtons(List<Month> months,string code, User user)
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();            
            foreach (Month month in months)
            {
                string data = $"{code}/M/{month.MonthId}/{user.UserId}";
                buttons.Add(InlineKeyboardButton.WithCallbackData(month.Name, data));
            }
            return new InlineKeyboardMarkup(buttons);                 
        }
        /// <summary>
        /// Клавиатура выбора дня
        /// </summary>
        /// <param name="days">Список дней</param>
        /// <param name="options">Параметры</param>
        /// <param name="code">Спец код вызова</param>
        /// <param name="user">Пользователь</param>
        /// <returns></returns>
        internal protected static IReplyMarkup GetDaysButton(List<Day> days, BotOptions<MasterBotConfig, PersonalMasterBotConfig> options, string code, User user)
        {
            int count = 0;
            int line = 0;
            List<List<InlineKeyboardButton>> buttons = new List<List<InlineKeyboardButton>>();
            foreach (Day day in days)
            {
                string data = $"{code}/D/{day.DayId}/{user.UserId}";
                string message;
                if (!day.IsWorkDay)
                {
                    message = $"{day.Date}{options.personalConfig.Messages["DAYOFF"]}";
                }
                else
                {                  
                    if (day.IsHighPriceDay)
                        message = $"{day.Date}{options.personalConfig.Messages["HIGHPRICEDAY"]}";
                    else
                        message = $"{day.Date}{options.personalConfig.Messages["REGULARDAY"]}";

                }
                var buttn = InlineKeyboardButton.WithCallbackData(message, data);
                if (count % 2 == 0)
                {
                    var list = new List<InlineKeyboardButton>();
                    list.Add(buttn);
                    buttons.Add(list);
                    count++;
                }
                else
                {
                    buttons[line].Add(buttn);
                    line++;
                    count++;
                }
            }
            return new InlineKeyboardMarkup(buttons);
        }
        /// <summary>
        /// Вызвать клавиатуру для выбора времени на запись.
        /// </summary>
        /// <param name="apps">Список записей</param>
        /// <param name="options">Параметры</param>
        /// <param name="code">Код вызова</param>
        /// <param name="userId">Пользователь</param>
        /// <returns></returns>
        internal protected static IReplyMarkup GetAppointmentKeyboard(List<Appointment> apps, BotOptions<MasterBotConfig, PersonalMasterBotConfig> options, string code, int userId)
        {
            int count = 0;
            int line = 0;
            List<List<InlineKeyboardButton>> buttons = new List<List<InlineKeyboardButton>>();
            foreach (Appointment app in apps)
            {
                string message = $"{app.AppointmentTime}{options.personalConfig.Messages["APPEMPTY"]}";
                string data = $"{code}/A/{app.AppointmentId}/{userId}";
                if (!app.IsEmpty)
                {
                    message = $"{app.AppointmentTime}{options.personalConfig.Messages["APPNOTEMPTY"]}"; 
                }           
                var buttn = InlineKeyboardButton.WithCallbackData(message, data);
                if (count % 2 == 0)
                {
                    var list = new List<InlineKeyboardButton>();
                    list.Add(buttn);
                    buttons.Add(list);
                    count++;
                }
                else
                {
                    buttons[line].Add(buttn);
                    line++;
                    count++;
                }
            }
            return new InlineKeyboardMarkup(buttons);
        }
        
      
        
    }
}