using BotLibary.BotManager.Interfaces;
using BotLibary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Options;

namespace BotLibary.BotManager.HelperClasses
{
    class ConsoleSaver : IBotSaver
    {

        ChangesLog log { get; set; }
        string PathToDirectory { get; set; }
        string FileSystem = "\\FileSystem";
        public ConsoleSaver(ChangesLog log, string path)
        {
            this.log = log;
            this.PathToDirectory = path;
        }            
        public void Update(Bot SelectedBot)
        {
            if (FindPath(SelectedBot.BotName.Name, PathToDirectory))
            {
                using (StreamWriter file = File.CreateText(PathToDirectory + $"{FileSystem}\\{SelectedBot.BotName.Name}\\BotConfig.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, SelectedBot.botConfig);
                }
                using (StreamWriter file = File.CreateText(PathToDirectory + $"{FileSystem}\\{SelectedBot.BotName.Name}\\PersonalConfig.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, SelectedBot.personalConfig);
                }
                log?.Invoke($"Бот {SelectedBot.BotName.Name} обновлен");
            }
            else
            {
                log?.Invoke("Бот не найден");
            }
        }
        
        
        bool FindPath(string Name, string Path)
        {
            return Directory.Exists(Path + $"{FileSystem}\\{Name}");
        }

        public void UpdateAll(List<Bot> bots)
        {
            foreach (Bot bot in bots)
            {
                Update(bot);
            }
        }
    }
}
