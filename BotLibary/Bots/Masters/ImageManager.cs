using BotLibary.BotManager.Interfaces;
using BotLibary.Bots.Events;
using System;


namespace BotLibary.Bots.Masters
{
    public class ImageManager : IImageManager
    {
        public UpdateEvent AddWorkExample { get; set; }
        public UpdateEvent DeleteWorkExample { get; set; }
        public UpdateEvent ChangePresentImage { get; set; }
        public UpdateEvent ChangePrice { get; set; }
    }
}
