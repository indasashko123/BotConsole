using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.TestingMock
{
    internal class OpenDateFunctionTestFAKE : DateFunction
    {
        DateTime TestTime { get; set; }
        public OpenDateFunctionTestFAKE(DateTime time)
        {
            TestTime = time;            
        }
        internal override void CreateMonths(DateTime Time)
        {
            DateTime dateNow = TestTime;
            CurrentDay = dateNow.Day;
            CurrentMonth.MonthNumber = dateNow.Month;
            CurrentMonth.Year = dateNow.Year;
            CurrentMonth.DayCount = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.MonthNumber);
            CurrentMonth.Name = MonthNames[CurrentMonth.MonthNumber];
            NextMonth = IncreementMonth(CurrentMonth);
        }
        
    }
}
