﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace BotLibary.Interfaces
{
    internal interface IBot
    {
        void  BotStart();
        void BotStop();
        
    }
}