using BotLibary;
using BotLibary.TestingMock;
using DataBase.Database;
using DataBase.Database.Context.MySQL;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ввести T + номер теста");
            string comm = Console.ReadLine();            
            if (comm == "T1")
            {
                Test1();
                Console.ReadLine();
            }
            if (comm == "T2")
            {
                ///Test2();
                Console.ReadLine();
            }
            
            static async Task ConsoleAsync(string text)
            {
                await Task.Run(() => Console.WriteLine(text));
            }
            static async Task Test1()
            {
                DataBaseConnector connector = new DataBaseConnector(new SQLContext("Test001"));
                var CrudDb = connector.db;
                User admin = await CrudDb.FindAdminAsync();
                var currentUser = await CrudDb.FindUserAsync(123L);
                if (currentUser == null)
                {
                    currentUser = await CrudDb.CreateNewUserAsync("Яша", "Мыколаичь", "Жопко", 123L);
                }
                if (currentUser.IsAdmin)
                {
                    await ConsoleAsync("Жопко - одмэн");
                }
                else
                {
                    await ConsoleAsync("Жопко просто юзер");
                }
                
                TestDateFunctionFAKE df = new TestDateFunctionFAKE();
                await ConsoleAsync("1");
                await df.CreateMonthsAsync();
                await ConsoleAsync("2");
                await CrudDb.AddMonthAsync(df.getCurrentMonth());
                await ConsoleAsync("3");
                await CrudDb.CreateDaysAsync(df.getCurrentDay(), df.getCurrentMonth(), df.getNames());
                List<Day> daysCurrentMonth = await CrudDb.FindDaysAsync(df.getCurrentMonth().MonthId);
                List<string> appTime = new List<string>()
                {
                "10-00",
                "14-00",
                "18-00"
                };
                foreach (Day day in daysCurrentMonth)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Appointment app = new Appointment(appTime[i], day.DayId);
                        await CrudDb.AddAppointmentAsync(app);
                    }
                }
                await CrudDb.AddMonthAsync(df.getNextMonth());
                await CrudDb.CreateDaysAsync(1, df.getNextMonth(), df.getNames());
                List<Day> daysNextMonth = await CrudDb.FindDaysAsync(df.getNextMonth().MonthId);
                foreach (Day day in daysNextMonth)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Appointment app = new Appointment(appTime[i], day.DayId);
                        await CrudDb.AddAppointmentAsync(app);
                    }
                }
                currentUser.IsAdmin = true;
                await CrudDb.UpdateUserAsync(currentUser);
                admin = await CrudDb.FindAdminAsync();
                if (admin == null)
                {
                    await ConsoleAsync($"Ошибка при регистрации");
                    return;
                }
                await ConsoleAsync("zareg\n");





                var days = await CrudDb.FindDaysAsync(df.getCurrentMonth().MonthId);
                List<Appointment> allApps = new List<Appointment>();
                foreach (Day day in days)
                {
                    var apps = await CrudDb.FindAppointmentsAsync(day.DayId);
                    allApps.AddRange(apps);
                }
                
                await ConsoleAsync($"Создано расписание\n {days.Count} - колличество дней \n {allApps.Count} - количество записей");
                await ConsoleAsync($"{currentUser.ChatId}, Создано рассписание на {days.Count} дней\n\n\n\n");

                var days2 = await CrudDb.FindDaysAsync(df.getNextMonth().MonthId);
                List<Appointment> allApps2 = new List<Appointment>();
                foreach (Day day in days)
                {
                    var apps = await CrudDb.FindAppointmentsAsync(day.DayId);
                    allApps.AddRange(apps);
                }

                await ConsoleAsync($"Создано расписание\n {days2.Count} - колличество дней \n {allApps2.Count} - количество записей");
                await ConsoleAsync($"{currentUser.ChatId}, Создано рассписание на {days.Count} дней");

            }







        }
    }
}
