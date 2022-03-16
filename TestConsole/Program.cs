using BotLibary;
using BotLibary.TestingMock;
using DataBase.Database;
using DataBase.Database.Context.MySQL;
using DataBase.Models;
using Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BotLibary.BotManager;

namespace TestConsole
{
    class Program
    {
        static void Log(string text) => Console.WriteLine(text);
        static void Main(string[] args)
        {
            var BotManager = new ConsoleBotManager("D:\\BotManager", Log);
            while (true) 
            {
                
                Console.WriteLine("Ввести T + номер теста");
                string comm = Console.ReadLine();
                if (comm == "T1")
                {
                    //Test1();
                    Console.ReadLine();
                }
                if (comm == "T2")
                {
                    //Test2();
                    Console.ReadLine();
                }
                if (comm == "T3")
                { 
                }
                if (comm == "T4")
                {
                    
                }
                if (comm == "T5")
                {
                   
                }
            }
           














            static async Task ConsoleAsync(string text)
            {
                await Task.Run(() => Console.WriteLine(text));
            }
            static async Task Test1()
            {
                var rep = new MySqlRepository();
                rep.Init(new MySqlReader("ConsltestDF"),
                         new MySqlCreater("ConsltestDF"),
                         new MySqlUpdater("ConsltestDF"),
                         new MySqlErraiser("ConsltestDF"));
                DataBaseConnector connector = new DataBaseConnector(rep);
                var CrudDb = connector.db;
                User admin = await CrudDb.Reader.FindAdminAsync();
                var currentUser = await CrudDb.Reader.FindUserAsync(123L);
                if (currentUser == null)
                {
                    currentUser = await CrudDb.Creater.CreateNewUserAsync("Яша", "Мыколаичь", "Жопко", 123L);
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
                await df.CreateMonthsAsync(DateTime.Now);
                await ConsoleAsync("2");
                await CrudDb.Creater.AddMonthAsync(df.getCurrentMonth());
                await ConsoleAsync("3");
                await CrudDb.Creater.CreateDaysAsync(df.getCurrentDay(), df.getCurrentMonth(), df.getNames());
                List<Day> daysCurrentMonth = await CrudDb.Reader.FindDaysAsync(df.getCurrentMonth().MonthId);
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
                        await CrudDb.Creater.AddAppointmentAsync(app);
                    }
                }
                await CrudDb.Creater.AddMonthAsync(df.getNextMonth());
                await CrudDb.Creater.CreateDaysAsync(1, df.getNextMonth(), df.getNames());
                List<Day> daysNextMonth = await CrudDb.Reader.FindDaysAsync(df.getNextMonth().MonthId);
                foreach (Day day in daysNextMonth)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Appointment app = new Appointment(appTime[i], day.DayId);
                        await CrudDb.Creater.AddAppointmentAsync(app);
                    }
                }
                currentUser.IsAdmin = true;
                await CrudDb.Updater.UpdateUserAsync(currentUser);
                admin = await CrudDb.Reader.FindAdminAsync();
                if (admin == null)
                {
                    await ConsoleAsync($"Ошибка при регистрации");
                    return;
                }
                await ConsoleAsync("zareg\n");





                var days = await CrudDb.Reader.FindDaysAsync(df.getCurrentMonth().MonthId);
                List<Appointment> allApps = new List<Appointment>();
                foreach (Day day in days)
                {
                    var apps = await CrudDb.Reader.FindAppointmentsAsync(day.DayId);
                    allApps.AddRange(apps);
                }
                
                await ConsoleAsync($"Создано расписание\n {days.Count} - колличество дней \n {allApps.Count} - количество записей");
                await ConsoleAsync($"{currentUser.ChatId}, Создано рассписание на {days.Count} дней\n\n\n\n");

                var days2 = await CrudDb.Reader.FindDaysAsync(df.getNextMonth().MonthId);
                List<Appointment> allApps2 = new List<Appointment>();
                foreach (Day day in days)
                {
                    var apps = await CrudDb.Reader.FindAppointmentsAsync(day.DayId);
                    allApps.AddRange(apps);
                }

                await ConsoleAsync($"Создано расписание\n {days2.Count} - колличество дней \n {allApps2.Count} - количество записей");
                await ConsoleAsync($"{currentUser.ChatId}, Создано рассписание на {days.Count} дней");

            }            
           
        }
    }
}
