using DataBase.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase.Masters.Database.Interface
{
    public interface IUpdaterEntity
    {
        internal void DeleteDB();
        /// <summary>
        /// Находим админа
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Сохранить изменения в дне
        /// </summary>
        /// <param name="day">День который нужно обновить</param>
        /// <returns></returns>
        internal void UpdateDay(Day day);            
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
        /// Удаляет выбранную запись из БД
        /// </summary>
        /// <param name="app">Запись для удаления</param>
        /// <returns></returns>
         async Task DeleteAppAsync(Appointment app)
        {
            await Task.Run(() => DeleteApp(app));
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
        /// <summary>
        /// Удалить список записей
        /// </summary>
        /// <param name="apps">Список записей на удаление</param>
        async Task DeleteAppointmentsAsync(List<Appointment> apps)
        {
            await Task.Run(() => DeleteAppointments(apps));
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
        #endregion

    }
}
