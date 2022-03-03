using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Masters.Database.Context.MySQL.Moq
{
    public class ConnectionMoq : Connection
    {
        public string DbName { get; set; }
        public ConnectionMoq(string DbName) : base(DbName)
        {
            this.DbName = DbName;
        }
        protected override void
            OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseMySql(
                $"server=localhost;user=root;password=botadmin;database={DbName};",
                new MySqlServerVersion(new Version(0,8,26))
                ); //sslca={ sslCa};
        }
    }
}
