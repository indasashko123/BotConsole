using DataBase.Database;
using Microsoft.EntityFrameworkCore;
using DataBase.Database.Context.MySQL;
using DataBase.Database.Interface;

namespace UnutTesting
{
    public class TestContextFactory
    {
        public static MySqlDbContext CreateMySqlContext()
        {
            var options = new DbContextOptionsBuilder<MySqlDbContext>().
                UseInMemoryDatabase("Tst").Options;
            var context = new MySqlDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
        public static void Destroy(MySqlDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose(); 
        }
    }
}
