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
        //string TemplateBotConfig = "D:\\BotManager\\Template\\BotConfig.json";
        //string TemplatePersonalConfig = "D:\\BotManager\\Template\\PersonalConfig.json";
        string FileSystem = "\\FileSystem";
        ChangesLog SystemMessage { get; set; }
        public ConsoleCreator(ChangesLog log)
        {
            SystemMessage = log;
        }
        
        public void Create(BotName Name, string Path , List<Bot> bots)
        {           
            CreateFolderSystem(Path);           
            Directory.CreateDirectory(Path + $"{FileSystem}\\{Name.Name}");
            Directory.CreateDirectory(Path + $"{FileSystem}\\{Name.Name}\\MyPhoto");
            Directory.CreateDirectory(Path + $"{FileSystem}\\{Name.Name}\\MyWorks");
            BotOptions options = CreateBotOptions(Path + $"{FileSystem}\\{Name.Name}");
            Bot newBot = new Bot(options, Name);
            bots.Add(newBot);
            SystemMessage?.Invoke("Ost");
        }
        bool CheckFileSystem(string Path)
        {
            SystemMessage($"Ищем папку {Path}");
            return Directory.Exists(Path);            
        }
        void CreateFolderSystem(string Path)
        {
            Directory.CreateDirectory(Path + FileSystem);
        }
        BotOptions CreateBotOptions(string Path)
        {
            BotConfig botConfig = new BotConfig().CreatTemplate();
            PersonalConfig personalConfig = new PersonalConfig().CreateTemplate();
            personalConfig.Paths["GREETING"] = Path + personalConfig.Paths["GREETING"];
            personalConfig.Paths["PRICE"] = Path + personalConfig.Paths["PRICE"];
            personalConfig.Partfolio[0] = Path + @"\MyWorks\" + "0.jpg";
            personalConfig.Partfolio[1] = Path + @"\MyWorks\" + "1.jpg";
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
        bool CheckBotExist(string path)
        {
            SystemMessage($"Проверяем существование папки {path}");
            return Directory.Exists(path);
        }
    }
}
