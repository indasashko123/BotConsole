using DataBase.Database.Interface;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Database.MySQL.Context
{
    public class Context : ICRUD
    {
        public User CreateNewUser(string username, string firstName, string lastName, long id)
        {
            User newUser = new User(username, firstName, lastName, id);
        }

        public User FindAdmin()
        {
            throw new NotImplementedException();
        }

        public User FindUser(long chatId)
        {
            throw new NotImplementedException();
        }
    }
}
