using BotLibary.BotManager.Interfaces;
using BotLibary.Events;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Options;
using BotLibary.TelegramBot;

namespace BotLibary.BotManager.HelperClasses
{
    /// <summary>
    /// Сущность для обновления состояния бота.
    /// Может обновить существующего бота
    /// или обновить  всех ботов сразу
    /// </summary>
    class ConsoleSaver : IBotSaver
    {

        public ChangesLog log;
        string PathToDirectory { get; set; }
        string FileSystem = "\\FileSystem";
        public ConsoleSaver(ChangesLog log, string path)
        {
            this.log = (string text)=> log?.Invoke(text);
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
        public void SetNewConfig(Bot SelectedBot)
        {
            BotConfig botConfig;
            string botConfigText = File.ReadAllText(PathToDirectory + FileSystem + SelectedBot.BotName.Name + $"\\BotConfig.json", Encoding.UTF8);
            botConfig = JsonConvert.DeserializeObject<BotConfig>(botConfigText);
            PersonalConfig personalConfig;
            string personalConfigText = File.ReadAllText(PathToDirectory + FileSystem + SelectedBot.BotName.Name+ $"\\PersonalConfig.json", Encoding.UTF8);
            personalConfig = JsonConvert.DeserializeObject<PersonalConfig>(personalConfigText);
            SelectedBot.GetNewConfig(new BotOptions(botConfig, personalConfig));
            log?.Invoke($"Бот {SelectedBot.BotName.Name} обновлен");
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
