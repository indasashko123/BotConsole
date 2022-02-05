using BotLibary.BotManager.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.BotManager
{
    public class ConsoleBotManager: BotManager
    {
        string Path { get; set; }
        public override void InitBotManager()
        {
            this.SystemMessage = ((string text) => { Console.WriteLine(text); });
            this.BotCreater = new ConsoleCreator(SystemMessage, Path);
            this.BotSaver = new ConsoleSaver(SystemMessage,Path);
            this.BotFinder = new ConsoleFinder(SystemMessage, Path);
        }
        public ConsoleBotManager(string Path)
        {
            this.Path = Path;
            InitBotManager();
        }


    }
}
