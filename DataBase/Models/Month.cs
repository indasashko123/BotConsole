using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class Month
    {
        public int MonthId { get; set; }
        public bool IsCurrent { get; set; }
        public string Name { get; set; }
        public int User { get; set; }
        public int DayCount { get; set; }
        public int MonthNumber { get; set; }
        public int Year { get; set; }
    }
}
