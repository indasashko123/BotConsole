using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary
{
    public class BotName
    {
        internal string Name { get; set; }
        internal string CustomerName { get; set; }
        internal string Direction { get; set; }
        public BotName(string name, string customerName, string direction)
        {
            Name = name;
            CustomerName = customerName;
            Direction = direction;
        }
        public BotName(string name)
        {
            Name = name;
        }
    }
}
