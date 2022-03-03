using DataBase.Masters.Database.Interface;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataBase.Masters.Database.Context.MySQL
{
    public class UpdaterEntity : IUpdaterEntity
    {
        void IUpdaterEntity.DeleteDB()
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Database.EnsureDeleted();
            }    
        }
        string DataBaseName { get; set; }
        public UpdaterEntity(string dbName)
        {
            DataBaseName = dbName;
            Connection db = new Connection(DataBaseName);
            db.CreateDataBase();
        }               
        void IUpdaterEntity.UpdateApp(Appointment app)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Appointments.Update(app);
                db.SaveChanges();
            }
        }
        void IUpdaterEntity.UpdateUser(User user)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Users.Update(user);
                db.SaveChanges();
            }
        }
        void IUpdaterEntity.DeleteApp(Appointment app)
        {
           using (Connection db = new Connection(DataBaseName))
            {
                db.Appointments.Remove(app);
                db.SaveChanges();
            }
        }   
        

        void IUpdaterEntity.UpdateDay(Day day)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Days.Update(day);
                db.SaveChanges();
            }
        }

        

        void IUpdaterEntity.DeleteAppointments(List<Appointment> apps)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Appointments.RemoveRange(apps);
                db.SaveChanges();
            }
        }

        void IUpdaterEntity.DeleteDays(List<Day> days)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Days.RemoveRange(days);
                db.SaveChanges();
            }
        }

        void IUpdaterEntity.DeleteMonths(List<Month> months)
        {
            using (Connection db = new Connection(DataBaseName))
            {
                db.Months.RemoveRange(months);
                db.SaveChanges();
            }
        }

       
    }
}
