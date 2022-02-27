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
        public bool IsAdmin { get; set; }
        public long ChatId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public User()
        {

        }
        public User(string username, string firstName, string lastName, long chat)
        {
            this.Username = username;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.ChatId = chat;
        }
        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName} { this.Username}";
        }
    }
}
