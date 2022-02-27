using BotLibary.Bots.Masters;
using System;

namespace BotLibary.TestingMock.Masters.DateFunc
{
    internal class OpenDateFunctionTestFAKE : DateFunction
    {
        DateTime TestTime { get; set; }
        public OpenDateFunctionTestFAKE(DateTime time)
        {
            TestTime = time;            
        }
        internal override void CreateMonths()
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
