
namespace BotLibary.Bots
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
        public override string ToString()
        {
            return $"Имя бота - {Name}, Имя пользователя - {CustomerName}, Направление бота - {Direction}";
        }
    }
}
