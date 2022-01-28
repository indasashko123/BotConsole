using DataBase.Database.Interface;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Database.Context.MySQL
{
    public class SQLContext : ICRUD
    {
        string DataBaseName { get; set; }
        public SQLContext(string dbName)
        {
            DataBaseName = dbName;
        }

        User ICRUD.FindAdmin()
        {
            User admin;
            using (Connection db = new Connection(DataBaseName))
            {
               admin = (from user in db.Users
                               where user.isAdmin
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
                         where user.chatId == chatId
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
                apps = (from app in db.Appoinmetns
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
                app = (from appointment in db.Appoinmetns
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
                db.Appoinmetns.Update(app);
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
                db.Appoinmetns.Remove(app);
                db.SaveChanges();
            }
        }

        Appointment ICRUD.AddAppointment(int dayId)
        {
            Appointment app = new Appointment(dayId);
            using (Connection db = new Connection(DataBaseName))
            {
                db.Appoinmetns.Add(app);
                db.SaveChanges();
            }
            return app;
        }

        void ICRUD.AddAppointment(Appointment app)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Appoinmetns.Add(app);
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
                apps = (from app in db.Appoinmetns
                        where app.IsConfirm == IsConfirm
                        select app).ToList();
            }
            return apps;
        }
    }
}
