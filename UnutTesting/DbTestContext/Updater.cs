using DataBase.Database.Context.MySQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnutTesting.DbTestContext
{
    class Updater : MySqlUpdater
    {
        string DataBaseName;
        public Updater(string DataBaseName) : base(DataBaseName)
        {
        }
        protected override DbContext Connect()
        {
            return TestContextFactory.CreateMySqlContext();
        }
    }
}
