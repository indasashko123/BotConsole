using BotLibary.BotManager.Interfaces;
using BotLibary.Events;
using BotLibary.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BotLibary.BotManager.HelperClasses.Serializator
{
    /// <summary>
    /// Serializator делает -> добаляет и удаляет фото из хранилища
    /// </summary>
    class ConsoleImageManager : IImageManager
    {
        string Path { get; set; }
        string FileSystem = "\\FileSystem";
        ChangesLog log { get; set; }
        public ConsoleImageManager(string Path, ChangesLog log)
        {
            this.Path = Path;
            this.log = log;
        }

        public async void AddExample(EventArgsUpdate e)
        {
            string fileName =await CheckValidExampleNameAsync(e.bot.personalConfig.Partfolio, e.bot.BotName.Name);
            var file = await e.bot.bot.GetFileAsync(e.fileId);
            string fullPath = $"{Path}+{FileSystem}\\{e.bot.BotName.Name}\\MyWorks\\{fileName}";
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await e.bot.bot.DownloadFileAsync(file.FilePath, stream);
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
        void SerializePersonalConfig(string fullPath, Bot bot)
        {
            bot.personalConfig.Partfolio.Add(fullPath);
            using (StreamWriter configFile = File.CreateText($"{Path}{FileSystem}\\{bot.BotName.Name}" + "\\PersonalConfig.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(configFile, bot.personalConfig);
            }
        }
        async Task SerializePersonalConfigAsync(string fullPath, Bot bot)
        {
            await Task.Run(() => SerializePersonalConfig(fullPath,bot));
        }

    }
}
