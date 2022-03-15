using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Database.Interface
{
    public interface ICRUD
    {
        public void DeleteDb();

        /// <summary>
        /// Находим админа
        /// </summary>
        /// <returns></returns>
        internal User FindAdmin();
        internal Day GetFirstDay();
        internal Month GetFirstMonth();

        /// <summary>
        /// Находим пользователя по ChatId
        /// </summary>
        /// <param name="chatId">ChatId</param>
        /// <returns></returns>
        internal User FindUser(long chatId);

        /// <summary>
        /// Создаем нового пользователя и возвращаем его
        /// </summary>
        /// <param name="username">Никнайм</param>
        /// <param name="firstName">Имя</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="id">ChatId</param>
        /// <returns>Созданный пользователь</returns>
        internal User CreateNewUser(string username, string firstName, string lastName, long id);

        /// <summary>
        /// Найти пользователя по Id
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns></returns>
        internal User FindUser(int userId);

        /// <summary>
        /// Найти всех пользователей
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        internal List<User> FindUsers();

        /// <summary>
        /// Находим список месяцев
        /// </summary>
        /// <returns>Все месяцы</returns>
        internal List<Month> GetMonths();
        /// <summary>
        /// Находим список месяцев исключая месяцы с id из параметров
        /// </summary>
        /// <returns>месяцы не соответсвующие id</returns>
        internal List<Month> GetMonths(params int[] monthsId);

        /// <summary>
        /// Найти день по id
        /// </summary>
        /// <param name="dayId">id дня</param>
        /// <returns></returns>
        internal Day FindDay(int dayid);

        /// <summary>
        /// Найти дни в определенном месяце
        /// </summary>
        /// <param name="monthId">Месяц</param>
        /// <returns></returns>
        internal List<Day> FindDays(int monthId);

        /// <summary>
        /// Найти запись по id
        /// </summary>
        /// <param name="appId">id записи</param>
        /// <returns></returns>
        internal Appointment FindAppointment(int appId);

        /// <summary>
        /// Создаем новый Appoinment, добавляем его в бд, возвращаем его же
        /// </summary>
        /// <param name="dayId">День к которому привязан Appointment</param>
        /// <returns>Созданный Appointment</returns>
        internal Appointment AddAppointment(int dayId);

        /// <summary>
        /// Найти все записи на определеннный день
        /// </summary>
        /// <param name="dayId">День в котором нужно найти записи</param>
        /// <returns></returns>
        internal List<Appointment> FindAppointments(int dayId);

        /// <summary>
        /// Выбирает все подписи которые подтверждены если параметр true и не подтвержденные если false
        /// </summary>
        /// <param name="IsConfirm">параметр true если нужны подтвержденные и false если не подтвержденные</param>
        /// <returns></returns> 
        internal List<Appointment> FindConfirmAppointments(bool IsConfirm);

        /// <summary>
        /// Добавить месяц в базу данных
        /// </summary>
        /// <param name="month">Месяц для добавления в бд</param>
        /// <returns></returns>
        internal void AddMonth(Month month);

        /// <summary>
        /// Сохранить изменения в дне
        /// </summary>
        /// <param name="day">День который нужно обновить</param>
        /// <returns></returns>
        internal void UpdateDay(Day day);

        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="app">запись которую нужно добавить в БД</param>
        /// <returns></returns>
        internal void AddAppointment(Appointment app);

        /// <summary>
        /// Наполняет месяц новыми днями
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="currentDay">День с которого начинать создавать дни</param>
        /// <param name="month">Месяц в который нужно добавить дни</param>
        /// <returns></returns>
        internal void CreateDays(int currentDay, Month month, Dictionary<int, string> dayNames);

        /// <summary>
        /// Удаляет выбранную запись из БД
        /// </summary>
        /// <param name="app">Запись для удаления</param>
        /// <returns></returns>
        internal void DeleteApp(Appointment app);

        /// <summary>
        /// Обновить изменения в свойствах пользователя
        /// </summary>
        /// <param name="user">Пользователь для обновления</param>
        /// <returns></returns>
        internal void UpdateUser(User user);

        /// <summary>
        /// Обновить изменения в записи
        /// </summary>
        /// <param name="app">запись которую нужно обновить</param>
        /// <returns></returns>
        internal void UpdateApp(Appointment app);
        /// <summary>
        /// Удалить список записей
        /// </summary>
        /// <param name="apps">Список записей на удаление</param>
        internal void DeleteAppointments(List<Appointment> apps);

        /// <summary>
        /// Удаляет список дней
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        internal void DeleteDays(List<Day> days);

        /// <summary>
        /// Удаляет список месяцев
        /// </summary>
        /// <param name="months"></param>
        /// <returns></returns>
        internal void DeleteMonths(List<Month> month);
  
        #region ASYNC
        /// <summary>
        /// Находим админа
        /// </summary>
        /// <returns></returns>
        async Task<User> FindAdminAsync()
        {
            return await Task.Run(() => FindAdmin());
        }
        
        /// <summary>
        /// Находим пользователя по ChatId
        /// </summary>
        /// <param name="chatId">ChatId</param>
        /// <returns></returns>
         async Task<User> FindUserAsync(long chatId)
        {
            return await Task.Run(() => FindUser(chatId));
        }      
        
        /// <summary>
        /// Создаем нового пользователя и возвращаем его
        /// </summary>
        /// <param name="username">Никнайм</param>
        /// <param name="firstName">Имя</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="id">ChatId</param>
        /// <returns>Созданный пользователь</returns>
         async Task<User> CreateNewUserAsync(string username, string firstName, string lastName, long id)
        {
            return await Task.Run(() => CreateNewUser(username, firstName, lastName, id));
        }
        
        /// <summary>
        /// Находим список месяцев
        /// </summary>
        /// <returns>Все месяцы</returns>
        async Task<List<Month>> GetMonthsAsync()
        {
            return await Task.Run(() => GetMonths());
        }

        /// <summary>
        /// Найти дни в определенном месяце
        /// </summary>
        /// <param name="monthId">Месяц</param>
        /// <returns></returns>
         async Task<List<Day>> FindDaysAsync(int monthId)
        {
            return await Task.Run(() => FindDays(monthId));
        }

        /// <summary>
        /// Добавить месяц в базу данных
        /// </summary>
        /// <param name="month">Месяц для добавления в бд</param>
        /// <returns></returns>
         async Task AddMonthAsync( Month month)
        {
            await Task.Run(() => AddMonth( month));
        }

        /// <summary>
        /// Наполняет месяц новыми днями
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="currentDay">День с которого начинать создавать дни</param>
        /// <param name="month">Месяц в который нужно добавить дни</param>
        /// <returns></returns>
         async Task CreateDaysAsync(int currentDay, Month month, Dictionary<int ,string> dayNamse)
        {
            await Task.Run(() => CreateDays(currentDay, month,dayNamse));
        }
        
        /// <summary>
        /// Найти все записи на определеннный день
        /// </summary>
        /// <param name="dayId">День в котором нужно найти записи</param>
        /// <returns></returns>
         async Task<List<Appointment>> FindAppointmentsAsync(int dayId)
        {
            return await Task.Run(() => FindAppointments(dayId));
        }

        /// <summary>
        /// Найти запись по id
        /// </summary>
        /// <param name="appId">id записи</param>
        /// <returns></returns>
         async Task<Appointment> FindAppointmentAsync(int appId)
        {
            return await Task.Run(() => FindAppointment(appId));
        }

        /// <summary>
        /// Найти день по id
        /// </summary>
        /// <param name="dayId">id дня</param>
        /// <returns></returns>
         async Task<Day> FindDayAsync(int dayId)
        {
            return await Task.Run(() => FindDay(dayId));
        }

        /// <summary>
        /// Обновить изменения в записи
        /// </summary>
        /// <param name="app">запись которую нужно обновить</param>
        /// <returns></returns>
         async Task UpdateAppAsync(Appointment app)
        {
            await Task.Run(() => UpdateApp(app));
        }     

        /// <summary>
        /// Обновить изменения в свойствах пользователя
        /// </summary>
        /// <param name="user">Пользователь для обновления</param>
        /// <returns></returns>
         async Task UpdateUserAsync(User user)
        {
            await Task.Run(() => UpdateUser(user));
        }

        /// <summary>
        /// Создаем новый Appoinment, добавляем его в бд, возвращаем его же
        /// </summary>
        /// <param name="dayId">День к которому привязан Appointment</param>
        /// <returns>Созданный Appointment</returns>
         async Task<Appointment> AddAppointmentAsync(int dayId)
        {
            return await Task.Run(() => AddAppointment(dayId));
        }

        /// <summary>
        /// Удаляет выбранную запись из БД
        /// </summary>
        /// <param name="app">Запись для удаления</param>
        /// <returns></returns>
         async Task DeleteAppAsync(Appointment app)
        {
            await Task.Run(() => DeleteApp(app));
        }
      
        /// <summary>
        /// Найти пользователя по Id
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns></returns>
         async Task<User> FindUserAsync(int userId)
        {
            return await Task.Run(() => FindUser(userId));
        }

        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="app">запись которую нужно добавить в БД</param>
        /// <returns></returns>
         async Task AddAppointmentAsync(Appointment app)
        {
            await Task.Run(() => AddAppointment(app));
        }

        /// <summary>
        /// Найти всех пользователей
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        async Task<List<User>> FindUsersAsync ()
        {
            return await Task.Run(() => FindUsers());
        }

        /// <summary>
        /// Сохранить изменения в дне
        /// </summary>
        /// <param name="day">День который нужно обновить</param>
        /// <returns></returns>
        async Task UpdateDayAsync(Day day)
        {
            await Task.Run(() => UpdateDay(day));
        }
        async Task<Day> GetFirstDayAsync()
        {
            return await Task.Run(() => GetFirstDay()); 
        }
        /// <summary>
        /// Выбирает все подписи которые подтверждены если параметр true и не подтвержденные если false
        /// </summary>
        /// <param name="IsConfirm">параметр true если нужны подтвержденные и false если не подтвержденные</param>
        /// <returns></returns>       
        async Task<List<Appointment>> FindConfirmAppointmentsAsync(bool IsConfirm)
        {
            return await Task.Run(() => FindConfirmAppointments(IsConfirm));
        }
        /// <summary>
        /// Удалить список записей
        /// </summary>
        /// <param name="apps">Список записей на удаление</param>
        async Task DeleteAppointmentsAsync(List<Appointment> apps)
        {
            await Task.Run(() => DeleteAppointments(apps));
        }
        /// <summary>
        /// Находим список месяцев исключая месяцы с id из параметров
        /// </summary>
        /// <returns>месяцы не соответсвующие id</returns>
        async Task<List<Month>> GetMonthsAsync(params int[] monthsId)
        {
            return await Task.Run(() => GetMonths(monthsId));
        }
        /// <summary>
        /// Удаляет список дней
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        async Task DeleteDaysAsync(List<Day> days)
        {
            await Task.Run(() => DeleteDays(days));
        }
        /// <summary>
        /// Удаляет список месяцев
        /// </summary>
        /// <param name="months"></param>
        /// <returns></returns>
        async Task DeleteMonthsAsync(List<Month> months)
        {
            await Task.Run(() => DeleteMonths(months));
        }
        async Task<Month> GetFirstMonthAsync()
        {
            return await Task.Run(() => GetFirstMonth());
        }
        #endregion

    }
}
