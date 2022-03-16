
using DataBase.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase.Database.Interface
{
    public interface IErraiser
    {
        public void DeleteDataBase();
        /// <summary>
        /// Удаляет выбранную запись из БД
        /// </summary>
        /// <param name="app">Запись для удаления</param>
        /// <returns></returns>
        internal void DeleteApp(Appointment app);
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
        #region Async
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
        /// <summary>
        /// Удаляет выбранную запись из БД
        /// </summary>
        /// <param name="app">Запись для удаления</param>
        /// <returns></returns>
        async Task DeleteAppAsync(Appointment app)
        {
            await Task.Run(() => DeleteApp(app));
        }
        #endregion
    }
}
