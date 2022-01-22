using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary
{
    internal static class DateNames
    {
        internal static readonly Dictionary<int, string> Days = new Dictionary<int, string>()
            {
            {0 , "Воскресенье" },
            {1 , "Понедельник" },
            {2 , "Вторник" },
            {3 , "Среда" },
            {4 , "Четверг" },
            {5 , "Пятница" },
            {6 , "Суббота" },
            };
        internal static readonly Dictionary<int, string> Months = new Dictionary<int, string>()
            {
            {0 , "Январь" },
            {1 , "Февраль" },
            {2 , "Март" },
            {3 , "Апрель" },
            {4 , "Май" },
            {5 , "Июнь" },
            {6 , "Июль" },
            {0 , "Август" },
            {1 , "Сентябрь" },
            {2 , "Октябрь" },
            {3 , "Ноябрь" },
            {4 , "Декабрь" }        
            };
    }
}
