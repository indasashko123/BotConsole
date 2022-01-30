using DataBase.Models;
using Options;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotLibary
{
    internal class KeyBoards
    {
        /// <summary>
        /// Клавиатура для админа.
        /// </summary>
        /// <param name="options">Параметры бота</param>
        /// <returns></returns>
        internal static IReplyMarkup GetKeyboardAdmin(BotOptions options)
        {
            // TODO: не закончено
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = options.personalConfig.AdminButtons["ADDAPP"] }, new KeyboardButton { Text = options.personalConfig.AdminButtons["DELAPP"] } ,new KeyboardButton { Text = options.personalConfig.Buttons["ALLUSERS"] } },
                    new List<KeyboardButton> { new KeyboardButton { Text = options.personalConfig.Buttons["MAKEWEEKEND"] }, new KeyboardButton { Text = options.personalConfig.Buttons["FEEDBACK"] } ,new KeyboardButton { Text = options.personalConfig.Buttons["LOCATION"] } },
                    new List<KeyboardButton> {new KeyboardButton { Text = "LINK" } }
                },
                ResizeKeyboard = true
            };
        }

        /// <summary>
        /// Клавиатура для пользователя.
        /// </summary>
        /// <param name="options">Параметры бота</param>
        /// <returns></returns>
        internal static IReplyMarkup GetStartKeyboard(BotOptions options)
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = options.personalConfig.Buttons["APPOINTMENT"] }, new KeyboardButton { Text = options.personalConfig.Buttons["PRICE"] } ,new KeyboardButton { Text = options.personalConfig.Buttons["ABOUT"] } },
                    new List<KeyboardButton> { new KeyboardButton { Text = options.personalConfig.Buttons["MYWORKS"] }, new KeyboardButton { Text = options.personalConfig.Buttons["FEEDBACK"] } ,new KeyboardButton { Text = options.personalConfig.Buttons["LOCATION"] } },
                    new List<KeyboardButton> {new KeyboardButton { Text = "LINK" } }
                },
                ResizeKeyboard = true
            };
        }

        /// <summary>
        /// Инлайн клавиатура для выбора месяца.
        /// </summary>
        /// <param name="months">Список месяцев в БД</param>
        /// <param name="code">Код для дальнейшей логики из класса Codes</param>
        /// <param name="user">Пользователь вызвавший клавиатуру</param>
        /// <returns></returns>
        internal static IReplyMarkup GetMonthButtons(List<Month> months,string code, User user)
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
        /// Возвращает кнопку которая ссылка на инстаграмм.
        /// </summary>
        /// <param name="options">Параметры бота.</param>
        /// <returns></returns>
        internal static IReplyMarkup GetInstagrammButton(BotOptions options)
        {
            return new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(options.personalConfig.Messages["INSTAGRAM"], options.personalConfig.MediaLink["INSTAGRAM"]));
        }

        /// <summary>
        /// Вызывает клавиатуру с ссылками на медиапространства.
        /// </summary>
        /// <param name="options">Конфигурация бота</param>
        /// <returns></returns>
        internal static IReplyMarkup GetLinkButtons(BotOptions options)
        {
            int count = 0;
            int line = 0;
            List<List<InlineKeyboardButton>> buttons = new List<List<InlineKeyboardButton>>();
            foreach (var link in options.personalConfig.MediaLink)
            {
                var buttn = InlineKeyboardButton.WithUrl(options.personalConfig.Messages[link.Key], link.Value);
                if (count%2 == 0)
                {                   
                    var list = new List<InlineKeyboardButton>();
                    list.Add(buttn);
                    buttons.Add(list);
                    count++;
                }
                if (count%2 == 1)
                {
                    buttons[line].Add(buttn);
                    line++;
                    count++;
                }
            }
            return new InlineKeyboardMarkup(buttons);
        }

        /// <summary>
        /// Вызывает список дней 
        /// </summary>
        /// <param name="days">Список дней</param>
        /// <param name="options">Параметры</param>
        /// <param name="code">Спец код вызова</param>
        /// <param name="user">Пользователь</param>
        /// <returns></returns>
        internal static IReplyMarkup GetDaysButton(List<Day> days, BotOptions options, string code, User user)
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
                if (count % 2 == 1)
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
        internal static IReplyMarkup GetAppointmentKeyboard(List<Appointment> apps,BotOptions options, string code, int userId)
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
                if (count % 2 == 1)
                {
                    buttons[line].Add(buttn);
                    line++;
                    count++;
                }
            }
            return new InlineKeyboardMarkup(buttons);
        }
        /// <summary>
        /// Клавиатура подтверждения
        /// </summary>
        /// <param name="EntityId">Id Сущности</param>
        /// <param name="options">Параметры бота</param>
        /// <param name="code">Код операции</param>
        /// <param name="userId">Id пользователя</param>
        /// <returns></returns>
        internal static IReplyMarkup GetConfirmKeyboard(int EntityId, BotOptions options, string code, int userId)
        {
            return new InlineKeyboardMarkup(new List<InlineKeyboardButton>()
            {
                InlineKeyboardButton.WithCallbackData($"{options.personalConfig.AdminButtons["CONFIRM"]}",$"{code}/Y/{EntityId}/{userId}"),
                InlineKeyboardButton.WithCallbackData($"{options.personalConfig.AdminButtons["NOTCONFIRM"]}",$"{code}/N/{EntityId}/{userId}")
            }) ;
            
        }
        /// <summary>
        /// Кнопка отмены
        /// </summary>
        /// <param name="appointmentId">Сущность</param>
        /// <param name="options">Параметры бота</param>
        /// <param name="code">Код операции</param>
        /// <param name="userId">Пользователь</param>
        /// <returns></returns>
        internal static IReplyMarkup GetCanccelButton(int appointmentId, BotOptions options, string code, int userId)
        {
            return new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Отменить", $"{code}/Y/{appointmentId}/{ userId}"));
        }
    }
}