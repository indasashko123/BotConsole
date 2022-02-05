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
        public void Create(BotName Name,  List<Bot> bots, string Token)
        {
            log?.Invoke($"Созданм на пути {Path}");
            CreateFolderSystem(Path);           
            Directory.CreateDirectory(Path + $"{FileSystem}\\{Name.Name}");
            Directory.CreateDirectory(Path + $"{FileSystem}\\{Name.Name}\\MyPhoto");
            Directory.CreateDirectory(Path + $"{FileSystem}\\{Name.Name}\\MyWorks");
            BotOptions options = CreateBotOptions(Path + $"{FileSystem}\\{Name.Name}", Name);
            AddPhotos(Path, Path + $"\\{FileSystem}\\{Name.Name}\\MyPhoto", "\\Hello.jpg");
            AddPhotos(Path, Path + $"\\{FileSystem}\\{Name.Name}\\MyPhoto", "\\Price.jpg");
            AddPhotos(Path, Path + $"\\{FileSystem}\\{Name.Name}\\MyWorks", "\\0.jpg");
            AddPhotos(Path, Path + $"\\{FileSystem}\\{Name.Name}\\MyWorks", "\\1.jpg");
            Bot newBot = new Bot(options);            
            bots.Add(newBot);
            log?.Invoke($"Создан бот {newBot.BotName.Name}");
        }
       
        void CreateFolderSystem(string Path)
        {
            Directory.CreateDirectory(Path + FileSystem);
        }
        BotOptions CreateBotOptions(string Path, BotName name)
        {
            BotConfig botConfig = new BotConfig().CreatTemplate();
            PersonalConfig personalConfig = new PersonalConfig().CreateTemplate();
            personalConfig.Paths["GREETING"] = Path + personalConfig.Paths["GREETING"];
            personalConfig.Paths["PRICE"] = Path + personalConfig.Paths["PRICE"];
            personalConfig.Partfolio[0] = Path + @"\MyWorks\" + "0.jpg";
            personalConfig.Partfolio[1] = Path + @"\MyWorks\" + "1.jpg";
            botConfig.Name = name.Name;
            botConfig.CustomerName = name.CustomerName;
            botConfig.Direction = name.Direction;
            BotOptions options = new BotOptions(botConfig, personalConfig);
            using (StreamWriter file = File.CreateText(Path+"\\BotConfig.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, botConfig);
            }
            using (StreamWriter file = File.CreateText(Path+"\\PersonalConfig.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, personalConfig);
            }
            return options;
        }
        bool CheckFileSystem(string Path)
        {
            log?.Invoke($"Ищем папку {Path}");
            return Directory.Exists(Path);
        }
        bool CheckBotExist(string path)
        {
            log?.Invoke($"Проверяем существование папки {path}");
            return Directory.Exists(path);
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
        }
    }
}
