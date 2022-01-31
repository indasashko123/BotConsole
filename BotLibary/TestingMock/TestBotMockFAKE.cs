using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLibary;
using DataBase.Models;
using Options;
using DataBase.Database;
using System.Threading;

namespace BotLibary.TestingMock
{
    public class TestBotMockFAKE : Bot
    {
         int HourMin { get; set; }
         int HourMax { get; set; }
        public TestDateFunctionFAKE FAKEdateFunction { get; set; }
        public DataBaseConnector publicContext { get; set; }
        public TestBotMockFAKE(BotOptions options, DateTime time) : base(options)
        {
            publicContext = this.context;
            FAKEdateFunction = new TestDateFunctionFAKE(time);
        }
        protected async override void StartNotificationAsync()
        {;
                await CheckNotificationAsync();
        }       
        public void SetDBTable(Action<DataBaseConnector> setDb)
        {
            setDb?.Invoke(publicContext);
        }

        


        protected async override void CheckNotification()
        {        
            if (HourMax <= 19 && HourMin >= 18)
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
                        
                    }
                }
            }
        }
        public void SetHours(int min, int max)
        {
            HourMin = min;
            HourMax = max;
        }
    }
}
