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
        public string Description { get; set; }
        public string AppointmentTime { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public bool IsConfirm { get; set; }
        public bool IsEmpty { get; set; }
        public bool IsHighPrice { get; set; }
        public decimal Price { get; set; }
        public int User { get; set; }
    }
}
