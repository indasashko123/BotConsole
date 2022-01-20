using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Requests;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using DataBase.Database;
using DataBase.Database.Interface;
using System.Threading;
using Options;
using DataBase.Models;
using BotLibary.Events;
using Telegram.Bot.Types.InputFiles;

namespace BotLibary
{
    
    public class Bot
    {
        public ChangesLog ConsoleMessage;
        public readonly TelegramBotClient bot;
        public readonly Telegram.Bot.Types.User me;
        private readonly BotOptions options;
        private readonly BotConfig botConfig;
        private readonly PersonalConfig personalConfig;
        private readonly string token;


        private readonly Message lastMessage;
        private DateFunction dateFunction;
        private readonly DataBaseConnector context;

        public Bot(BotOptions options)
        {
            this.options = options;
            botConfig = options.botConfig;
            personalConfig = options.personalConfig;
            this.token = botConfig.token;
            this.bot = new TelegramBotClient(token);
            me = bot.GetMeAsync().Result;
            bot.OnMessage += onMessage;
            bot.OnCallbackQuery += OnQuery;
            dateFunction = new DateFunction();
            context = new DataBaseConnector(DataBaseType.MySQL);
        }
        /// <summary>
        /// Обработка входящей команды или кнопки клавиатуры.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void onMessage(object sender, MessageEventArgs e)
        {
            if (e.Message != null && e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text && !string.IsNullOrEmpty(e.Message.Text))
            {
                DataBase.Models.User admin = await context.db.FindAdminAsync();
                DataBase.Models.User currentUser =await context.db.FindUserAsync(e.Message.Chat.Id);
                ConsoleMessage?.Invoke(currentUser == null ? $"\nПользователь с chatId {e.Message.Chat.Id} не найден\n" : $"\nПользователь с chatId {e.Message.Chat.Id} уже заходил\n");
                if (currentUser == null)
                {
                    ConsoleMessage?.Invoke($"Создаем нового пользователся с ChatID {e.Message.Chat.Id}\n");
                    currentUser = await context.db.CreateNewUserAsync(e.Message.From.Username, e.Message.From.FirstName, e.Message.From.LastName, e.Message.Chat.Id);
                }
                if (!currentUser.isAdmin)
                {
                    if (e.Message.Text == "/start")
                    {
                        await bot.SendPhotoAsync(currentUser.chatId, new InputOnlineFile(personalConfig.Paths["GREETING"]));
                        await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["USERGREETING"], replyMarkup: KeyBoards.GetStartKeyboard(options));
                        return;
                    }
                    //TODO: apps
                    if (e.Message.Text == personalConfig.Buttons["APPOINTMENT"])
                    {
                        await bot.SendTextMessageAsync(currentUser.chatId, "Для записи необходимо выбрать месяц", replyMarkup: KeyBoards.GetMonthButtons());
                    }
                    if (e.Message.Text == personalConfig.Buttons["PRICE"])
                    {
                        await bot.SendPhotoAsync(currentUser.chatId, new InputOnlineFile(personalConfig.Paths["PRICE"]), replyMarkup: KeyBoards.GetStartKeyboard(options));
                    }
                    if (e.Message.Text == personalConfig.Buttons["FEEDBACK"])
                    {
                        await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["FEEDBACK"]);
                        await bot.SendTextMessageAsync(currentUser.chatId, "👇", replyMarkup: KeyBoards.GetInstagrammButton());
                    }
                    if (e.Message.Text == personalConfig.Buttons["MYWORKS"])
                    {
                        foreach (var photoURL in personalConfig.Partfolio)
                        {
                            await bot.SendPhotoAsync(currentUser.chatId, new InputOnlineFile(photoURL));
                        }
                    }                  
                    if (e.Message.Text == personalConfig.Buttons["ABOUT"])
                    {
                        await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["ABOUT"], replyMarkup: KeyBoards.GetStartKeyboard(options));
                    }
                    if (e.Message.Text == personalConfig.Buttons["LOCATION"])
                    {
                        await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["LOCATION"]);
                        await bot.SendLocationAsync(currentUser.chatId, personalConfig.Latitude, personalConfig.Longitude, replyMarkup: KeyBoards.GetStartKeyboard(options));
                    }
                    if (e.Message.Text == personalConfig.Buttons["LINK"])
                    {
                        await bot.SendTextMessageAsync(currentUser.chatId, "👇", replyMarkup: KeyBoards.GetLinkButtons());
                    }

                    //else
                    await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["UNKNOWN"]);
                }              
                if (currentUser.isAdmin)
                {
                    if (e.Message.Text == "/start")
                    await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["ADMINGREETING"], replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                    return;
                    //TODO: admin commands.
                }
            }
        }
        /// <summary>
        /// Обработка кнопки на экране.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnQuery(object sender, CallbackQueryEventArgs e)
        {
            throw new NotImplementedException();
        }

        
    }
}
