using DataBase.Database.Interface;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataBase.Database.Context.MySQL
{
    public class MySqlReader : IReader
    {  
        string DataBaseName { get; set; }
        protected virtual DbContext Connect()
        {
            return new MySqlDbContext(DataBaseName);
        }
        public MySqlReader(string dbName)
        {
            DataBaseName = dbName;
        }
        Month IReader.GetFirstMonth()
        {
            Month month;
            using(MySqlDbContext db = Connect() as MySqlDbContext)
            {
                month = db.Months.OrderBy(m => m.MonthNumber).FirstOrDefault();
            }
            return month;
        }
        Day IReader.GetFirstDay()
        {           
            Day day;
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                Month firstMonth = db.Months.OrderBy(m => m.MonthNumber).FirstOrDefault();
                if (firstMonth == null)
                {
                    throw new Exception("Не найден месяц");
                }
                    day = db.Days.Where(d=>d.Month == firstMonth.MonthId).OrderBy(d => d.Date).FirstOrDefault();
            }
            return day;
        }
        User IReader.FindAdmin()
        {
            User admin;
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
               admin = (from user in db.Users
                               where user.IsAdmin
                               select user).FirstOrDefault();
            }
            return admin;
        }
        User IReader.FindUser(long chatId)
        {
            User currentUser;
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                currentUser = (from user in db.Users
                         where user.ChatId == chatId
                         select user).FirstOrDefault();
            }
            return currentUser;
        }
        List<Month> IReader.GetMonths()
        {
            List<Month> months;
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                months = db.Months.ToList();
            }
            return months;
        }
        List<Day> IReader.FindDays(int monthId)
        {
            List<Day> days;
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                days = (from day in db.Days
                        where day.Month == monthId
                        select day).ToList();
            }
            return days;
        }
        List<Appointment> IReader.FindAppointments(int dayId)
        {
            List<Appointment> apps;
            using (MySqlDbContext db = Connect() as MySqlDbContext) 
            {
                apps = (from app in db.Appointments
                        where app.Day == dayId
                        select app).ToList();
            }
            return apps;
        }
        Appointment IReader.FindAppointment(int appId)
        {
            Appointment app;
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                app = (from appointment in db.Appointments
                       where appointment.AppointmentId == appId
                       select appointment).FirstOrDefault();
            }
            return app;
        }
        Day IReader.FindDay(int dayid)
        {
            Day day;
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                day = (from findday in db.Days
                       where findday.DayId == dayid
                       select findday).FirstOrDefault();
            }
            return day;
        }
        User IReader.FindUser(int userId)
        {
            User currentUser;
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                currentUser = (from user in db.Users
                               where user.UserId == userId
                               select user).FirstOrDefault();
            }
            return currentUser;
        }
        List<User> IReader.FindUsers()
        {
            List<User> users;
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                users = db.Users.ToList();
            }
            return users;
        }
        List<Appointment> IReader.FindConfirmAppointments(bool IsConfirm)
        {
            List<Appointment> apps;
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                apps = (from app in db.Appointments
                        where app.IsConfirm == IsConfirm && app.IsEmpty == false
                        select app).ToList();
            }
            return apps;
        }
        List<Month> IReader.GetMonths(params int[] monthsId)
        {
            List<Month> ExeptMonth = new List<Month>();
            using (MySqlDbContext db = Connect() as MySqlDbContext)
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
        
    }
}
