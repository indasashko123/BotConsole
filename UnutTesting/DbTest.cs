using DataBase.Database;
using DataBase.Database.Context.MySQL;
using DataBase.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotLibary.TestingMock;

namespace UnutTesting
{
    [TestClass]
    public class DbTest
    {

        [TestMethod]
        public void TestMethod1()
        {
            ///Context
            MySqlRepository rep = new MySqlRepository();
            rep.Init(new MySqlReader("tst"), new MySqlCreater("tst"), new MySqlUpdater("tst"), new MySqlErraiser("tst"));
            DataBaseConnector context = new DataBaseConnector(rep);
            context.db.Erraiser.DeleteDataBase();
            context.db.Creater.CreateDataBase();
            /// DateFunction
            DateTime date = new DateTime(2000, 1, 1);
            TestDateFunctionFAKE dateFunction = new TestDateFunctionFAKE();
            dateFunction.CreatMonth(date);
            List<Day> answer = new List<Day>();
            /// options
            List<string> appCounts = new List<string>{ "12:00", "14:00" };

            /// Test
            Task.Run(async () =>
                {
                    User currentUser = await context.db.Creater.CreateNewUserAsync("jopa", "S", "ruchkoi", 514842497);
                    await context.db.Creater.AddMonthAsync(dateFunction.getCurrentMonth());
                    await context.db.Creater.CreateDaysAsync(dateFunction.getCurrentDay(), dateFunction.getCurrentMonth(), dateFunction.getNames());
                    List<Day> daysCurrentMonth = await context.db.Reader.FindDaysAsync(dateFunction.getCurrentMonth().MonthId);
                    foreach (Day day in daysCurrentMonth)
                    {
                        for (int i = 0; i < appCounts.Count; i++)
                        {
                            Appointment app = new Appointment(appCounts[i], day.DayId);
                            await context.db.Creater.AddAppointmentAsync(app);
                        }
                    }
                    await context.db.Creater.AddMonthAsync(dateFunction.getNextMonth());
                    await context.db.Creater.CreateDaysAsync(1, dateFunction.getNextMonth(), dateFunction.getNames());
                    List<Day> daysNextMonth = await context.db.Reader.FindDaysAsync(dateFunction.getNextMonth().MonthId);
                    foreach (Day day in daysNextMonth)
                    {
                        for (int i = 0; i < appCounts.Count; i++)
                        {
                            Appointment app = new Appointment(appCounts[i], day.DayId);
                            await context.db.Creater.AddAppointmentAsync(app);
                        }
                    }
                    currentUser.IsAdmin = true;
                    await context.db.Updater.UpdateUserAsync(currentUser);
                    User admin = await context.db.Reader.FindAdminAsync();

                    /// answer
                    answer = await context.db.Reader.FindDaysAsync(dateFunction.getCurrentMonth().MonthId);
                }).Wait();
            Assert.AreEqual(dateFunction.getCurrentMonth().DayCount, answer.Count);
        }
        [TestMethod]
        public void Test2()
        {
            ///Context
            MySqlRepository rep = new MySqlRepository();
            rep.Init(new MySqlReader("tst2"), new MySqlCreater("tst2"), new MySqlUpdater("tst2"), new MySqlErraiser("tst2"));
            DataBaseConnector context = new DataBaseConnector(rep);
            context.db.Erraiser.DeleteDataBase();
            context.db.Creater.CreateDataBase();
            /// DateFunction
            DateTime date = new DateTime(2000, 1, 1);
            TestDateFunctionFAKE dateFunction = new TestDateFunctionFAKE();
            dateFunction.CreatMonth(date);
            /// options
            List<string> appCounts = new List<string> { "12:00", "14:00" };

            /// Test
            List<Day> answer = new List<Day>();
            DateTime timeNow = new DateTime(2000, 1, 2);
            Task.Run(async () => {
                var admin = await context.db.Reader.FindAdminAsync();
                if (admin == null)
                {
                    return;
                }
                while (timeNow.Day > dateFunction.getCurrentDay())
                {
                    Day pastDay = await context.db.Reader.GetFirstDayAsync();
                    List<Appointment> pastApps = await context.db.Reader.FindAppointmentsAsync(pastDay.DayId);                   
                    await context.db.Erraiser.DeleteDaysAsync(new List<Day> { pastDay });
                    await context.db.Erraiser.DeleteAppointmentsAsync(pastApps);
                    await Task.Run(()=>dateFunction.IncreementDay());
                    if (dateFunction.getCurrentDay() == 1)
                    {
                        Month pastMonth = await context.db.Reader.GetFirstMonthAsync();
                        await context.db.Erraiser.DeleteMonthsAsync(new List<Month> { pastMonth });
                        await context.db.Creater.AddMonthAsync(dateFunction.getNextMonth());
                        await context.db.Creater.CreateDaysAsync(dateFunction.getCurrentDay(), dateFunction.getNextMonth(), dateFunction.getNames());
                        List<Day> daysNextMonth = await context.db.Reader.FindDaysAsync(dateFunction.getNextMonth().MonthId);
                        foreach (Day day in daysNextMonth)
                        {
                            for (int i = 0; i < appCounts.Count; i++)
                            {
                                await context.db.Creater.AddAppointmentAsync(new Appointment(appCounts[i], day.DayId));
                            }
                        }
                        answer = await context.db.Reader.FindDaysAsync(dateFunction.getCurrentMonth().MonthId);
                    }
                }
                Assert.AreEqual(dateFunction.getCurrentMonth().DayCount-1, answer.Count);

            }).Wait();
        }
    }
}
