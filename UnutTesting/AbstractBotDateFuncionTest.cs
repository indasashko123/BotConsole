using System;
using System.Collections.Generic;
using System.Threading;
using BotLibary;
using BotLibary.TestingMock;
using DataBase.Database;
using DataBase.Database.Context.MySQL;
using DataBase.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnutTesting
{
    [TestClass]
    public class AbstractBotDateFuncionTest
    {
        [TestMethod]
        public void TestDf()
        {
            AbstractBotMockDateFunctionTest bot = new AbstractBotMockDateFunctionTest();
            bot.SetContext(new DataBaseConnector(new SQLContext("testDF")));
            bot.CreateDateFunction(new DateTime(1000, 1, 1));
            var df = bot.GetDF();
            Assert.AreEqual(1, df.getCurrentDay());
        }
        [TestMethod]
        public void TestUpdate()
        {
            AbstractBotMockDateFunctionTest bot = new AbstractBotMockDateFunctionTest();
            bot.SetContext(new DataBaseConnector(new SQLContext("testDF")));
            bot.CreateDateFunction(new DateTime(1000, 1, 1));
            var df = bot.GetDF();
            bot.UpdateDate(new DateTime(1000,1,2));
            Assert.AreEqual(2, df.getCurrentDay());
        }
        [TestMethod]
        public void TestUpdate2()
        {
            AbstractBotMockDateFunctionTest bot = new AbstractBotMockDateFunctionTest();
            bot.SetContext(new DataBaseConnector(new SQLContext("testDF")));
            bot.CreateDateFunction(new DateTime(1000, 1, 1));
            var df = bot.GetDF();
            bot.UpdateDate(new DateTime(1000, 1,3));
            Thread.Sleep(1000);
            bot.UpdateDate(new DateTime(1000, 1, 3));
            Assert.AreEqual(3, df.getCurrentDay());
        }
        [TestMethod]
        public void TestUpdateWithContext()
        {
            AbstractBotMockDateFunctionTest bot = new AbstractBotMockDateFunctionTest();
            bot.SetContext(new DataBaseConnector(new SQLContext("testDF")));
            bot.CreateDateFunction(new DateTime(1000, 1, 27));
            List<Day> days = new List<Day>();
                 bot.GetDays(days);
            Thread.Sleep(4000);
            Assert.AreEqual(4, days.Count);
        }
    }
}
