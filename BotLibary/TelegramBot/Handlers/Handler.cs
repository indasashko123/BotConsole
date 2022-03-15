using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using BotLibary.Events;
using BotLibary.TelegramBot.Handlers.CallBackHandlers;

namespace BotLibary.TelegramBot.Handlers
{
    public class Handler
    {
        private Bot bot;
        private BotOnMessageRecervedHandler onMessage;
        private BotOnCallBackHandler onCallBack;

        public event ChangesLog consoleMessage;
        public event AdminMessage adminMessage;

        public Handler(Bot bot, ChangesLog consoleMessage, AdminMessage adminMessage)
        {
            this.bot = bot;
            this.consoleMessage += (string text) => consoleMessage?.Invoke(text);
            this.adminMessage += (EventArgsNotification e) => adminMessage?.Invoke(e);
            onMessage = new BotOnMessageRecervedHandler(this.bot, this.consoleMessage, this.adminMessage);
            onCallBack = new BotOnCallBackHandler(this.bot, this.consoleMessage, this.adminMessage);
        }
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => onMessage.BotOnMessageReceived(botClient, update.Message!),
                UpdateType.EditedMessage => onMessage.BotOnMessageReceived(botClient, update.EditedMessage!),
                UpdateType.CallbackQuery => onCallBack.BotOnCallbackQueryReceived(botClient, update.CallbackQuery!),
                UpdateType.InlineQuery => onCallBack.BotOnInlineQueryReceived(botClient, update.InlineQuery!),
                UpdateType.ChosenInlineResult => onCallBack.BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult!),
                _ => onCallBack.UnknownUpdateHandlerAsync(botClient, update)
            };
            if (handler != null)
            {
                try
                {
                    await handler;

                }
                catch (Exception exception)
                {
                    await bot.errorHandler.HandleErrorAsync(botClient, exception, cancellationToken);
                }
            }
            
        }
    }
}
