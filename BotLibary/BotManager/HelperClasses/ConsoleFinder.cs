using BotLibary.BotManager.Interfaces;
using BotLibary.Events;
using BotLibary.TelegramBot;
using Newtonsoft.Json;
using Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BotLibary.BotManager.HelperClasses
{
    class ConsoleFinder : IBotFinder
    {
        string PathToDirectory { get; set; }
        string FileSystem = "\\FileSystem";
        public event ChangesLog log;
        public ConsoleFinder(ChangesLog log, string path)
        {
            PathToDirectory = path;
            this.log = (string text) => log?.Invoke(text);
        }
        public Bot FindByName(BotName Name, List<Bot> bots)
        {           
            Bot bot =  bots.Where(bot => bot.BotName.Name == Name.Name).FirstOrDefault();
            if (bot == null)
            {
                log?.Invoke("Бот не найден");
            }
            else
            {
                log?.Invoke($"Выбран бот {bot.BotName.CustomerName}");           
            }
            return bot;
        }

        public void ShowBots(List<Bot> bots)
        {
            if (bots == null || bots.Count ==0)
            {
                Console.WriteLine("Ботов пока нет");
            }
            foreach (Bot bot in bots)
            {
                string answer = $" bot name is {bot.BotName.Name}, customer is {bot.BotName.CustomerName}, direction is  {bot.BotName.Direction}\n";
                log?.Invoke(answer);
            }
        }
        public void ShowCurrent(Bot bot)
        {
            if (bot == null)
            {
                log?.Invoke("Бот не выбран");
                return;
            }
            string answer = $" bot name is {bot.BotName.Name}, customer is {bot.BotName.CustomerName}, direction is  {bot.BotName.Direction}\n";
            log?.Invoke(answer);
        }
        public List<Bot> FindAllBots()
        {
            List<Bot> bots = new List<Bot>();
            string[] directories = Directory.GetDirectories(PathToDirectory + FileSystem);
            if (directories == null || directories.Length == 0)
            {
                log?.Invoke("Ботов не найдено");
                return bots;
            }
            foreach (var path in directories)
            {
                BotConfig botConfig;
                string botConfigText = File.ReadAllText(path + $"\\BotConfig.json", Encoding.UTF8);
                botConfig = JsonConvert.DeserializeObject<BotConfig>(botConfigText);
                PersonalConfig personalConfig;
                string personalConfigText = File.ReadAllText(path + $"\\PersonalConfig.json", Encoding.UTF8);
                personalConfig = JsonConvert.DeserializeObject<PersonalConfig>(personalConfigText);
                Bot newBot = new Bot(new BotOptions(botConfig, personalConfig), log);
                log?.Invoke($"Найден бот {newBot.BotName.Name}");
                bots.Add(newBot);
            }
            log?.Invoke($"Найденно {bots.Count}");
            return bots;
        }
    }
}
