using BotLibary.BotManager.Interfaces;
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
                if (await context.Finder.FindAdminAsync() != null)
                {
                    await Task.Run(() => { Thread.Sleep(85000000); });
                    Log?.Invoke($"Проверка обновлений у бота {BotName.Name}\n");
                    await CheckUpdateAsync();
                }

            }
        }
        protected virtual async Task CheckUpdateAsync()
        {
            while(DateTime.Now.Day > dateFunction.CurrentDay)
            {
                Day pastDay = await context.Finder.GetFirstDayAsync();
                List<Appointment> pastApps = await context.Finder.FindAppointmentsAsync(pastDay.DayId);
                await context.Updater.DeleteDaysAsync(new List<Day> {pastDay});
                await context.Updater.DeleteAppointmentsAsync(pastApps);
                await dateFunction.IncreementDayAsync();
                if (dateFunction.CurrentDay == 1)
                {
                    List<Month> pastMonths = await context.Finder.GetMonthsAsync();
                    Month pastMonth = await context.Finder.GetFirstMonthAsync();
                    await context.Updater.DeleteMonthsAsync(new List<Month> { pastMonth });
                    await context.Creater.AddMonthAsync(dateFunction.NextMonth);
                    await context.Creater.CreateDaysAsync(dateFunction.CurrentDay, dateFunction.NextMonth, DateFunction.DayNames);
                    List<Day> daysCurrentMonth = await context.Finder.FindDaysAsync(dateFunction.CurrentMonth.MonthId);
                    foreach (Day day in daysCurrentMonth)
                    {
                        for (int i = 0; i < botConfig.appointmentStandartCount; i++)
                        {                           
                            await context.Creater.AddAppointmentAsync(new Appointment(botConfig.appointmentStandartTimes[i], day.DayId));
                        }
                    }
                }
            }
        }
        protected async virtual void StartNotificationAsync(DateTime time)
        {
            Log?.Invoke($"Начало проверки отправки уведомлений у бота {BotName.Name}\n");
            while (true)
            {
                await Task.Run(() =>
                {
                    Thread.Sleep(30000000);
                    Log?.Invoke($"Проверка необходимости отправки уведомлений у бота {BotName.Name}\n");
                });
                await CheckNotificationAsync(time);

            }
        }
        public async virtual Task CheckNotificationAsync(DateTime time)
        {
            await Task.Run(async () =>
            {
                if (time.Hour <= 19 && time.Hour >= 18)
                {
                    Day firstDay = await context.Finder.GetFirstDayAsync();
                    List<Appointment> apps = await context.Finder.FindAppointmentsAsync(firstDay.DayId);
                    var admin = await context.Finder.FindAdminAsync();
                    foreach (Appointment app in apps)
                    {
                        if (app.IsConfirm)
                        {
                            var user = await context.Finder.FindUserAsync(app.User);
                            await bot.SendTextMessageAsync(user.ChatId, personalConfig.Messages["NOTIFICATION"] + "\n " + $"на время {app.AppointmentTime}");
                            AdminLog?.Invoke(new EventArgsNotification(admin.ChatId, $"{user.FirstName} {user.LastName} @{user.Username} записан на завтра на время {app.AppointmentTime}"));
                        }
                    }
                }
            });
            
        }
    }
}
