using BotLibary.BotManager;
using System;
using BotLibary;

namespace BotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleBotManager manager = new ConsoleBotManager(@"D:\BotManager");
            manager.FindAllBots();
            Console.WriteLine("Ввод команды\n" +
                   "new + name + customer name + direct + token\n" +
                   "select\n" +
                   "update\n" +
                   "showall\n" +
                   "start\n" +
                   "stop\n" +
                   "current\n");
            while (true)
            {
               
                string[] command = Console.ReadLine().Split(' ');
                if (command[0] == "q" || command[0] == "Quit")
                {
                    manager.UpdateAll();
                    break;
                }
                if (command[0].ToLower() == "new")
                {
                    if (command.Length == 6)
                    {
                        manager.CreateBot(new BotName(command[1], command[2], command[3]), command[4], command[5]);
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка в {command}\n");
                    }
                }
                if (command[0].ToLower()== "select")
                {
                    if (command.Length > 1)
                    {
                        manager.SelectBot(new BotName(command[1]));
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка в {command}\n");
                    }
                }
                if (command[0].ToLower() == "update")
                {
                    manager.BotUpdate();
                }
                if (command[0].ToLower() == "showall")
                {
                    manager.ShowBots();
                }
                if (command[0].ToLower() == "start")
                {
                    manager.BotStart();
                }
                if (command[0].ToLower() == "stop")
                {
                    manager.BotStop();
                }
                if (command[0].ToLower() == "current")
                {
                    manager.ShowCurrent();
                }
            }
           
        }
    }
}
