using BotLibary.Interfaces;
using DataBase.Database;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BotLibary.TestingMock
{
    public class AbstractBotMockDateFunctionTest : AbstractBot
    {
        public void SetContext(DataBaseConnector connector)
        {
            this.context = connector;
            context.db.DeleteDb();
        }
        public AbstractBotMockDateFunctionTest()
        {
            this.dateFunction = new TestDateFunctionFAKE().df;
        }
        public TestDateFunctionFAKE GetDF()
        {
            var df = new TestDateFunctionFAKE();
            df.df = dateFunction;
            return df;
        }
        public void CreateDateFunction(DateTime time)
        {
            dateFunction.CreateMonths(time);
            AddDb(dateFunction.CurrentDay, dateFunction.CurrentMonth);
            Thread.Sleep(2200);
            AddDb(1, dateFunction.NextMonth);
        }
        public void AddDb(int StartDay,Month month)
        {
            context.db.CreateDaysAsync(StartDay, month, dateFunction.DayNames);
            Thread.Sleep(10000);
        }
        public async void UpdateDate(DateTime time)
        {
              this.CheckUpdate(time);
            await Task.Delay(3000);
        }
        public async void GetDays(List<Day> days)
        {
            int monthID = dateFunction.CurrentMonth.MonthId;
            days =await context.db.FindDaysAsync(monthID);
        }
        protected async override void CheckUpdate(DateTime timeNow)
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
                    if (pastMonth != null && pastMonth.Count > 0)
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
    }
}
