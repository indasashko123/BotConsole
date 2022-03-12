using BotLibary.BotManager.HelperClasses;
using System;

namespace BotLibary.BotManager
{
    public class ConsoleBotManager: BotManager
    {
        string Path { get; set; }
        public override void InitBotManager()
        {
            SystemMessage = ((string text) => { Console.WriteLine(text); });
            BotCreater = new ConsoleCreator(SystemMessage, Path);
            BotSaver = new ConsoleSaver(SystemMessage,Path);
            BotFinder = new ConsoleFinder(SystemMessage, Path);
        }
        public ConsoleBotManager(string Path)
        {
            this.Path = Path;
            InitBotManager();
        }


    }
}
