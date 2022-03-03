
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataBase.Models;

namespace BotLibary.Bots.Masters
{
    public  class DateFunction
    {      
        public static readonly Dictionary<int, string> MonthNames = new Dictionary<int, string>()
            {
            {1 , "Январь🌃" },
            {2 , "Февраль🏙" },
            {3 , "Март🌄" },
            {4 , "Апрель🌆" },
            {5 , "Май🌇" },
            {6 , "Июнь🌇" },
            {7 , "Июль🌄" },
            {8 , "Август🌅" },
            {9 , "Сентябрь🌇" },
            {10 , "Октябрь🏙" },
            {11 , "Ноябрь🌃" },
            {12, "Декабрь🏙" }
            };
        public static readonly Dictionary<int, string> DayNames = new Dictionary<int, string>()
            {
            {0 , "Воскресенье" },
            {1 , "Понедельник" },
            {2 , "Вторник" },
            {3 , "Среда" },
            {4 , "Четверг" },
            {5 , "Пятница" },
            {6 , "Суббота" },
            };
        internal protected int CurrentDay { get; set; }
        public Month CurrentMonth { get; set; }
        public Month NextMonth { get; set; }   
        private protected bool IsCreated { get; set; }
        public DateFunction()
        {
            CurrentMonth = new Month();
            NextMonth = new Month();           
            IsCreated = false;
        }
        public virtual void CreateMonths(DateTime time)
        {
            IsCreated = true;
            DateTime dateNow = time;           
            CurrentDay = dateNow.Day;
            CurrentMonth.MonthNumber = dateNow.Month;
            CurrentMonth.Year = dateNow.Year;
            CurrentMonth.DayCount = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.MonthNumber);
            CurrentMonth.Name = MonthNames[CurrentMonth.MonthNumber];

            NextMonth = IncreementMonth(CurrentMonth);          
        }
        public void IncreementDay()
        {
            if (IsCreated)
            {
                CurrentDay++;
                if (CurrentMonth.DayCount < CurrentDay)
                {
                    CurrentDay = 1;
                    CurrentMonth = new Month()
                    {
                        DayCount = NextMonth.DayCount,
                        MonthNumber = NextMonth.MonthNumber,
                        Name = NextMonth.Name,
                        Year = NextMonth.Year
                    };
                    NextMonth = IncreementMonth(NextMonth);
                }
            }
            
        }
        protected internal async Task CreateMonthsAsync(DateTime time)
        {
            await Task.Run(()=>CreateMonths(time));
        }
        protected internal async Task IncreementDayAsync()
        {
            await Task.Run(() => IncreementDay());
        }
        protected virtual Month IncreementMonth(Month previousMonth)
        {
            Month newMonth = new Month();
            if (previousMonth.MonthNumber == 12)
            {
                newMonth.MonthNumber = 1;
                newMonth.Year = previousMonth.Year + 1;
            }
            else
            {
                newMonth.MonthNumber = previousMonth.MonthNumber + 1;
                newMonth.Year = previousMonth.Year;
            }
            newMonth.Name = MonthNames[newMonth.MonthNumber];
            newMonth.DayCount = DateTime.DaysInMonth(newMonth.Year, newMonth.MonthNumber);
            return newMonth;
        }
     
    }
}