
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
        internal void CreateMonths(int UserId)
        {
            DateTime dateNow = DateTime.Now;           
            CurrentDay = dateNow.Day;
            CurrentMonth.MonthNumber = dateNow.Month;
            CurrentMonth.Year = dateNow.Year;
            CurrentMonth.DayCount = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.MonthNumber);
            CurrentMonth.Name = DateNames.Months[CurrentMonth.MonthNumber];
            CurrentMonth.IsCurrent = true;
            CurrentMonth.User = UserId;

            NextMonth.User = UserId;
            NextMonth.IsCurrent = false;
            if (CurrentMonth.MonthNumber == 11)
            {
                NextMonth.MonthNumber = 0;
                NextMonth.Year = CurrentMonth.Year + 1;
            }
            else
            {
                NextMonth.MonthNumber = CurrentMonth.MonthNumber + 1;
                NextMonth.Year = CurrentMonth.Year;
            }
            NextMonth.Name = DateNames.Months[NextMonth.MonthNumber];
            NextMonth.DayCount = DateTime.DaysInMonth(NextMonth.Year, NextMonth.MonthNumber);
        }
    }
}