using DataBase.Database.Interface;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataBase.Database.Context.MySQL
{
    public class MySqlErraiser : IErraiser
    {
        string DataBaseName;
        protected virtual DbContext Connect()
        {
            return new MySqlDbContext(DataBaseName);
        }
        public MySqlErraiser(string DataBaseName)
        {
            this.DataBaseName = DataBaseName;
        }
        public void DeleteDataBase()
        {
            Connect().Database.EnsureDeleted();
        }
        void IErraiser.DeleteApp(Appointment app)
        {
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                db.Appointments.Remove(app);
                db.SaveChanges();
            }
        }
        void IErraiser.DeleteAppointments(List<Appointment> apps)
        {
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                db.Appointments.RemoveRange(apps);
                db.SaveChanges();
            }
        }
        void IErraiser.DeleteDays(List<Day> days)
        {
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                db.Days.RemoveRange(days);
                db.SaveChanges();
            }
        }
        void IErraiser.DeleteMonths(List<Month> months)
        {
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                db.Months.RemoveRange(months);
                db.SaveChanges();
            }
        }

    }
}
