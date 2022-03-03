using DataBase.Masters.Database.Interface;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Masters.Database.Context.MySQL
{
    public class EntityFinder : IEntityFinder
    {
        string DataBaseName { get; set; }
        public EntityFinder(string dbName)
        {
            DataBaseName = dbName;
            Connection db = new Connection(DataBaseName);
            db.CreateDataBase();
        }
        User IEntityFinder.FindAdmin()
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
        Month IEntityFinder.GetFirstMonth()
        {
            Month firstMonth;
            using (Connection db = new Connection(DataBaseName))
            {
                firstMonth = (from month in db.Months
                              select month).OrderBy(m => m.MonthNumber).FirstOrDefault();
            }
            return firstMonth;
        }
        User IEntityFinder.FindUser(long chatId)
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
        List<Month> IEntityFinder.GetMonths()
        {
            List<Month> months;
            using (Connection db = new Connection(DataBaseName))
            {
                months = db.Months.ToList();
            }
            return months;
        }
        List<Day> IEntityFinder.FindDays(int monthId)
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
        List<Appointment> IEntityFinder.FindAppointments(int dayId)
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
        Appointment IEntityFinder.FindAppointment(int appId)
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
        Day IEntityFinder.FindDay(int dayid)
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
        User IEntityFinder.FindUser(int userId)
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
        List<User> IEntityFinder.FindUsers()
        {
            List<User> users;
            using (Connection db = new Connection(DataBaseName))
            {
                users = db.Users.ToList();
            }
            return users;
        }
        List<Appointment> IEntityFinder.FindConfirmAppointments(bool IsConfirm)
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
        List<Month> IEntityFinder.GetMonths(params int[] monthsId)
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
        Day IEntityFinder.GetFirstDay()
        {
            Day day;
            using (Connection db = new Connection(DataBaseName))
            {
                var month = db.Months.OrderBy(m => m.MonthNumber).First();
                var days = db.Days.Where(d => d.Month == month.MonthId).ToList();
                day = days.OrderBy(d => d.Date).FirstOrDefault();
            }
            return day;
        }
    }
}
