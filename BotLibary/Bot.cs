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
using BotLibary.CallBackData;

namespace BotLibary
{
    
    public class Bot
    {
        public ChangesLog ConsoleMessage;
        public AdminMessage adminMessage;
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
            adminMessage += (async(EventArgsNotification e) => { await bot.SendTextMessageAsync(e.Admin,e.Text, replyMarkup:e?.Keyboard); });
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
                    return;
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
                        await bot.SendTextMessageAsync(currentUser.chatId, "Для записи необходимо выбрать месяц", replyMarkup: KeyBoards.GetMonthButtons(await context.db.GetMonthsAsync(), Codes.UserChoise,currentUser));
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["PRICE"])
                    {
                        await bot.SendPhotoAsync(currentUser.chatId, new InputOnlineFile(personalConfig.Paths["PRICE"]), replyMarkup: KeyBoards.GetStartKeyboard(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["FEEDBACK"])
                    {
                        await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["FEEDBACK"]);
                        await bot.SendTextMessageAsync(currentUser.chatId, "👇", replyMarkup: KeyBoards.GetInstagrammButton(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["MYWORKS"])
                    {
                        foreach (var photoURL in personalConfig.Partfolio)
                        {
                            await bot.SendPhotoAsync(currentUser.chatId, new InputOnlineFile(photoURL));
                        }
                        return;
                    }                  
                    if (e.Message.Text == personalConfig.Buttons["ABOUT"])
                    {
                        await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["ABOUT"], replyMarkup: KeyBoards.GetStartKeyboard(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["LOCATION"])
                    {
                        await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["LOCATION"]);
                        await bot.SendLocationAsync(currentUser.chatId, personalConfig.Latitude, personalConfig.Longitude, replyMarkup: KeyBoards.GetStartKeyboard(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.Buttons["LINK"])
                    {
                        await bot.SendTextMessageAsync(currentUser.chatId, "👇", replyMarkup: KeyBoards.GetLinkButtons(options));
                        return;
                    }
                    if (e.Message.Text == "/reg"+botConfig.password && admin == null)
                    {
                        dateFunction.CreateMonths(currentUser.UserId);
                        ConsoleMessage?.Invoke("Началась регистрация\n");
                        await context.db.AddMonthAsync(currentUser.UserId, dateFunction.CurrentMonth);
                        ConsoleMessage?.Invoke($"месяц {dateFunction.CurrentMonth.Name}, дней {dateFunction.CurrentMonth.DayCount}\n");
                        await context.db.CreateDaysAsync(currentUser.UserId, dateFunction.CurrentDay, dateFunction.CurrentMonth);
                        await context.db.AddMonthAsync(currentUser.UserId, dateFunction.NextMonth);
                        await context.db.CreateDaysAsync(currentUser.UserId, dateFunction.NextMonth);
                        admin = await context.db.MakeAdminAsync(currentUser);
                        ConsoleMessage?.Invoke($"месяц {dateFunction.NextMonth.Name}, дней {dateFunction.NextMonth.DayCount}");
                        await adminMessage?.Invoke(new EventArgsNotification(admin.chatId, "Зарегестрированно!"));
                    }
                    //else
                    await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["UNKNOWN"]);
                    return;
                }
                if (currentUser.isAdmin)
                {
                    if (e.Message.Text == "/start")
                    {
                        await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["ADMINGREETING"], replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                        return;
                    }
                    if (e.Message.Text == personalConfig.AdminButtons["ADDAPP"])
                    {
                        List<Month> month = await context.db.GetMonthsAsync();
                        await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["ADDAPP_MONTHLVL"], replyMarkup: KeyBoards.GetMonthButtons(month,Codes.AdminAdd, currentUser));
                        return;
                    }                   
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
            DataBase.Models.User admin = await context.db.FindAdminAsync();
            long chatId = e.CallbackQuery.Message.Chat.Id;
            DataBase.Models.User currentUser = await context.db.FindUserAsync(chatId);
            CallBackData.CallBackData data =await CallBackData.CallBackData.GetDataAsync(e.CallbackQuery.Data);
            if (currentUser == null)
            {
                ConsoleMessage?.Invoke($"Пользователь NULL при нажатии кнопки {e.CallbackQuery.Data}");
                return;
            }
            if (data.Error == "0" && !data.EmptyButton)
            {
                if (data.UserRole == UserRole.User)
                {
                    // месяц
                    if (data.Stage == Stage.Month && data.Action == CallBackData.Action.Choise)
                    {
                        List<Day> days = await context.db.FindDaysAsync(data.EntityId, data.UserId);
                        await bot.SendTextMessageAsync(chatId, personalConfig.Messages["CHOSEDAY"], replyMarkup: KeyBoards.GetDaysButton(days, options, Codes.UserChoise, currentUser));
                        return;
                    }
                    if (data.Stage == Stage.Day && data.Action == CallBackData.Action.Choise)
                    {
                        List<Appointment> apps = await context.db.FindAppointmentsAsync(data.EntityId, data.UserId);
                        await bot.SendTextMessageAsync(chatId, personalConfig.Messages["CHOSEAPP"], replyMarkup: KeyBoards.GetAppointmentKeyboard(apps, options, Codes.UserChoise, currentUser.UserId));
                        return;
                    }
                    if (data.Stage == Stage.Appointment && data.Action == CallBackData.Action.Choise)
                    {
                        //TODO: app
                        Appointment app = await context.db.FindAppointmentAsync(data.EntityId, data.UserId);
                        if (!app.IsEmpty)
                        {
                            await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["SORRYTAKEN"], replyMarkup: KeyBoards.GetStartKeyboard(options));
                            return;
                        }
                        app.IsConfirm = false;
                        app.IsEmpty = false;
                        app.User = currentUser.UserId;
                        string message = $"{personalConfig.Messages["NEWAPP"]}\n " +
                            $"{currentUser.firstName} {currentUser.lastName} {currentUser.username} на время - {app.AppointmentTime}";
                        await bot.SendTextMessageAsync(currentUser.chatId, personalConfig.Messages["WAITCINFIRM"], replyMarkup: KeyBoards.GetStartKeyboard(options));
                        await bot.SendTextMessageAsync(admin.chatId, message, replyMarkup: KeyBoards.GetConfirmKeyboard(app,options, Codes.AdminConfirm, currentUser));
                        return;
                    }
                }
                // Admin выбрал
                if (callBackData[0] == "A")
                {

                }
            }
            // User выбрал 
            

            if (data.Error == "404")
            {
                ConsoleMessage?.Invoke($"Ошибка 404 {e.CallbackQuery.Data}");
                await bot.SendTextMessageAsync(chatId, "Какая-то ошибка(( попробуйте еще раз", replyMarkup: KeyBoards.GetStartKeyboard(options));
            }
        }

        
    }
}
