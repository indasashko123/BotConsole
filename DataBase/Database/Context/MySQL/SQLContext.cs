using DataBase.Database.Interface;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataBase.Database.Context.MySQL
{
    public class SQLContext : ICRUD
    {
        void ICRUD.DeleteDB()
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Database.EnsureDeleted();
            }    
        }
        string DataBaseName { get; set; }
        public SQLContext(string dbName)
        {
            DataBaseName = dbName;
            Connection db = new Connection(DataBaseName);
            db.CreateDataBase();
        }
        Day ICRUD.GetFirstDay()
        {
            Day day;
            using (Connection db = new Connection(DataBaseName))
            {
                day = db.Days.OrderBy(d => d.Date).FirstOrDefault();
            }
            return day;
        }
        User ICRUD.FindAdmin()
        {
            User admin;
            using (Connection db = new Connection(DataBaseName))
            {
               admin = (from user in db.Users
                               where user.IsAdmin
                               select user).FirstOrDefault();
            }
            return admin;
        }

        User ICRUD.FindUser(long chatId)
        {
            User currentUser;
            using (Connection db = new Connection(DataBaseName))
            {
                currentUser = (from user in db.Users
                         where user.ChatId == chatId
                         select user).FirstOrDefault();
            }
            return currentUser;
        }

        User ICRUD.CreateNewUser(string username, string firstName, string lastName, long id)
        {
            User currentUser = new User(username, firstName, lastName, id);
            using (Connection db = new Connection(DataBaseName))
            {
                db.Users.Add(currentUser);
                db.SaveChanges();
            }
            return currentUser;
        }

        List<Month> ICRUD.GetMonths()
        {
            List<Month> months;
            using (Connection db = new Connection(DataBaseName))
            {
                months = db.Months.ToList();
            }
            return months;
        }

        List<Day> ICRUD.FindDays(int monthId)
        {
            List<Day> days;
            using (Connection db = new Connection(DataBaseName))
            {
                days = (from day in db.Days
                        where day.Month == monthId
                        select day).ToList();
            }
            return days;
        }

        void ICRUD.AddMonth(Month month)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Months.Add(month);
                db.SaveChanges();
            }
        }

        void ICRUD.CreateDays(int currentDay, Month month, Dictionary<int, string> dayNames)
        {
            while (currentDay <= month.DayCount )
            {
                int dayOfWeek =(int) new DateTime(month.Year, month.MonthNumber, currentDay).DayOfWeek;
                Day day = new Day(month, currentDay, dayNames[dayOfWeek] );
                using (Connection db = new Connection(DataBaseName))
                {
                    db.Days.Add(day);
                    db.SaveChanges();
                }
                currentDay++;
            }
        }
         
        List<Appointment> ICRUD.FindAppointments(int dayId)
        {
            List<Appointment> apps;
            using (Connection db = new Connection(DataBaseName)) 
            {
                apps = (from app in db.Appointments
                        where app.Day == dayId
                        select app).ToList();
            }
            return apps;
        }

        Appointment ICRUD.FindAppointment(int appId)
        {
            Appointment app;
            using (Connection db = new Connection(DataBaseName))
            {
                app = (from appointment in db.Appointments
                       where appointment.AppointmentId == appId
                       select appointment).FirstOrDefault();
            }
            return app;
        }

        Day ICRUD.FindDay(int dayid)
        {
            Day day;
            using (Connection db = new Connection(DataBaseName))
            {
                day = (from findday in db.Days
                       where findday.DayId == dayid
                       select findday).FirstOrDefault();
            }
            return day;
        }

        void ICRUD.UpdateApp(Appointment app)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Appointments.Update(app);
                db.SaveChanges();
            }
        }

        void ICRUD.UpdateUser(User user)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Users.Update(user);
                db.SaveChanges();
            }
        }

        void ICRUD.DeleteApp(Appointment app)
        {
           using (Connection db = new Connection(DataBaseName))
            {
                db.Appointments.Remove(app);
                db.SaveChanges();
            }
        }

        Appointment ICRUD.AddAppointment(int dayId)
        {
            Appointment app = new Appointment(dayId);
            using (Connection db = new Connection(DataBaseName))
            {
                db.Appointments.Add(app);
                db.SaveChanges();
            }
            return app;
        }

        void ICRUD.AddAppointment(Appointment app)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Appointments.Add(app);
                db.SaveChanges();
            }
        }
      
        User ICRUD.FindUser(int userId)
        {
            User currentUser;
            using (Connection db = new Connection(DataBaseName))
            {
                currentUser = (from user in db.Users
                               where user.UserId == userId
                               select user).FirstOrDefault();
            }
            return currentUser;
        }

        List<User> ICRUD.FindUsers()
        {
            List<User> users;
            using (Connection db = new Connection(DataBaseName))
            {
                users = db.Users.ToList();
            }
            return users;
        }

        void ICRUD.UpdateDay(Day day)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Days.Update(day);
                db.SaveChanges();
            }
        }

        List<Appointment> ICRUD.FindConfirmAppointments(bool IsConfirm)
        {
            List<Appointment> apps;
            using (Connection db = new Connection(DataBaseName))
            {
                apps = (from app in db.Appointments
                        where app.IsConfirm == IsConfirm && app.IsEmpty == false
                        select app).ToList();
            }
            return apps;
        }

        List<Month> ICRUD.GetMonths(params int[] monthsId)
        {
            List<Month> ExeptMonth = new List<Month>();
            using (Connection db = new Connection(DataBaseName))
            {
                List<Month> months = db.Months.ToList();
                foreach (Month month in months)
                {
                    bool IsNeedToAdd = true;
                    foreach (int id in monthsId)
                    {
                        if (month.MonthId == id)
                        {
                            IsNeedToAdd = false;
                        }
                    }
                    if (IsNeedToAdd)
                    {
                        ExeptMonth.Add(month);
                    }
                }
            }
            return ExeptMonth;
        }

        void ICRUD.DeleteAppointments(List<Appointment> apps)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Appointments.RemoveRange(apps);
                db.SaveChanges();
            }
        }

        void ICRUD.DeleteDays(List<Day> days)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Days.RemoveRange(days);
                db.SaveChanges();
            }
        }

        void ICRUD.DeleteMonths(List<Month> months)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Months.RemoveRange(months);
                db.SaveChanges();
            }
        }

    }
}
