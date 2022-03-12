using BotLibary.Events;
using DataBase.Database;
using DataBase.Database.Context.MySQL;
using DataBase.Models;
using Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        protected internal BotName BotName {get;set;}
        public ChangesLog ConsoleMessage { get; set; }
        public AdminMessage adminMessage { get; set; }


        protected virtual async void StartUpdateDays()
        {           
            while (true)
            {
                if (await context.db.FindAdminAsync() != null)
                {
                    await Task.Delay(10000000);                  
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
            if (timeNow.Day > dateFunction.CurrentDay)
            {               
                await dateFunction.IncreementDayAsync();
                if (dateFunction.CurrentDay == 1)
                {
                    List<Month> pastMonth = await context.db.GetMonthsAsync(new int[]
                        {dateFunction.CurrentMonth.MonthId,
                        dateFunction.NextMonth.MonthId});
                    List<Appointment> pastAppointment = new List<Appointment>();
                    List<Day> pastDays = new List<Day>();
                    if (pastMonth != null && pastMonth.Count > 0 )
                    {
                        foreach (Month month in pastMonth)
                        {
                            pastDays.AddRange(await context.db.FindDaysAsync(month.MonthId));
                        }
                        foreach (Day day in pastDays)
                        {
                            pastAppointment.AddRange(await context.db.FindAppointmentsAsync(day.DayId));
                        }
                        await context.db.DeleteAppointmentsAsync(pastAppointment);
                        await context.db.DeleteDaysAsync(pastDays);
                        await context.db.DeleteMonthsAsync(pastMonth);
                    }
                    await context.db.AddMonthAsync(dateFunction.NextMonth);
                    await context.db.CreateDaysAsync(dateFunction.CurrentDay, dateFunction.NextMonth, dateFunction.DayNames);
                    List<Day> daysCurrentMonth = await context.db.FindDaysAsync(dateFunction.CurrentMonth.MonthId);
                    foreach (Day day in daysCurrentMonth)
                    {
                        for (int i = 0; i < botConfig.appointmentStandartTimes.Count; i++)
                        {
                            Appointment app = new Appointment(botConfig.appointmentStandartTimes[i], day.DayId);
                            await context.db.AddAppointmentAsync(app);
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
                        await bot.SendTextMessageAsync(user.ChatId, personalConfig.Messages["NOTIFICATION"] + "\n " + $"на время {app.AppointmentTime}");
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
