using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class Appointment
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        public int AppointmentId { get; set; }      
        /// <summary>
        /// Время записи
        /// </summary>
        public string AppointmentTime { get; set; }
        /// <summary>
        /// День на который ведется запись. ID дня
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// Подтвержденная запись?
        /// </summary>
        public bool IsConfirm { get; set; }
        /// <summary>
        ///  Пустая запись?
        /// </summary>
        public bool IsEmpty { get; set; }
        /// <summary>
        /// Пользователь записаный на эту запись. 0 если запись пустая
        /// </summary>
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
            AppointmentTime = "";
            Day = day;
            IsConfirm = false;
            IsEmpty = true;
            User = 0;
        }
    }
}
