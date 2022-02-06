using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLibary.Events;

namespace BotLibary.Interfaces
{
    public interface IImageManager
    {
        public UpdateEvent AddWorkExample { get; set; }
        public UpdateEvent DeleteWorkExample { get; set; }
        public UpdateEvent ChangePresentImage { get; set; }
        public UpdateEvent ChangePrice { get; set; }
    }
}
