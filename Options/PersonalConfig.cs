using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Options
{
    public class PersonalConfig
    {
        public Dictionary<string, string> Messages { get; set; }
        public Dictionary<string, string> Buttons { get; set; }
        public Dictionary<string, string> AdminButtons { get; set; }
        public Dictionary<string, string> Paths { get; set; }
        public List<string> Partfolio { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public Dictionary<string,string> MediaLink { get; set; }

        public PersonalConfig()
        {
            Messages = new Dictionary<string, string>();
            Buttons = new Dictionary<string, string>();
            AdminButtons = new Dictionary<string, string>();
            MediaLink = new Dictionary<string, string>();
            Paths = new Dictionary<string, string>();
            Partfolio = new List<string>();
            Latitude = 0;
            Longitude = 0;

        }
        public PersonalConfig(string Path)
        {
            PersonalConfig config;
            string text = File.ReadAllText(Path, Encoding.UTF8);
            config = JsonConvert.DeserializeObject<PersonalConfig>(text);
            Messages = config.Messages;
            Buttons = config.Buttons;
            AdminButtons = config.AdminButtons;
            MediaLink = config.MediaLink;
            Paths = config.Paths;
            Partfolio = config.Partfolio;
            Latitude = config.Latitude;
            Longitude = config.Longitude;
        }
        public PersonalConfig CreateTemplate()
        {
            PersonalConfig config = new PersonalConfig()
            {
                Messages = new Dictionary<string, string>()
                {
                    ["USERGREETING"] = "Приветствие пользователя",
                    ["FEEDBACK"] = "Сообщение при ссылке на отзывы",
                    ["ABOUT"] = "Обо мне",
                    ["LOCATION"] = "Текст для описания местанахождения",
                    ["UNKNOWN"] = "Текст для неизвестной команды",
                    ["ADMINGREETING"] = "Приветствие админа",
                    ["INSTAGRAM"] = "Сообщение при ссылке на инст",
                    ["CHOSEDAY"] = "Выбирите день",


                    ["DAYOFF"] = "Не рабочий день",
                    ["HIGHPRICEDAY"] = "Высокая цена",
                    ["REGULARDAY"] = "Обычный день",
                    ["ADMINSELECTMONTH"] = "Выбор месяца для админа",
                    ["CHOSEAPP"] = "Пользователь выбрал день для записи",
                    ["APPEMPTY"] = "Если запись не занята",
                    ["APPNOTEMPTY"] = "Если запись занята",
                    ["SORRYTAKEN"] = "Извиняемся если запись уже занята. Это ошибка",

                    ["WAITCINFIRM"] = "Ожидате подтверждения админа",
                    ["NEWAPP"] = "Человек записался на прием",
                    ["YOURAPPCONFIRM"] = "Ваша запись подтверждена",
                    ["YOURAPPNOTCONFIRM"] = "Ваша запись не подтверждена",
                    ["ADMINCHOISEDAY"] = "Выбирите день для админа",
                    ["WRITEAPPTIME"] = "Напишите время приема или время приема",
                    ["ISWEEKENDFORUSER"] = "Сообщение пользователю что день не рабочий",
                    ["WEEKENDWARNING"] = "Предупреждение. Продолжить делать день выходным? имеет не свободные записи",

                    ["USERAPPISCANCEL"] = "Запись пользователя отменена",
                    ["ADMINAPPISCANCEL"] = "Сообщение для админа, о том что запись отменена",
                    ["ADMINMAILING"] = "Инструкция для отправки рассылки",
                    ["NOTIFICATION"] = "Уведомление пользователя о записи на завтра"
                },
                Buttons = new Dictionary<string, string>() {
                    ["APPOINTMENT"] = "Записаться",
                    ["PRICE"] = "Прайс",
                    ["FEEDBACK"] = "Отзывы",
                    ["MYWORKS"] = "Мои работы",
                    ["ABOUT"] = "Обо мне",
                    ["LOCATION"] = "Как добраться",
                    ["LINK"] = "Ссылки"
                },
                AdminButtons = new Dictionary<string, string>() {
                    ["ADDAPP"] = "Добавить запись",
                    ["CONFIRM"] = "Подтвердить запись",
                    ["NOTCONFIRM"] = "Отменить запись",
                    ["DELAPP"] = "Удалить запись",
                    ["ALLUSERS"] = "Все пользователи",
                    ["MAKEWEEKEND"] = "Сделать выходной",
                    ["LOOKCONFIRM"] = "Подтвержденные записи",
                    ["LOOKNOTCONFIRM"] = "Не подствержденные записи"
                },
                Paths = new Dictionary<string, string>() {
                    ["GREETING"] = @"\MyPhoto\Hello.jpg",
                    ["PRICE"] = @"\MyPhoto\Price.jpg."
                },
                MediaLink = new Dictionary<string, string>() {
                    ["INSTAGRAM"] = "instagram.com",
                    ["TELEGRAM"] = "t.me",
                    ["PHONE"] = "8-800-800-800",
                    ["YOUTUBE"] = "youtube.com",
                    ["VK"] = "vk.com"
                },
                Partfolio = new List<string>()
                {
                    "1.jpg",
                    "2.jpg"
                },
                Latitude = 1.23f,
                Longitude = 3.21f
            };
            return config;
        }
    }
}