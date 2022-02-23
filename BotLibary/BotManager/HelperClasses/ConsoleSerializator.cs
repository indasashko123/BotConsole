using BotLibary.BotManager.Interfaces;
using BotLibary.Bots.Events;
using BotLibary.Bots.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.BotManager.HelperClasses
{
    class ConsoleSerializator:ISerializator
    {
        string Path { get; set; }
        string FileSystem = "\\FileSystem";
        ChangesLog log { get; set; }
        public ConsoleSerializator(string Path, ChangesLog log)
        {
            this.Path = Path;
            this.log = log;
        }

        public async void AddExample(EventArgsUpdate e)
        {
            string fileName =await CheckValidExampleNameAsync(e.bot.GetOptions().personalConfig.Partfolio, e.bot.GetName().Name);
            var file = await e.bot.GetFileAsync(e.fileId);
            string fullPath = $"{Path}+{FileSystem}\\{e.bot.GetName().Name}\\MyWorks\\{fileName}";
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await e.bot.DownLoadFileAsync(file.FilePath, stream);
            }
            SerializePersonalConfig(fullPath,e.bot);
        }





        string CheckValidExampleName(List<string> paths, string botName)
        {
            string path = "";
            bool IsUnique = false;
            int count = 0;
            while (!IsUnique)
            {
                IsUnique = true;
                count++;
                path = $"{Path}{FileSystem}\\{botName}\\MyWorks\\{count}.jpg";
                foreach (string controlePath in paths)
                {
                    if (controlePath == path)
                    {
                        IsUnique = false;
                    }
                }               
            }
            return $"{count}.jpg";
        }
        async Task<string> CheckValidExampleNameAsync(List<string> paths, string botName)
        {
            return await Task.Run(() => CheckValidExampleName(paths, botName) );            
        }
        void SerializePersonalConfig(string fullPath, IBot bot)
        {
            bot.GetOptions().personalConfig.Partfolio.Add(fullPath);
            using (StreamWriter configFile = File.CreateText($"{Path}{FileSystem}\\{bot.GetName().Name}" + "\\PersonalConfig.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(configFile, bot.GetOptions().personalConfig);
            }
        }
        async Task SerializePersonalConfigAsync(string fullPath, IBot bot)
        {
            await Task.Run(() => SerializePersonalConfig(fullPath,bot));
        }

    }
}
