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
        public int Date { get; set; }
        public int Month { get; set; }
        public bool IsWorkDay { get; set; }
        public bool IsHighPriceDay { get; set; }
        public int Year { get; set; }
        public int MonthNumber { get; set; }
        public string DayOfWeek { get; set; }
    }
}
