
using System;
using DataBase.Models;

namespace BotLibary
{
    internal class DateFunction
    {     
        internal int CurrentDay { get; set; }
        internal Month CurrentMonth { get; set; }
        internal Month NextMonth { get; set; }
        internal DateFunction()
        {
            CurrentMonth = new Month();
            NextMonth = new Month(); 
        }
        internal void CreateMonths()
        {
            DateTime dateNow = DateTime.Now;           
            CurrentDay = dateNow.Day;
            CurrentMonth.MonthNumber = dateNow.Month;
            CurrentMonth.Year = dateNow.Year;
            CurrentMonth.DayCount = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.MonthNumber);
            CurrentMonth.Name = DateNames.Months[CurrentMonth.MonthNumber];
            CurrentMonth.IsCurrent = true;

            NextMonth = IncreementMonth(CurrentMonth);          
        }
        internal void IncreementDay()
        {
            CurrentDay++;
            if (CurrentMonth.DayCount < CurrentDay)
            {
                CurrentDay = 1;
                CurrentMonth = NextMonth;
                NextMonth = IncreementMonth(NextMonth);
            }
        }
        Month IncreementMonth(Month previousMonth)
        {
            Month newMonth = new Month();
            newMonth.IsCurrent = false;
            if (previousMonth.MonthNumber == 11)
            {
                newMonth.MonthNumber = 0;
                newMonth.Year = previousMonth.Year + 1;
            }
            else
            {
                newMonth.MonthNumber = previousMonth.MonthNumber + 1;
                newMonth.Year = previousMonth.Year;
            }
            newMonth.Name = DateNames.Months[newMonth.MonthNumber];
            newMonth.DayCount = DateTime.DaysInMonth(newMonth.Year, newMonth.MonthNumber);
            return newMonth;
        }
    }
}