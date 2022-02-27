using BotLibary.BotManager.Interfaces;
using BotLibary.Bots;
using BotLibary.Bots.Events;
using BotLibary.Bots.Interfaces;
using BotLibary.Bots.Masters;
using Newtonsoft.Json;
using Options;
using Options.MasterBotConfig;
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
        public void Create(string Data, List<IBot> bots)
        {
            log?.Invoke($"Создан на пути {Path}");
            string[] ConsoleData = Data.Split(' ');
            BotConfig botConfig = null;
            if (ConsoleData.Length >= 6)
            {
                botConfig = new BotConfig(ConsoleData[3], ConsoleData[5], ConsoleData[4], ConsoleData[0], ConsoleData[1], ConsoleData[2]);
            }                
            IBot newBot;
            switch (botConfig.Direction)
            {
                case "Nails":
                    {
                    CreateMasterFolderSystem(botConfig.Name, botConfig.Direction);
                    PersonalMasterBotConfig personalConfig = new PersonalMasterBotConfig().CreateMasterBotTemplate();
                    MasterBotConfig masterBotConfig = new MasterBotConfig(ConsoleData[6], botConfig);
                    var options = new BotOptions<MasterBotConfig, PersonalMasterBotConfig>(masterBotConfig, personalConfig);
                    CreateMasterBotPersonalConfig(personalConfig);
                    CreateMasterBotPhotos(options.botConfig);
                    SerializeOptions<MasterBotConfig,PersonalMasterBotConfig>(options);
                    newBot = BotDestinationCreater.Create(options, options.botConfig.Direction);                                        
                    break;
                    }
                default:
                    {
                        newBot = null;
                        break;
                    }
            }
            bots.Add(newBot);
            log?.Invoke(newBot == null?$"Создан бот {newBot.GetName()}": "Бот не создан");
        }      
       
        void SerializeOptions<T,K>(BotOptions<T,K> options)
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





        #region Methods for masterBot's photos 
        void CreateMasterFolderSystem(string name, string direction)
        {
            Directory.CreateDirectory(Path + FileSystem);
            Directory.CreateDirectory(Path + $"{FileSystem}\\{name}");
            Directory.CreateDirectory(Path + $"{FileSystem}\\{name}\\MyPhoto");
            Directory.CreateDirectory(Path + $"{FileSystem}\\{name}\\MyWorks");
            File.Create(Path + FileSystem + $"\\{name}" + "\\Dir.txt");
            File.WriteAllText(Path + FileSystem + $"\\{name}" + "\\Dir.txt", direction);
        }
        void CreateMasterBotPersonalConfig(PersonalMasterBotConfig personalConfig)
        {           
            personalConfig.Paths["GREETING"] = Path + personalConfig.Paths["GREETING"];
            personalConfig.Paths["PRICE"] = Path + personalConfig.Paths["PRICE"];
            personalConfig.Partfolio[0] = Path + @"\MyWorks\" + "0.jpg";
            personalConfig.Partfolio[1] = Path + @"\MyWorks\" + "1.jpg";
        }
        void CreateMasterBotPhotos(BotConfig config)
        {
            AddPhotos(Path + $"\\{FileSystem}\\{config.Name}\\MyPhoto", "\\Hello.jpg");
            AddPhotos(Path + $"\\{FileSystem}\\{config.Name}\\MyPhoto", "\\Price.jpg");
            AddPhotos(Path + $"\\{FileSystem}\\{config.Name}\\MyWorks", "\\0.jpg");
            AddPhotos(Path + $"\\{FileSystem}\\{config.Name}\\MyWorks", "\\1.jpg");
        }        
        /// <summary>
        /// Add Photos from templates on the disk
        /// </summary>
        /// <param name="newPath">путь куда будут копироваться фото</param>
        /// <param name="fileName">Имя файла который будет копирован</param>
        void AddPhotos(string newPath, string fileName)
        {
            string pathTemplate =Path + $"\\Template\\{fileName}";
            log?.Invoke("Путь к шаблону -  " + pathTemplate + "\n Путь к новому файлу -" +
                $"{newPath+fileName}");
            FileInfo fileInf = new FileInfo(pathTemplate);
            if (fileInf.Exists)
            {
                fileInf.CopyTo(newPath+fileName, true);
            } 
            else
            {
                log?.Invoke("Ошибка при копировании фото из шаблона\n");
            }
        }
        #endregion

    }
}
