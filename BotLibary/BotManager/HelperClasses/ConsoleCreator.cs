using BotLibary.BotManager.Interfaces;
using BotLibary.Events;
using Newtonsoft.Json;
using Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace BotLibary.BotManager.HelperClasses
{
    class ConsoleCreator : IBotCreater
    {
        string Path { get; set; }
        string FileSystem = "\\FileSystem";
        ChangesLog log { get; set; }
        
        public ConsoleCreator(ChangesLog log, string Path)
        {
            this.log = log;
            this.Path = Path;
        }
        public void Create(string botConfig, List<Bot> bots)
        {
            string[] data = botConfig.Split(' ');
            log?.Invoke($"Созданм на пути {Path}");
            BotConfig config = null;
            switch (data[2])
            {
                case "Nails":
                    {
                        config = new BotConfig()
                        {
                            Name = data[0],
                            CustomerName = data[1],
                            Direction = data[2],
                            token = data[3],
                            dataBaseName=data[4],
                            password=data[5],
                        };
                        var appTime = data[6].Split('|');
                        foreach (var time in appTime)
                        {
                            config.appointmentStandartTimes.Add(time);
                            config.appointmentStandartCount++;
                        }
                        break;
                    }
            }            
            CreateFolderSystem(Path, config.Name);
            string FullPath = $"{Path}\\{FileSystem}\\{config.Name}";
            BotOptions options = new BotOptions(config, CreatePersonalConfig(Path, config.Name));
            AddPhotos(Path, FullPath+"\\MyPhoto", "\\Hello.jpg");
            AddPhotos(Path, FullPath+"\\MyPhoto", "\\Price.jpg");
            AddPhotos(Path, FullPath+"\\MyWorks", "\\0.jpg");
            AddPhotos(Path, FullPath+"\\MyWorks", "\\1.jpg");
            SerializeOptions(options, FullPath);
            Bot newBot = new Bot(options);
            bots.Add(newBot);
            log?.Invoke($"Создан бот {newBot.BotName.Name}");
        }  
        void CreateFolderSystem(string Path, string name)
        {
            Directory.CreateDirectory(Path + FileSystem);
            Directory.CreateDirectory(Path + $"{FileSystem}\\{name}");
            Directory.CreateDirectory(Path + $"{FileSystem}\\{name}\\MyPhoto");
            Directory.CreateDirectory(Path + $"{FileSystem}\\{name}\\MyWorks");
        }
              
        PersonalConfig CreatePersonalConfig(string Path, string name)
        {
            PersonalConfig personalConfig = new PersonalConfig().CreateTemplate();
            personalConfig.Paths["GREETING"] = Path + $"\\{FileSystem}\\{name}" + personalConfig.Paths["GREETING"];
            personalConfig.Paths["PRICE"] = Path + $"\\{FileSystem}\\{name}"+ personalConfig.Paths["PRICE"];
            personalConfig.Partfolio[0] = Path+$"\\{FileSystem}\\{name}" + @"\MyWorks\" + "0.jpg";
            personalConfig.Partfolio[1] = Path +$"\\{FileSystem}\\{name}"+@"\MyWorks\" + "1.jpg";
            return personalConfig;
        }
        void AddPhotos(string path, string newPath, string fileName)
        {
            string pathTemplate =path + $"\\Template\\{fileName}";
            log?.Invoke("Путь к шаблону -  " + pathTemplate + "\n Путь к новому файлу -" +
                $"{newPath+fileName}");

            FileInfo fileInf = new FileInfo(pathTemplate);
            if (fileInf.Exists)
            {
                fileInf.CopyTo(newPath+fileName, true);
            }
            else
            {
                Console.WriteLine("Изображение не найдено");
            }
        }
        void SerializeOptions(BotOptions options, string Path)
        {
            using (StreamWriter file = File.CreateText(Path + "\\BotConfig.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, options.botConfig);
            }
            using (StreamWriter file = File.CreateText(Path + "\\PersonalConfig.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, options.personalConfig);
            }
        }

    }
}
