using BotLibary.Events;
using BotLibary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary
{
    public class ImageManager : IImageManager
    {
        public UpdateEvent AddWorkExample { get; set; }
        public UpdateEvent DeleteWorkExample { get; set; }
        public UpdateEvent ChangePresentImage { get; set; }
        public UpdateEvent ChangePrice { get; set; }
    }
}
