using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }      
        public string AppointmentTime { get; set; }
        public int Day { get; set; }
        public bool IsConfirm { get; set; }
        public bool IsEmpty { get; set; }
        public int User { get; set; }
        public Appointment()
        {

        }
        public Appointment(string appTime,int day)
        {
            AppointmentTime = appTime;
            Day = day;
            IsConfirm = false;
            IsEmpty = true;           
            User = 0;
        }
        public Appointment(int day)
        {
            Day = day;
            IsConfirm = false;
            IsEmpty = true;
            User = 0;
        }
    }
}
