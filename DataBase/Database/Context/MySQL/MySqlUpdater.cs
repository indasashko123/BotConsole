using DataBase.Database.Interface;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;
namespace DataBase.Database.Context.MySQL
{
    public class MySqlUpdater : IUpdater
    {
        string DataBaseName;
        public MySqlUpdater(string DataBaseName)
        {
            this.DataBaseName = DataBaseName;
        }
        protected virtual DbContext Connect()
        {
            return new MySqlDbContext(DataBaseName);
        }
        void IUpdater.UpdateApp(Appointment app)
        {
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                db.Appointments.Update(app);
                db.SaveChanges();
            }
        }
        void IUpdater.UpdateUser(User user)
        {
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                db.Users.Update(user);
                db.SaveChanges();
            }
        }
        void IUpdater.UpdateDay(Day day)
        {
            using (MySqlDbContext db = Connect() as MySqlDbContext)
            {
                db.Days.Update(day);
                db.SaveChanges();
            }
        }
    }
}
