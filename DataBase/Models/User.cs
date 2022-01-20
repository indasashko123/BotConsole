using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class User
    {
        public int UserId { get; set; }
        public bool isAdmin { get; set; }
        public long chatId { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public long id { get; set; }

        public User(string username, string firstName, string lastName, long id)
        {
            this.username = username;
            this.firstName = firstName;
            this.lastName = lastName;
            this.id = id;
        }
    }
}
