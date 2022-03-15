using BotLibary.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace BotLibary.TelegramBot.Handlers
{
    public class ErrorHandler
    {
        public event ChangesLog consoleMessage;
        public ErrorHandler(ChangesLog consoleMessage)
        {           
             this.consoleMessage += (string text) => consoleMessage?.Invoke(text); 
        }
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            consoleMessage?.Invoke($"{ErrorMessage}");
            return Task.CompletedTask;
        }
    }
}
