using BotLibary.BotManager;
using System;
using Options.MasterBotConfig;
using BotLibary.Bots;
using Options;

namespace BotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Путь  - \n");

            ConsoleBotManager manager = new ConsoleBotManager(Console.ReadLine());
            manager.FindAllBots();
            Console.WriteLine("Ввод команды\n" +
                   "newMaster + name + customerName + direct + token + dbName + password +Times('|')\n" +
                   "select + name\n" +
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
                if (command[0].ToLower() == "newMaster")
                {
                    manager.CreateBot(CreateCommad(command));
                }
                if (command[0].ToLower() == "select")
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
            string CreateCommad(string[] Data)
            {
                string command = "";
                foreach (var data in Data)
                {
                    command += data + " ";
                }
                return command;
            }
        }
    }
}
