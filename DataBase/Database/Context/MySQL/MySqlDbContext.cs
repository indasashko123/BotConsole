using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using DataBase.Models;
using System.IO;
using Newtonsoft.Json;

namespace DataBase.Database.Context.MySQL
{
    public class MySqlDbContext : DbContext
    {
        public Action<string> SystemMessage;
        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Month> Months { get; set; }
        private string server { get; set; }
        private string user { get; set; }
        private string password { get; set; }
        private string DbName { get; set; }
        private string port { get; set; }
        private string host { get; set; }
        private string sslCa { get; set; }
        private string sslMode { get; set; }
        private string connectionString { get; set; }
        private bool IsLocal { get; set; }
        private MySQLVersion version { get; set; }
        private void GetOptions()
        {
            string path = Environment.CurrentDirectory;
            string file = File.ReadAllText(path + $"\\BaseConfig.json", Encoding.UTF8);
            var config = JsonConvert.DeserializeObject<BaseConfig>(file);
            this.server = config.server;
            this.user = config.user;
            this.password = config.password;
            this.port = config.port;
            this.host = config.host;
            this.sslCa = config.sslCa;
            this.sslMode = config.sslMode;
            this.version = config.version;
            this.IsLocal = config.IsLocal;
            if (!IsLocal)
            {
                connectionString = $"host={host};port={port};user={user};password={password};sslmode={sslMode};sslca={sslCa};";
            }
            else
            {
                connectionString = $"server = {server};user={user};password={password}; ";
            }
        }
        public MySqlDbContext(string DbName)
        {
           // GetOptions();
            this.DbName = DbName;
            Database.EnsureCreated();
        }
        public MySqlDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void
            OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseMySql(
                $"server=localhost;user=root;password=botadmin;database={DbName}",
                     new MySqlServerVersion(new Version(8, 0, 26))
                 // $"{connectionString}database={DbName}",
                //  new MySqlServerVersion(new Version(version.v1, version.v2, version.v3))
                );  //sslca={ sslCa};
        }
    }
}
