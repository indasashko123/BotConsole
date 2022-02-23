using BotLibary.Bots.Events;

namespace BotLibary.BotManager.Interfaces
{
    public interface IImageManager
    {
        public UpdateEvent AddWorkExample { get; set; }
        public UpdateEvent DeleteWorkExample { get; set; }
        public UpdateEvent ChangePresentImage { get; set; }
        public UpdateEvent ChangePrice { get; set; }
    }
}
