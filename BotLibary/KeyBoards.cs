using Options;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotLibary
{
    internal class KeyBoards
    {
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

        internal static IReplyMarkup GetMonthButtons()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
            new List<InlineKeyboardButton>()
            {
                InlineKeyboardButton.WithCallbackData("Mecяц", "data"),InlineKeyboardButton.WithCallbackData("Mecяц", "data")
            },
            new List<InlineKeyboardButton>()
            {
                InlineKeyboardButton.WithCallbackData("Mecяц", "data"),InlineKeyboardButton.WithCallbackData("Mecяц", "data")
            },
            new List<InlineKeyboardButton>()
            {
                InlineKeyboardButton.WithCallbackData("Mecяц", "data"),InlineKeyboardButton.WithCallbackData("Mecяц", "data")
            }
            });                  
        }

        internal static IReplyMarkup GetInstagrammButton()
        {
            throw new NotImplementedException();
        }

        internal static IReplyMarkup GetLinkButtons()
        {
            throw new NotImplementedException();
        }
    }
}