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
using System.IO;
using Newtonsoft.Json;

namespace DataBase.Database.Context.MySQL
{
    class Connection : DbContext
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
            if (string.IsNullOrWhiteSpace(server))
            {
                connectionString = $"host={host};port={port};user={user};password={password};sslmode={sslMode};sslca={sslCa};";
            }
            else
            {
                connectionString = $"server = {server};user={user};password={password}; ";
            }
        }
        public Connection(string DbName)
        {
            GetOptions();
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
                $"{connectionString}database={DbName}",
                new MySqlServerVersion(new Version(version.v1, version.v2, version.v3))
                ); //sslca={ sslCa};
        }
    }
}
