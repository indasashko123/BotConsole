
using DataBase.Database.Interface;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DataBase.Database.Context.MySQL
{
    public class MySqlCreater : ICreater
    {
        string DataBaseName;
        protected virtual DbContext Connect()
        {
            return new MySqlDbContext(DataBaseName);
        }
        public MySqlCreater(string DataBaseName)
        {
            this.DataBaseName = DataBaseName;
        }
        public void CreateDataBase()
        {
            Connect().Database.EnsureCreated();
        }
        User ICreater.CreateNewUser(string username, string firstName, string lastName, long id)
        {
            User currentUser = new User(username, firstName, lastName, id);
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                db.Users.Add(currentUser);
                db.SaveChanges();
            }
            return currentUser;
        }
        void ICreater.AddMonth(Month month)
        {
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                db.Months.Add(month);
                db.SaveChanges();
            }
        }
        void ICreater.CreateDays(int currentDay, Month month, Dictionary<int, string> dayNames)
        {
            while (currentDay <= month.DayCount)
            {
                int dayOfWeek = (int)new DateTime(month.Year, month.MonthNumber, currentDay).DayOfWeek;
                Day day = new Day(month, currentDay, dayNames[dayOfWeek]);
                using (MySqlDbContext db = Connect() as MySqlDbContext)
                {
                    db.Days.Add(day);
                    db.SaveChanges();
                }
                currentDay++;
            }
        }
        Appointment ICreater.AddAppointment(int dayId)
        {
            Appointment app = new Appointment(dayId);
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                db.Appointments.Add(app);
                db.SaveChanges();
            }
            return app;
        }
        void ICreater.AddAppointment(Appointment app)
        {
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                db.Appointments.Add(app);
                db.SaveChanges();
            }
        }
        

    }
}
