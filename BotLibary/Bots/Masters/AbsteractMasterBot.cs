﻿using BotLibary.BotManager.Interfaces;
using BotLibary.Bots.Events;
using BotLibary.Bots.Interfaces;
using DataBase.Models;
using Options;
using Options.MasterBotConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BotLibary.Bots.Masters
{
    public class AbstractMasterBot : AbstractBot
    {
        public IImageManager ImageManager;
        protected internal DateFunction dateFunction;
        protected internal BotOptions<MasterBotConfig, PersonalMasterBotConfig> options;
        protected internal MasterBotConfig botConfig;
        protected internal PersonalMasterBotConfig personalConfig;
        public ChangesLog Log { get; set; }
        public AdminMessage AdminLog { get; set; }
        protected virtual async void StartUpdateDays()
        {
            Log?.Invoke($"Начало проверки обновлений у бота {BotName.Name}\n");
            while (true)
            {
                if (await context.db.FindAdminAsync() != null)
                {
                    await Task.Run(() => { Thread.Sleep(85000000); });
                    Log?.Invoke($"Проверка обновлений у бота {BotName.Name}\n");
                    await CheckUpdateAsync();
                }

            }
        }
        protected virtual async Task CheckUpdateAsync()
        {
            await Task.Run(() => CheckUpdate());

        }
        protected virtual async void CheckUpdate()
        {
            if (DateTime.Now.Day > dateFunction.CurrentDay)
            {
                await dateFunction.IncreementDayAsync();
                if (dateFunction.CurrentDay == 1)
                {
                    List<Month> pastMonth = await context.db.GetMonthsAsync(new int[]
                        {dateFunction.CurrentMonth.MonthId,
                        dateFunction.NextMonth.MonthId});
                    List<Appointment> pastAppointment = new List<Appointment>();
                    List<Day> pastDays = new List<Day>();
                    if (pastMonth != null)
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
                        for (int i = 0; i < botConfig.appointmentStandartCount; i++)
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
            Log?.Invoke($"Начало проверки отправки уведомлений у бота {BotName.Name}\n");
            while (true)
            {
                await Task.Run(() =>
                {
                    Thread.Sleep(30000000);
                    Log?.Invoke($"Проверка необходимости отправки уведомлений у бота {BotName.Name}\n");
                });
                await CheckNotificationAsync();

            }
        }
        protected async virtual void CheckNotification()
        {
            DateTime timeNow = DateTime.Now;
            if (timeNow.Hour <= 19 && timeNow.Hour >= 18)
            {
                List<Day> days = await context.db.FindDaysAsync(dateFunction.CurrentMonth.MonthId);
                days = days.OrderBy(day => day.Date).ToList();
                Day firstDay = days.FirstOrDefault();
                List<Appointment> apps = await context.db.FindAppointmentsAsync(firstDay.DayId);
                var admin = await context.db.FindAdminAsync();
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