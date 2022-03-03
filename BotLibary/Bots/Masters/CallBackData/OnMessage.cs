
using BotLibary.Bots.Events;
using BotLibary.Bots.Masters.Keyboards;
using DataBase.Masters.Database;
using DataBase.Models;
using Options;
using Options.MasterBotConfig;
using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;

namespace BotLibary.Bots.Masters.CallBackData
{
    internal class OnMessage
    {
        private readonly DataBaseMastersConnector context;
        private BotOptions<MasterBotConfig, PersonalMasterBotConfig> options;
        MasterBotConfig botConfig;
        PersonalMasterBotConfig personalConfig;
        TelegramBotClient bot;
        AdminMessage AdminLog;
        ChangesLog Log;
        BotName BotName;
        DateFunction dateFunction;
        public OnMessage(DataBaseMastersConnector context, BotOptions<MasterBotConfig, PersonalMasterBotConfig> options, TelegramBotClient bot, AdminMessage adminLog, ChangesLog log, DateFunction dateFunction)
        {
            this.dateFunction = dateFunction;
            AdminLog = adminLog;
            Log = log;
            this.bot = bot;
            this.options = options;
            this.context = context;
            botConfig = options.botConfig;
            personalConfig = options.personalConfig;
            BotName = new BotName(botConfig.Name, botConfig.CustomerName, botConfig.Direction);
        }
        internal virtual async void UserOnMessage(object sender, MessageEventArgs e)
        {
            User admin = await context.Finder.FindAdminAsync();
            User currentUser = await context.Finder.FindUserAsync(e.Message.Chat.Id);
            if (!currentUser.IsAdmin)
            {
                if (e.Message.Text == "/start" || e.Message.Text == personalConfig.Buttons["ABOUT"])
                {
                    try
                    {
                        await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(System.IO.File.OpenRead(personalConfig.Paths["GREETING"])), caption: personalConfig.Messages["USERGREETING"], replyMarkup: UserKeyboard.GetStartKeyboard(options));
                    }
                    catch (Exception ex)
                    {
                        Log?.Invoke($"\n\n !!!Возникла ошибка {BotName.Name}  " + ex.Message + "\n\n");
                    }
                    return;
                }
                if (e.Message.Text == personalConfig.Buttons["APPOINTMENT"])
                {
                    if (admin == null)
                    {
                        await bot.SendTextMessageAsync(currentUser.ChatId, "Бот пока не активирован");
                        return;
                    }
                    await bot.SendTextMessageAsync(currentUser.ChatId, "Для записи необходимо выбрать месяц", replyMarkup: CommonKeyboard.GetMonthButtons(await context.Finder.GetMonthsAsync(), Codes.UserChoise, currentUser));
                    return;
                }
                if (e.Message.Text == personalConfig.Buttons["PRICE"])
                {
                    try
                    {
                        await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(System.IO.File.OpenRead(personalConfig.Paths["PRICE"])), replyMarkup: UserKeyboard.GetStartKeyboard(options));
                    }
                    catch (Exception ex)
                    {
                        Log?.Invoke($"\n\n !!!Возникла ошибка {BotName.Name}  " + ex.Message + "\n\n");
                    }
                    return;
                }
                if (e.Message.Text == personalConfig.Buttons["FEEDBACK"])
                {
                    await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["FEEDBACK"]);
                    await bot.SendTextMessageAsync(currentUser.ChatId, "👇", replyMarkup: UserKeyboard.GetInstagrammButton(options));
                    return;
                }
                if (e.Message.Text == personalConfig.Buttons["MYWORKS"])
                {
                    foreach (var photoURL in personalConfig.Partfolio)
                    {
                        try
                        {
                            await bot.SendPhotoAsync(currentUser.ChatId, new InputOnlineFile(System.IO.File.OpenRead(photoURL)));
                        }
                        catch (Exception ex)
                        {
                            Log?.Invoke($"\n\n !!!Возникла ошибка {BotName.Name}  " + ex.Message + "\n\n");
                        }
                    }
                    return;
                }
                if (e.Message.Text == personalConfig.Buttons["LOCATION"])
                {
                    await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["LOCATION"]);
                    await bot.SendLocationAsync(currentUser.ChatId, personalConfig.Latitude, personalConfig.Longitude, replyMarkup: UserKeyboard.GetStartKeyboard(options));
                    return;
                }
                if (e.Message.Text == personalConfig.Buttons["LINK"])
                {
                    await bot.SendTextMessageAsync(currentUser.ChatId, "👇", replyMarkup: UserKeyboard.GetLinkButtons(options));
                    return;
                }
                if (e.Message.Text == "/reg" + botConfig.Password && admin == null)
                {
                    await dateFunction.CreateMonthsAsync(DateTime.Now);
                    await context.Creater.AddMonthAsync(dateFunction.CurrentMonth);
                    await context.Creater.CreateDaysAsync(dateFunction.CurrentDay, dateFunction.CurrentMonth, DateFunction.DayNames);
                    List<Day> daysCurrentMonth = await context.Finder.FindDaysAsync(dateFunction.CurrentMonth.MonthId);
                    foreach (Day day in daysCurrentMonth)
                    {
                        await context.Creater.CreateAppointmentsAtDayAsync(botConfig.appointmentStandartTimes, day.DayId);
                    }
                    await context.Creater.AddMonthAsync(dateFunction.NextMonth);
                    await context.Creater.CreateDaysAsync(1, dateFunction.NextMonth, DateFunction.DayNames);
                    List<Day> daysNextMonth = await context.Finder.FindDaysAsync(dateFunction.NextMonth.MonthId);
                    foreach (Day day in daysNextMonth)
                    {
                        await context.Creater.CreateAppointmentsAtDayAsync(botConfig.appointmentStandartTimes, day.DayId);
                    }
                    currentUser.IsAdmin = true;
                    await context.Updater.UpdateUserAsync(currentUser);
                    admin = await context.Finder.FindAdminAsync();
                    if (admin == null)
                    {
                        Log?.Invoke($"Ошибка при регистрации");
                        return;
                    }
                    await AdminLog?.Invoke(new EventArgsNotification(admin.ChatId, "Зарегестрированно!", AdminKeyboard.GetKeyboardAdmin(options)));

                    return;
                }
                //else
                await bot.SendTextMessageAsync(currentUser.ChatId, personalConfig.Messages["UNKNOWN"]);
                return;
            }
        }
    }
}
