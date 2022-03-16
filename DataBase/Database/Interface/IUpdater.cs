
using DataBase.Models;
using System.Threading.Tasks;

namespace DataBase.Database.Interface
{
    public interface IUpdater
    {
        /// <summary>
        /// Сохранить изменения в дне
        /// </summary>
        /// <param name="day">День который нужно обновить</param>
        /// <returns></returns>
        internal void UpdateDay(Day day);
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



        #region Async
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
        /// Сохранить изменения в дне
        /// </summary>
        /// <param name="day">День который нужно обновить</param>
        /// <returns></returns>
        async Task UpdateDayAsync(Day day)
        {
            await Task.Run(() => UpdateDay(day));
        }
        #endregion
    }
}
