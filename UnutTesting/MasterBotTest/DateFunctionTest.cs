using System;
using BotLibary.Bots.Masters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnutTesting.MasterBotTest
{
    [TestClass]
    public class DateFunctionTest
    {
        [TestMethod]
        public void CreateTest1990_01_12()
        {
            DateFunction df = new DateFunction();
            df.CreateMonths(new DateTime(1990,01,12));
            int answer = df.CurrentMonth.MonthNumber;
            Assert.AreEqual(1, answer);
        }
        [TestMethod]
        public void IncreementDay1990_01_31()
        {
            DateFunction df = new DateFunction();
            df.CreateMonths(new DateTime(1990, 01, 31));
            df.IncreementDay();
            int answer = df.CurrentMonth.MonthNumber;
            Assert.AreEqual(2, answer);
        }
        [TestMethod]
        public void IncreementDay1990_12_31()
        {
            DateFunction df = new DateFunction();
            df.CreateMonths(new DateTime(1990, 12, 31));
            df.IncreementDay();
            int answer = df.CurrentMonth.Year;
            Assert.AreEqual(1991, answer);
        }
    }
}
