
using DataBase.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase.Database.Interface
{
    public interface ICreater
    {
        public void CreateDataBase();
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
        /// Создаем новый Appoinment, добавляем его в бд, возвращаем его же
        /// </summary>
        /// <param name="dayId">День к которому привязан Appointment</param>
        /// <returns>Созданный Appointment</returns>
        internal Appointment AddAppointment(int dayId);
        /// <summary>
        /// Добавить месяц в базу данных
        /// </summary>
        /// <param name="month">Месяц для добавления в бд</param>
        /// <returns></returns>
        internal void AddMonth(Month month);
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


        #region Async
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
        /// Добавить месяц в базу данных
        /// </summary>
        /// <param name="month">Месяц для добавления в бд</param>
        /// <returns></returns>
        async Task AddMonthAsync(Month month)
        {
            await Task.Run(() => AddMonth(month));
        }
        /// <summary>
        /// Наполняет месяц новыми днями
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="currentDay">День с которого начинать создавать дни</param>
        /// <param name="month">Месяц в который нужно добавить дни</param>
        /// <returns></returns>
        async Task CreateDaysAsync(int currentDay, Month month, Dictionary<int, string> dayNamse)
        {
            await Task.Run(() => CreateDays(currentDay, month, dayNamse));
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
        /// Добавить запись в БД
        /// </summary>
        /// <param name="app">запись которую нужно добавить в БД</param>
        /// <returns></returns>
        async Task AddAppointmentAsync(Appointment app)
        {
            await Task.Run(() => AddAppointment(app));
        }
        #endregion
    }
}
