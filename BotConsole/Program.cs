using BotLibary.BotManager;
using System;
using BotLibary;
using Options;

namespace BotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Путь  - \n");

            ConsoleBotManager manager = new ConsoleBotManager(Console.ReadLine());
            string data = "";
            manager.FindAllBots();
            Console.WriteLine("Ввод команды\n" +
                "Создать новую конфигурацию бота \n" +
                   "config + name customerName + direct + token + dbName + password +Times('|')\n"+
                "Создать с имеющийся конфигурацией\n" + 
                   "new" +
                "Выбрать бота по имени\n " +
                   "select\n" +
                   "update\n" +
                "Показать всех ботов\n" +
                   "showall\n" +
                   "start\n" +
                   "stop\n" +
                "Показать бота, который выбран\n" +
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
                        data = command[1]+" "+command[2]+" "+command[3]+" "+command[4]+" "+command[5]+" "+command[6]+" "+command[7];
                        Console.WriteLine("СОзданы настройки бота");
                    }             
                    else
                    {
                        Console.WriteLine("Ошибка");
                    }
                }
                if (command[0].ToLower() == "new")
                {                               
                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            manager.CreateBot(data);
                        }
                        else
                        Console.WriteLine($"Ошибка в {command}\n");
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
