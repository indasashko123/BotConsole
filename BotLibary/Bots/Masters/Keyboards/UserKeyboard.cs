using Options;
using Options.MasterBotConfig;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotLibary.Bots.Masters.Keyboards
{
    internal class UserKeyboard
    {
        /// <summary>
        /// Клавиатура для пользователя.
        /// </summary>
        /// <param name="options">Параметры бота</param>
        /// <returns></returns>
        internal protected static IReplyMarkup GetStartKeyboard(BotOptions<MasterBotConfig, PersonalMasterBotConfig> options)
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = options.personalConfig.Buttons["APPOINTMENT"] }, new KeyboardButton { Text = options.personalConfig.Buttons["PRICE"] } ,new KeyboardButton { Text = options.personalConfig.Buttons["ABOUT"] } },
                    new List<KeyboardButton> { new KeyboardButton { Text = options.personalConfig.Buttons["MYWORKS"] }, new KeyboardButton { Text = options.personalConfig.Buttons["FEEDBACK"] } ,new KeyboardButton { Text = options.personalConfig.Buttons["LOCATION"] } },
                    new List<KeyboardButton> {new KeyboardButton { Text = options.personalConfig.Buttons["LINK"] } }
                },
                ResizeKeyboard = true
            };
        }
        /// <summary>
        /// Возвращает кнопку которая ссылка на инстаграмм.
        /// </summary>
        /// <param name="options">Параметры бота.</param>
        /// <returns></returns>
        internal protected static IReplyMarkup GetInstagrammButton(BotOptions<MasterBotConfig, PersonalMasterBotConfig> options)
        {
            return new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(options.personalConfig.Messages["INSTAGRAM"], options.personalConfig.MediaLink["INSTAGRAM"]));
        }
        /// <summary>
        /// Вызывает клавиатуру с ссылками на медиапространства.
        /// </summary>
        /// <param name="options">Конфигурация бота</param>
        /// <returns></returns>
        internal protected static IReplyMarkup GetLinkButtons(BotOptions<MasterBotConfig, PersonalMasterBotConfig> options)
        {
            int count = 0;
            int line = 0;
            List<List<InlineKeyboardButton>> buttons = new List<List<InlineKeyboardButton>>();
            foreach (var link in options.personalConfig.MediaLink)
            {
                InlineKeyboardButton buttn;
                if (link.Key == "PHONE")
                {
                    buttn = InlineKeyboardButton.WithCallbackData("Телефон", $"Phone/{link.Value}");
                }
                else
                {
                    buttn = InlineKeyboardButton.WithUrl(link.Key, link.Value);
                }
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
