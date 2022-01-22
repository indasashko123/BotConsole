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
        /// Инлайн клавиатура для выбора месяца на который можно записаться.
        /// </summary>
        /// <param name="months">Список месяцев в БД</param>
        /// <param name="user">Пользователь вызвавший клавиатуру</param>
        /// <returns></returns>
        internal static IReplyMarkup GetMonthButtons(List<Month> months, User user)
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            InlineKeyboardButton leftButton = InlineKeyboardButton.WithCallbackData("Не присвоенно значение","404/1");
            InlineKeyboardButton rightButton = InlineKeyboardButton.WithCallbackData("Не присвоенно значение", "404/1");
            foreach (var month in months)
            {
                string data = $"M/{month.MonthId}/{user.UserId}";
                if (month.IsCurrent)
                {
                    leftButton = InlineKeyboardButton.WithCallbackData(month.Name, data);
                }
                else
                {
                    rightButton = InlineKeyboardButton.WithCallbackData(month.Name, data);
                }
            }
            buttons.Add(leftButton);
            buttons.Add(rightButton);
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
        /// <param name="appointments"></param>
        /// <returns></returns>
        internal static IReplyMarkup GetDaysButton(List<Day> days, BotOptions options, User user)
        {
            int count = 0;
            int line = 0;
            List<List<InlineKeyboardButton>> buttons = new List<List<InlineKeyboardButton>>();
            foreach (Day day in days)
            {
                string data;
                string message;
                if (!day.IsHaveAppoint || !day.IsWorkDay)
                {
                    message = $"{day.Date}{options.personalConfig.Messages["DAYOFF"]}";
                    data = "0";
                }
                else
                {
                    data = $"D/{day.DayId}/{user.UserId}";
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
    }
}