using BotLibary.Events;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using BotLibary.TelegramBot.CallBackData;
using BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers
{
    class BotOnCallBackHandler
    {
        public event ChangesLog consoleLog;
        public event AdminMessage adminMessage;
        private readonly Bot bot;
        public BotOnCallBackHandler(Bot bot,ChangesLog consoleLog, AdminMessage adminMessage)
        {
            this.bot = bot;
            this.consoleLog += (string text) => consoleLog?.Invoke(text);
            this.adminMessage += (EventArgsNotification e) => adminMessage?.Invoke(e);
        }
        internal async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            CallBack callBack = new CallBack(callbackQuery.Data);
            DataBase.Models.User admin = await bot.context.db.Reader.FindAdminAsync();
            long chatId = callbackQuery.Message.Chat.Id;
            DataBase.Models.User currentUser = await bot.context.db.Reader.FindUserAsync(chatId);
           
            if (currentUser == null)
            {
                consoleLog?.Invoke($"Пользователь NULL");
                return;
            }
            var action = await new CallBackAction(this.bot, consoleLog, adminMessage)
                .GetAction(currentUser, admin, callBack, botClient, callbackQuery);

        }
        internal async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            Console.WriteLine($"Received inline query from: {inlineQuery.From.Id}");

            InlineQueryResult[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "3",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent(
                    "hello"
                )
            )
        };

            await botClient.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
                                                   results: results,
                                                   isPersonal: true,
                                                   cacheTime: 0);
        }
        internal async Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            consoleLog?.Invoke($"Received inline result: {chosenInlineResult.ResultId}");
            return;
        }
        internal async Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            consoleLog?.Invoke($"Unknown update type: {update.Type}");
            return;
        }
    }
}
