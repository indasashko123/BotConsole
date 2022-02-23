﻿using BotLibary.BotManager;
using System;
using Options;
using BotLibary.Bots;

namespace BotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Путь  - \n");

            ConsoleBotManager manager = new ConsoleBotManager(Console.ReadLine());
            BotConfig botConfig = new BotConfig();

            manager.FindAllBots();
            Console.WriteLine("Ввод команды\n" +
                   "config + name customerName + direct + token + dbName + password +Times('|')\n"+
                   "new + name + customer name + direct + token + dbName\n" +
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
                if (command[0].ToLower()== "config")
                {
                    if (command.Length == 8)
                    {
                        botConfig = new BotConfig()
                        {
                            Name = command[1],
                            CustomerName = command[2],
                            Direction = command[3],
                            token = command[4],
                            dataBaseName = command[5],
                            password = command[6]
                        };
                        foreach (var time in command[7].Split('|'))
                        {
                            botConfig.appointmentStandartTimes.Add(time);
                            botConfig.appointmentStandartCount++;
                        }
                        Console.WriteLine("СОзданы настройки бота");
                    }                 
                }
                if (command[0].ToLower() == "new")
                {
                    if (command.Length == 6)
                    {
                        manager.CreateBot(new BotName(command[1], command[2], command[3]), command[4], command[5]);
                    }
                    else
                    {
                        if (command.Length == 2 && command[2] == "config")
                        {
                            manager.CreateBot(botConfig);
                        }
                        else
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
