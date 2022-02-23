using BotLibary.BotManager.Interfaces;
using BotLibary.Bots;
using BotLibary.Bots.Events;
using BotLibary.Bots.Interfaces;
using BotLibary.Bots.Masters;
using Newtonsoft.Json;
using Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.BotManager.HelperClasses
{
    class ConsoleFinder : IBotFinder
    {
        string PathToDirectory { get; set; }
        string FileSystem = "\\FileSystem";
        ChangesLog log { get; set; }
        public ConsoleFinder(ChangesLog log, string path)
        {
            PathToDirectory = path;
            this.log = log;
        }
        public IBot FindByName(BotName Name, List<IBot> bots)
        {           
            var bot =  bots.Where(bot => bot.GetName().Name == Name.Name).FirstOrDefault();
            if (bot == null)
            {
                log?.Invoke("Бот не найден");
            }
            else
            {
                log?.Invoke($"Выбран бот {bot.GetName().CustomerName}");
                bot.Log += ((string text) => { log?.Invoke(text); });
                bot.Log?.Invoke("\n\n   Боту незначен log");               
            }
            return bot;
        }

        public void ShowBots(List<IBot> bots)
        {
            foreach (var bot in bots)
            {
                string answer = $"{bot.GetName()}\n";
                log?.Invoke(answer);
            }
        }
        public void ShowCurrent(IBot bot)
        {
            if (bot == null)
            {
                log?.Invoke("Бот не выбран");
                return;
            }
            string answer = $" {bot.GetName()}\n";
            log?.Invoke(answer);
        }
        public List<IBot> FindAllBots()
        {
            List<IBot> bots = new List<IBot>();
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
                // TODO: make deffirent bots
                var newBot = new MasterBot(new BotOptions(botConfig, personalConfig));
                log?.Invoke($"Найден бот {newBot.BotName.Name}");
                bots.Add(newBot);
            }
            log?.Invoke($"Найденно {bots.Count}");
            return bots;
        }
    }
}
