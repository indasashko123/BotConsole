using DataBase.Masters.Database.Interface;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Masters.Database.Context.MySQL
{
    public class CreaterEntity : ICreaterEntity
    {
        string DataBaseName { get; set; }
        public CreaterEntity(string dbName)
        {
            DataBaseName = dbName;
            Connection db = new Connection(DataBaseName);
            db.CreateDataBase();
        }      
        User ICreaterEntity.CreateNewUser(string username, string firstName, string lastName, long id)
        {
            User currentUser = new User(username, firstName, lastName, id);
            using (Connection db = new Connection(DataBaseName))
            {
                db.Users.Add(currentUser);
                db.SaveChanges();
            }
            return currentUser;
        }
        void ICreaterEntity.AddMonth(Month month)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Months.Add(month);
                db.SaveChanges();
            }
        }
        void ICreaterEntity.CreateDays(int currentDay, Month month, Dictionary<int, string> dayNames)
        {
            while (currentDay <= month.DayCount)
            {
                int dayOfWeek = (int)new DateTime(month.Year, month.MonthNumber, currentDay).DayOfWeek;
                Day day = new Day(month, currentDay, dayNames[dayOfWeek]);
                using (Connection db = new Connection(DataBaseName))
                {
                    db.Days.Add(day);
                    db.SaveChanges();
                }
                currentDay++;
            }
        }
        Appointment ICreaterEntity.AddAppointment(int dayId)
        {
            Appointment app = new Appointment(dayId);
            using (Connection db = new Connection(DataBaseName))
            {
                db.Appointments.Add(app);
                db.SaveChanges();
            }
            return app;
        }
        void ICreaterEntity.AddAppointment(Appointment app)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Appointments.Add(app);
                db.SaveChanges();
            }
        }
        void ICreaterEntity.CreateAppointmentsAtDay(List<string> times, int dayId)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                for (int i=0; i<times.Count; i++)
                {
                    Appointment app = new Appointment(times[i], dayId);
                    db.Appointments.Add(app);
                    db.SaveChanges();
                }
            }
        }
    }
}
