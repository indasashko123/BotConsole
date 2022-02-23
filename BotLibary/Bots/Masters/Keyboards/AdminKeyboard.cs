using Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotLibary.Bots.Masters.Keyboards
{
    internal class AdminKeyboard
    {
        /// <summary>
        /// Клавиатура для админа.
        /// </summary>
        /// <param name="options">Параметры бота</param>
        /// <returns></returns>
        internal protected static IReplyMarkup GetKeyboardAdmin(BotOptions options)
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = options.personalConfig.AdminButtons["ADDAPP"]  }, new KeyboardButton { Text = options.personalConfig.AdminButtons["DELAPP"] } ,new KeyboardButton { Text = options.personalConfig.AdminButtons["ALLUSERS"] } },
                    new List<KeyboardButton> { new KeyboardButton { Text = options.personalConfig.AdminButtons["MAKEWEEKEND"] }, new KeyboardButton { Text = options.personalConfig.AdminButtons["LOOKCONFIRM"] } ,new KeyboardButton { Text = options.personalConfig.AdminButtons["LOOKNOTCONFIRM"] } },
                    new List<KeyboardButton> {new KeyboardButton { Text = options.personalConfig.AdminButtons["MAILING"] }//, new KeyboardButton {Text = options.personalConfig.AdminButtons["OPTIONS"] } 
                    }
                },
                ResizeKeyboard = true
            };
        }
        /// <summary>
        /// Кнопка отмены
        /// </summary>
        /// <param name="appointmentId">Сущность</param>
        /// <param name="options">Параметры бота</param>
        /// <param name="code">Код операции</param>
        /// <param name="userId">Пользователь</param>
        /// <returns></returns>
        internal protected static IReplyMarkup GetCanccelButton(int appointmentId, BotOptions options, string code, int userId)
        {
            return new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Отменить", $"{code}/Y/{appointmentId}/{ userId}"));
        }
        /// <summary>
        /// Клавиатура подтверждения
        /// </summary>
        /// <param name="EntityId">Id Сущности</param>
        /// <param name="options">Параметры бота</param>
        /// <param name="code">Код операции</param>
        /// <param name="userId">Id пользователя</param>
        /// <returns></returns>
        internal protected static IReplyMarkup GetConfirmKeyboard(int EntityId, BotOptions options, string code, int userId)
        {
            return new InlineKeyboardMarkup(new List<InlineKeyboardButton>()
            {
                InlineKeyboardButton.WithCallbackData($"{options.personalConfig.AdminButtons["CONFIRM"]}",$"{code}/Y/{EntityId}/{userId}"),
                InlineKeyboardButton.WithCallbackData($"{options.personalConfig.AdminButtons["NOTCONFIRM"]}",$"{code}/N/{EntityId}/{userId}")
            });

        }
        internal protected static IReplyMarkup GetOptionsKeyboard(BotOptions options)
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>()
            {
                new List<KeyboardButton> { new KeyboardButton() {Text = options.personalConfig.Options["ADDEXAMPLE"] }, new KeyboardButton() { Text = options.personalConfig.Options["DELETEEXAMPLE"] } },
                new List<KeyboardButton> { new KeyboardButton() {Text = options.personalConfig.Options["GREETING"] }, new KeyboardButton() { Text = options.personalConfig.Options["PRICE"] } },
            },
                ResizeKeyboard = true
            };
        }
    }
}
