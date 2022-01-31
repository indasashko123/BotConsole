using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public TestDateFunctionFAKE(DateTime time)
        {
            this.df = new OpenDateFunctionTestFAKE(time);
        }
        public async Task CreateMonthsAsync()
        {
           await Task.Run(()=> df.CreateMonthsAsync());
        }
        public void CreatMonth()
        {
            df.CreateMonths();
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
