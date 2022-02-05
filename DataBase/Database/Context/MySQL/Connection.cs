using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector.Authentication;
using MySqlConnector.Logging;
using MySqlConnector;
using DataBase.Models;

namespace DataBase.Database.Context.MySQL
{
    class Connection : DbContext
    {
        public Action<string> SystemMessage;
        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Month> Months { get; set; }
        private string DbName { get; set; }
        public Connection(string DbName)
        {
            this.DbName = DbName;          
        }
        public void CreateDataBase()
        {
            Database.EnsureCreated();
        }
        protected override void
            OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseMySql(
                $"server=localhost;user=root;password=botadmin;database={DbName};",
                new MySqlServerVersion(new Version(8, 0, 26))
                );
        }
    }
}
