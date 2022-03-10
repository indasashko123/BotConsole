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
        public Dictionary<string, string> Options { get; set; }
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
                    ["USERGREETING"] = "ТАТЬЯНА | МАНИКЮР САРАТОВ\n" +
                                       "Товар / услуга\n" +
                                       "Влюблена в своё дело 🤍\n" +
                                       "• Аппаратный / комби маникюр💅\n" +
                                       "• Стильные,\n" +
                                       "минималистичные дизайны👍\n"+
                                       "• Стерильный инструмент🛠",

                    ["FEEDBACK"] = "Все отзывы со мною вы можете прочитать в моем инстаграме💥",

                    ["ABOUT"] = "ТАТЬЯНА | МАНИКЮР САРАТОВ\n" +
                                       "Товар / услуга\n" +
                                       "Влюблена в своё дело 🤍\n" +
                                       "• Аппаратный / комби маникюр💅\n" +
                                       "• Стильные,\n" +
                                       "минималистичные дизайны👍\n" +
                                       "• Стерильный инструмент🛠",

                    ["LOCATION"] = "Мы находимся:📍\n"+
                                   "Адрес: 129323, г.Саратов, проспект Ordzhonikidze, д. 24🏢\n"+
                                   "Часы работы:Только по записи.📖\n" +
                                   "Телефон: 8(986) 986 - 60 - 60📱\n\n" +

                                   "Общественным транспортом:🚎\n\n" +

                                   "Автобус 850 - 6 остановок\n" +
                                   "До остановки \"5-й автобусный парк\"\n\n" +

                                   "Далее пешком 300 метров.\n" +
                                   "Если стоять спиной к автобусу на лево 50 метров, в первый переулок на право 250 метров мимо ЖК \"BIGTIME\".\n\n" +

                                   "Троллейбусы 43\n" +
                                   "Автобус 48, 294, 800\n" +
                                   "Маршрутки 386М, 391М, 446М\n" +
                                   "до  остановки «Мневники д.4» от Метро \"Хорошево\" - 1 остановка\n\n" +

                                   "Гостинница \"Moscow holiday hotel\".\n" +
                                   "Наш вход с торца гостиницы справа.\n",

                    ["UNKNOWN"] = "Неизвестная команда",
                    ["INSTAGRAM"] = "Мой инстаграмм",
                    ["CHOSEDAY"] = "Для записи на  прием, вам необходимо выбрать дату:📆\n"+
                                   "🔴-Выходной\n" +
                                   "🟡-Цена к прайсу х1.5\n" +
                                   "🟢-Рабочий день",
                    ["DAYOFF"] = "🔴",
                    ["HIGHPRICEDAY"] = "🟡",
                    ["REGULARDAY"] = "🟢",
                    ["CHOSEAPP"] = "Выбирите время записи\n" +
                                   "🔴-Запись занята\n"+
                                   "🟢-Запись свободна",
                    ["APPEMPTY"] = "🟢",
                    ["APPNOTEMPTY"] = "🔴",
                    ["SORRYTAKEN"] = "Извиняемся. Запись уже занята.",
                    ["WAITCONFIRM"] = "Ожидайте подтверждения админа",
                    ["YOURAPPCONFIRM"] = "Ваша запись подтверждена",
                    ["YOURAPPNOTCONFIRM"] = "Ваша запись не подтверждена",
                    ["ISWEEKENDFORUSER"] = "Этот день - выходной",
                    ["USERAPPISCANCEL"] = "Запись пользователя отменена",
                    ["NOTIFICATION"] = "У Вас записано на завтра!",
                    ["ADMINOPTIONS"] = "Меню настройки бота"                  
                },
                Options = new Dictionary<string, string>()
                {
                    ["ADDEXAMPLE"] = "Добавить в примеры",
                    ["DELETEEXAMPLE"] = "Удалить из примеров",
                    ["GREETING"] = "Сменнить приветственное фото",
                    ["PRICE"] = "Сменить прайс"
                },
                Buttons = new Dictionary<string, string>() {
                    ["APPOINTMENT"] = "Запись на маникюр🖍",
                    ["PRICE"] = "Прайс на работы💰",
                    ["FEEDBACK"] = "Отзывы клиентов📘",
                    ["MYWORKS"] = "Мои работы💅",
                    ["ABOUT"] = "Обо мне🙎‍♀️",
                    ["LOCATION"] = "Как добраться📍",
                    ["LINK"] = "Связаться со мною📞"
                },
                AdminButtons = new Dictionary<string, string>() {
                    ["ADDAPP"] = "Добавить запись",
                    ["CONFIRM"] = "Подтвердить запись",
                    ["NOTCONFIRM"] = "Отменить запись",
                    ["DELAPP"] = "Удалить запись",
                    ["ALLUSERS"] = "Все пользователи",
                    ["MAKEWEEKEND"] = "Сделать выходной",
                    ["LOOKCONFIRM"] = "Подтвержденные записи",
                    ["LOOKNOTCONFIRM"] = "Не подствержденные записи", 
                    ["MAILING"] = "Отправить рассылку",
                    ["OPTIONS"] = "настройка🛠"
                },
                Paths = new Dictionary<string, string>() {
                    ["GREETING"] = @"\MyPhoto\Hello.jpg",
                    ["PRICE"] = @"\MyPhoto\Price.jpg."
                },
                MediaLink = new Dictionary<string, string>() {
                    ["INSTAGRAM"] = "https://www.instagram.com/volkova_nailartist/",
                    ["Telegramm🟢"] = "https://www.instagram.com/volkova_nailartist/",
                    ["PHONE"] = "8-986-9866-060",
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