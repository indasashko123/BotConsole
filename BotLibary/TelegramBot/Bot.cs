using System;
using Telegram.Bot;
using DataBase.Database;
using DataBase.Database.Context.MySQL;
using Options;
using BotLibary.Events;
using BotLibary.Interfaces;
using BotLibary.TelegramBot.Handlers;
using System.Threading;
using Telegram.Bot.Extensions.Polling;

namespace BotLibary.TelegramBot
{
    
    public class Bot : AbstractBot, IBot
    {
        public event ChangesLog ConsoleMessage;
        public event AdminMessage adminMessage;
        CancellationTokenSource cts = new CancellationTokenSource();
    public Bot(BotOptions options, ChangesLog consoleMessage)
        {
            this.options = options;
            botConfig = options.botConfig;
            personalConfig = options.personalConfig;
            BotName = new BotName(botConfig.Name, botConfig.CustomerName, botConfig.Direction);
            dateFunction = new DateFunction();
            context = new DataBaseConnector(new SQLContext(botConfig.dataBaseName));
            bot = new TelegramBotClient(botConfig.token);
            bot.Timeout = new TimeSpan(0, 10, 0);
            imageManager = new ImageManager();
            this.ConsoleMessage += (string text) => consoleMessage?.Invoke(text);
            adminMessage += async (EventArgsNotification e) => await bot.SendTextMessageAsync(e.Admin, e.Text, replyMarkup: e?.Keyboard);
            errorHandler = new ErrorHandler(ConsoleMessage);
            handler = new Handler(this, ConsoleMessage, adminMessage);

        }
        #region OLD

        #endregion
        /// <summary>
        ///  Запуск бота
        /// </summary>
        public void BotStart()
        {
            ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
            bot.StartReceiving(handler.HandleUpdateAsync,
                               errorHandler.HandleErrorAsync,
                               receiverOptions,
                               cts.Token);
            ConsoleMessage?.Invoke($"Бот {BotName.Name} запущен");
        }
        /// <summary>
        /// Остановка бота
        /// </summary>
        public void BotStop()
        {
            cts.Cancel();
            ConsoleMessage?.Invoke($"Бот {BotName.Name} остановлен");
        }     
        public void GetNewConfig(BotOptions options)
        {
            this.options = options;
            botConfig = options.botConfig;
            personalConfig = options.personalConfig;
        }
    }
}
