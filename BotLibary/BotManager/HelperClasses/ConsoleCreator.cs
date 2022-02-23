using BotLibary.BotManager.Interfaces;
using BotLibary.Bots;
using BotLibary.Bots.Events;
using BotLibary.Bots.Interfaces;
using BotLibary.Bots.Masters;
using Newtonsoft.Json;
using Options;
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
        public void Create(BotConfig config, List<IBot> bots)
        {
            log?.Invoke($"Создан на пути {Path}");
            CreateFolderSystem(Path, config.Name);
            BotOptions options;
            switch (config.Direction)
            {
                case "Nails":
                    {
                        options = CreateMasterBot(config);
                        break;
                    }
                default:
                    {
                        options = null;
                        break;
                    }
            }           
            var newBot = BotDestinationCreater.Create(options);
            bots.Add(newBot);
            log?.Invoke($"Создан бот {newBot.GetName()}");
        }
        public void Create(BotName Name,  List<IBot> bots, string Token, string DataBaseName)
        {
            log?.Invoke($"Созданм на пути {Path}");
            CreateFolderSystem(Path, Name.Name);                       
            BotOptions options = CreateBotOptions(Path + $"{FileSystem}\\{Name.Name}", Name, Token, DataBaseName);
            AddPhotos(Path, Path + $"\\{FileSystem}\\{Name.Name}\\MyPhoto", "\\Hello.jpg");
            AddPhotos(Path, Path + $"\\{FileSystem}\\{Name.Name}\\MyPhoto", "\\Price.jpg");
            AddPhotos(Path, Path + $"\\{FileSystem}\\{Name.Name}\\MyWorks", "\\0.jpg");
            AddPhotos(Path, Path + $"\\{FileSystem}\\{Name.Name}\\MyWorks", "\\1.jpg");
            SerializeOptions(options);
            var newBot = new MasterBot(options);         
            // TODO: I was stop here!
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
        void SerializeOptions (BotOptions options)
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
        BotOptions CreateBotOptions(string Path, BotName name, string Token, string DataBaseName)
        {
            BotConfig botConfig = new BotConfig().CreatTemplate();
            PersonalConfig personalConfig = CreatePersonalConfig(Path);
            botConfig.Name = name.Name;
            botConfig.CustomerName = name.CustomerName;
            botConfig.Direction = name.Direction;
            botConfig.token = Token;
            botConfig.dataBaseName = DataBaseName;
            BotOptions options = new BotOptions(botConfig, personalConfig);              
            return options;
        }
        PersonalConfig CreatePersonalConfig(string Path)
        {
            PersonalConfig personalConfig = new PersonalConfig().CreateTemplate();
            personalConfig.Paths["GREETING"] = Path + personalConfig.Paths["GREETING"];
            personalConfig.Paths["PRICE"] = Path + personalConfig.Paths["PRICE"];
            personalConfig.Partfolio[0] = Path + @"\MyWorks\" + "0.jpg";
            personalConfig.Partfolio[1] = Path + @"\MyWorks\" + "1.jpg";
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
        }

        BotOptions CreateMasterBot(BotConfig config)
        {
            BotOptions options = new BotOptions(config, CreatePersonalConfig(Path));
            AddPhotos(Path, Path + $"\\{FileSystem}\\{config.Name}\\MyPhoto", "\\Hello.jpg");
            AddPhotos(Path, Path + $"\\{FileSystem}\\{config.Name}\\MyPhoto", "\\Price.jpg");
            AddPhotos(Path, Path + $"\\{FileSystem}\\{config.Name}\\MyWorks", "\\0.jpg");
            AddPhotos(Path, Path + $"\\{FileSystem}\\{config.Name}\\MyWorks", "\\1.jpg");
            SerializeOptions(options);
            return options;
        }
    }
}
