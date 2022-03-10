using BotLibary.Events;

namespace BotLibary
{
    public class ImageManager
    {
        public UpdateEvent AddWorkExample { get; set; }
        public UpdateEvent DeleteWorkExample { get; set; }
        public UpdateEvent ChangePresentImage { get; set; }
        public UpdateEvent ChangePrice { get; set; }
    }
}
