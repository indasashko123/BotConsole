using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotLibary.TestingMock
{
    public class TestDateFunctionFAKE
    {
        public DateFunction df { get; set; }
        public TestDateFunctionFAKE()
        {
            df = new DateFunction();
        }
        public async Task CreateMonthsAsync(DateTime time)
        {
           await Task.Run(()=> df.CreateMonthsAsync(time));
        }
        public void CreatMonth(DateTime time)
        {
            df.CreateMonths(time);
        }
        public Month getCurrentMonth()
        {
            return df.CurrentMonth;
        }
        public Month getNextMonth()
        {
            return df.NextMonth;
        }
        public int getCurrentDay()
        {
            return df.CurrentDay;
        }
        public Dictionary<int,string> getNames()
        {
            return df.DayNames;
        }
        public void IncreementDay()
        {
            df.IncreementDay();
        }

    }
}
