using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class Month
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        public int MonthId { get; set; }
        /// <summary>
        /// Название месяца
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Количество дней в месяце
        /// </summary>
        public int DayCount { get; set; }
        /// <summary>
        /// Номер месяца по календарю
        /// </summary>
        public int MonthNumber { get; set; }
        /// <summary>
        /// Номер года по календарю
        /// </summary>
        public int Year { get; set; }
        public Month()
        {
        }
        public Month(int dayCount, int monthNumber, int year, string name)
        {

            Name = name;           
            DayCount = dayCount;
            MonthNumber = monthNumber;
            Year = year;
        }
    }
}
