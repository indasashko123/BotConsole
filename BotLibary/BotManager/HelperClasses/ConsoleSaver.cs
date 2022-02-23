using BotLibary.BotManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Options;
using BotLibary.Bots.Events;
using BotLibary.Bots.Interfaces;

namespace BotLibary.BotManager.HelperClasses
{
    /// <summary>
    /// Сущность для обновления состояния бота.
    /// Может обновить существующего бота
    /// или обновить  всех ботов сразу
    /// </summary>
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
        public void Update(IBot SelectedBot)
        {
            if (FindPath(SelectedBot.GetName().Name, PathToDirectory))
            {
                using (StreamWriter file = File.CreateText(PathToDirectory + $"{FileSystem}\\{SelectedBot.GetName().Name}\\BotConfig.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, SelectedBot.GetOptions().botConfig);
                }
                using (StreamWriter file = File.CreateText(PathToDirectory + $"{FileSystem}\\{SelectedBot.GetName().Name}\\PersonalConfig.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, SelectedBot.GetOptions().personalConfig);
                }
                log?.Invoke($"Бот {SelectedBot.GetName().Name} обновлен");
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

        public void UpdateAll(List<IBot> bots)
        {
            foreach (var bot in bots)
            {
                Update(bot);
            }
        }
    }
}
