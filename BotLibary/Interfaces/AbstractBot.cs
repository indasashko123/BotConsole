using BotLibary.TelegramBot;
using BotLibary.TelegramBot.Handlers;
using DataBase.Database;
using DataBase.Models;
using Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.Interfaces
{
    public abstract class AbstractBot
    {
        public AbstractBot()
        {

        }
        protected internal ImageManager imageManager;
        protected internal TelegramBotClient bot;
        protected internal BotOptions options;
        protected internal BotConfig botConfig;
        protected internal PersonalConfig personalConfig;
        protected internal Message lastMessage;
        protected internal DateFunction dateFunction;
        protected internal DataBaseConnector context;
        protected internal ErrorHandler errorHandler;
        protected internal Handler handler;
        protected internal BotName BotName {get;set;}        
        protected virtual async void StartUpdateDays()
        {           
            while (true)
            {
                if (await context.db.FindAdminAsync() != null)
                {
                    await Task.Delay(1000000);                  
                    await CheckUpdateAsync();
                }                
            }
        }
        protected virtual async Task CheckUpdateAsync()
        {
            await Task.Run(() => CheckUpdate(DateTime.Now));
        }
        protected virtual async void CheckUpdate(DateTime timeNow)
        {
            var admin = await context.db.FindAdminAsync();
            if (admin == null)
            {
                return;
            }
            while (timeNow.Day > dateFunction.CurrentDay)
            {
                Day pastDay = await context.db.GetFirstDayAsync();
                List<Appointment> pastApps = await context.db.FindAppointmentsAsync(pastDay.DayId);
                await bot.SendTextMessageAsync(admin.ChatId, $"Удалаяем день {pastDay.Date}");
                await context.db.DeleteDaysAsync(new List<Day> { pastDay });
                await context.db.DeleteAppointmentsAsync(pastApps);
                await dateFunction.IncreementDayAsync();
                if (dateFunction.CurrentDay == 1)
                {
                    Month pastMonth = await context.db.GetFirstMonthAsync();
                    await context.db.DeleteMonthsAsync(new List<Month> { pastMonth });
                    await context.db.AddMonthAsync(dateFunction.NextMonth);
                    await context.db.CreateDaysAsync(dateFunction.CurrentDay, dateFunction.NextMonth, dateFunction.DayNames);
                    List<Day> daysNextMonth = await context.db.FindDaysAsync(dateFunction.NextMonth.MonthId);
                    foreach (Day day in daysNextMonth)
                    {
                        for (int i = 0; i < botConfig.appointmentStandartTimes.Count; i++)
                        {
                            await context.db.AddAppointmentAsync(new Appointment(botConfig.appointmentStandartTimes[i], day.DayId));
                        }
                    }
                }
            }
        }

        protected async virtual void StartNotificationAsync()
        {
            while (true)
            {
                await Task.Delay(10000000);            
                await CheckNotificationAsync();
            }
        }
        protected async virtual void CheckNotification()
        {
            var admin = await context.db.FindAdminAsync();
            if (admin == null)
            {
                return;
            }
            DateTime timeNow = DateTime.Now;           
            if (timeNow.Hour >= 15 && timeNow.Hour <= 20)
            { 
                Day firstDay = await context.db.GetFirstDayAsync();
                List<Appointment> apps = await context.db.FindAppointmentsAsync(firstDay.DayId);               
                foreach (Appointment app in apps)
                {
                    if (app.IsConfirm)
                    {
                        var user = await context.db.FindUserAsync(app.User);
                        try
                        {
                            await bot.SendTextMessageAsync(user.ChatId, personalConfig.Messages["NOTIFICATION"] + "\n " + $"на время {app.AppointmentTime}");
                        }
                        catch
                        {
                            await bot.SendTextMessageAsync(admin.ChatId, $"Пользователь {user} заблокировал уведомления");
                        }
                        await bot.SendTextMessageAsync(admin.ChatId, $"{user.FirstName} {user.LastName} @{user.Username} записан на завтра на время {app.AppointmentTime}");
                    }
                }
            }
        }
        protected async Task CheckNotificationAsync()
        {
            await Task.Run(() => CheckNotification());
        }       
    }
}
