using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class Day
    {
        public int DayId { get; set; }
        /// <summary>
        /// Дата. Номер дня по счету в месяце.
        /// </summary>
        public int Date { get; set; }
        /// <summary>
        /// Айди месяца.
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// Рабочий день?
        /// </summary>
        public bool IsWorkDay { get; set; }
        /// <summary>
        /// Высокая цена?
        /// </summary>
        public bool IsHighPriceDay { get; set; }
        /// <summary>
        /// Год.
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Номер месяцав году.
        /// </summary>
        public int MonthNumber { get; set; }
        /// <summary>
        /// Название дня недели.
        /// </summary>
        public string DayOfWeek { get; set; }

        public Day()
        {

        }
        public Day(Month month, int date, string dayOfWeek)
        {
            Date = date;
            Month = month.MonthId;
            IsWorkDay = true;
            IsHighPriceDay = false;
            Year = month.Year;
            MonthNumber = month.MonthNumber;
            DayOfWeek = dayOfWeek;
        }
    }
}
