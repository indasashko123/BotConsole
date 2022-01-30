using BotLibary.TestingMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace UnutTesting
{
    [TestClass]
    public class DateFuncTest
    {
        [TestMethod]
        public void CheckTimeDF1000_1_1()
        {
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(new DateTime(1000, 1, 1));
            df.CreatMonth();
            string answer = df.getCurrentMonth().Year.ToString();
            Assert.AreEqual("1000", answer);
        }
        [TestMethod]
        public void IncreementDay1000_1_1to1000_1_1()
        {
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(new DateTime(1000, 1, 1));
            df.CreatMonth();
            df.IncreementDay();
            int curDay = df.getCurrentDay();
            Assert.AreEqual(2, 2);
        }
        [TestMethod]
        public void DayCount1000_1_1to31()
        {
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(new DateTime(1000, 1, 1));
            df.CreatMonth();
            df.IncreementDay();
            int maxDay = df.getCurrentMonth().DayCount;
            Assert.AreEqual(31, 31);
        }
        [TestMethod]
        public void IncreementMonth1000_1_1()
        {
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(new DateTime(1000, 1, 31));
            df.CreatMonth();
            int monthNumber = df.getNextMonth().MonthNumber;
            df.IncreementDay();
            Assert.AreEqual(2, df.getCurrentMonth().MonthNumber);
        }
        [TestMethod]
        public void IncreementMonth1000_12_31()
        {
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(new DateTime(1000, 12, 31));
            df.CreatMonth();
            int monthNumber = df.getNextMonth().MonthNumber;
            df.IncreementDay();
            string ans = df.getCurrentMonth().MonthNumber +""+df.getCurrentMonth().Year;
            Assert.AreEqual("11001", ans);
        }
        [TestMethod]
        public void DayOfWeek1000_1_1()
        {
            var day = new DateTime(1000, 1, 1);
            int dayOfWeek = (int)day.DayOfWeek;
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(day);

            string ans = df.getNames()[dayOfWeek];

            Assert.AreEqual("Среда", ans);
        }
        [TestMethod]
        public void DayOfWeek1000_1_2()
        {
            var day = new DateTime(1000, 1, 2);
            int dayOfWeek = (int)day.DayOfWeek;
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(day);

            string ans = df.getNames()[dayOfWeek];

            Assert.AreEqual("Четверг", ans);
        }
        [TestMethod]
        public void DayOfWeek1000_1_3()
        {
            var day = new DateTime(1000, 1, 3);
            int dayOfWeek = (int)day.DayOfWeek;
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(day);

            string ans = df.getNames()[dayOfWeek];

            Assert.AreEqual("Пятница", ans);
        }
        [TestMethod]
        public void DayOfWeek1000_1_4()
        {
            var day = new DateTime(1000, 1, 4);
            int dayOfWeek = (int)day.DayOfWeek;
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(day);

            string ans = df.getNames()[dayOfWeek];

            Assert.AreEqual("Суббота", ans);
        }
        [TestMethod]
        public void DayOfWeek1000_1_5()
        {
            var day = new DateTime(1000, 1, 5);
            int dayOfWeek = (int)day.DayOfWeek;
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(day);

            string ans = df.getNames()[dayOfWeek];

            Assert.AreEqual("Воскресенье", ans);
        }
        [TestMethod]
        public void DayOfWeek1000_1_6()
        {
            var day = new DateTime(1000, 1, 6);
            int dayOfWeek = (int)day.DayOfWeek;
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(day);

            string ans = df.getNames()[dayOfWeek];

            Assert.AreEqual("Понедельник", ans);
        }
        [TestMethod]
        public void DayOfWeek1000_1_7()
        {
            var day = new DateTime(1000, 1, 7);
            int dayOfWeek = (int)day.DayOfWeek;
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(day);

            string ans = df.getNames()[dayOfWeek];

            Assert.AreEqual("Вторник", ans);
        }
        [TestMethod]
        public void DayOfWeek1000_1_8()
        {
            var day = new DateTime(1000, 1, 8);
            int dayOfWeek = (int)day.DayOfWeek;
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(day);

            string ans = df.getNames()[dayOfWeek];

            Assert.AreEqual("Среда", ans);
        }
        [TestMethod]
        public void IncreementMonthName1000_1_31()
        {
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(new DateTime(1000, 1, 31));
            df.CreatMonth();            
            df.IncreementDay();

            Assert.AreEqual("Февраль", df.getCurrentMonth().Name);
        }
        [TestMethod]
        public void IncreementMonthName1000_11_30()
        {
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(new DateTime(1000, 11, 30));
            df.CreatMonth();
            df.IncreementDay();

            Assert.AreEqual("Декабрь", df.getCurrentMonth().Name);
        }
        [TestMethod]
        public void IncreementMonth2_1000_12_31()
        {
            TestDateFunctionFAKE df = new TestDateFunctionFAKE(new DateTime(1000, 12, 31));
            df.CreatMonth();
            int monthNumber = df.getNextMonth().MonthNumber;
            df.IncreementDay();
            string ans = df.getCurrentMonth().MonthNumber + "" + df.getCurrentMonth().Year+""+df.getCurrentDay();
            Assert.AreEqual("110011", ans);
        }
    }
}
