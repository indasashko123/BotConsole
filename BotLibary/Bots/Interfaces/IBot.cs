using BotLibary.Bots.Events;
using Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace BotLibary.Bots.Interfaces
{
    public interface IBot
    {
        Task<Telegram.Bot.Types.File> GetFileAsync(string field);
        Task DownLoadFileAsync(string filePath, Stream destination);
        public ChangesLog Log { get; set; }
        public AdminMessage AdminLog { get; set; }
        BotName GetName();
        void  BotStart();
        void BotStop();
    }
}
