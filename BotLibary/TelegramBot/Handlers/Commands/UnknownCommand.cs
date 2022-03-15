using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.Commands
{
    internal class UnknownCommand : ICommand
    {
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            return await bot.SendTextMessageAsync(message.Chat.Id, $"Неизвестная команда");
        }
    }
}
