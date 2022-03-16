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
            context.db.Erraiser.DeleteDataBase();
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
            context.db.Creater.CreateDaysAsync(StartDay, month, dateFunction.DayNames);
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
            days =await context.db.Reader.FindDaysAsync(monthID);
        }
        protected async override void CheckUpdate(DateTime timeNow)
        {
            if (timeNow.Day > dateFunction.CurrentDay)
            {
                await dateFunction.IncreementDayAsync();
                if (dateFunction.CurrentDay == 1)
                {
                    List<Month> pastMonth = await context.db.Reader.GetMonthsAsync(new int[]
                        {dateFunction.CurrentMonth.MonthId,
                        dateFunction.NextMonth.MonthId});
                    List<Appointment> pastAppointment = new List<Appointment>();
                    List<Day> pastDays = new List<Day>();
                    if (pastMonth != null && pastMonth.Count > 0)
                    {
                        foreach (Month month in pastMonth)
                        {
                            pastDays.AddRange(await context.db.Reader.FindDaysAsync(month.MonthId));
                        }
                        foreach (Day day in pastDays)
                        {
                            pastAppointment.AddRange(await context.db.Reader.FindAppointmentsAsync(day.DayId));
                        }
                        await context.db.Erraiser.DeleteAppointmentsAsync(pastAppointment);
                        await context.db.Erraiser.DeleteDaysAsync(pastDays);
                        await context.db.Erraiser.DeleteMonthsAsync(pastMonth);
                    }
                    await context.db.Creater.AddMonthAsync(dateFunction.NextMonth);
                    await context.db.Creater.CreateDaysAsync(dateFunction.CurrentDay, dateFunction.NextMonth, dateFunction.DayNames);
                    List<Day> daysCurrentMonth = await context.db.Reader.FindDaysAsync(dateFunction.CurrentMonth.MonthId);
                    foreach (Day day in daysCurrentMonth)
                    {
                        for (int i = 0; i < botConfig.appointmentStandartTimes.Count; i++)
                        {
                            Appointment app = new Appointment(botConfig.appointmentStandartTimes[i], day.DayId);
                            await context.db.Creater.AddAppointmentAsync(app);
                        }
                    }
                }
            }
        }
    }
}
