using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Masters.Database.Interface
{
    public interface IEntityFinder
    {
        internal User FindAdmin();
        /// <summary>
        /// Первый месяц из БД
        /// </summary>
        /// <returns></returns>
        internal Month GetFirstMonth();
        /// <summary>
        /// Находим пользователя по ChatId
        /// </summary>
        /// <param name="chatId">ChatId</param>
        /// <returns></returns>
        internal User FindUser(long chatId);
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
        /// Получить день который самый ранний
        /// </summary>
        /// <returns></returns>
        internal Day GetFirstDay();




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
        /// Найти первый существующий день в текущем месяце
        /// </summary>
        /// <returns></returns>
        async Task<Day> GetFirstDayAsync()
        {
            return await Task.Run(() => GetFirstDay());
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
        /// Найти всех пользователей
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        async Task<List<User>> FindUsersAsync()
        {
            return await Task.Run(() => FindUsers());
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
        /// Найти первый месяц
        /// </summary>
        /// <returns></returns>
        async Task<Month> GetFirstMonthAsync()
        {
            return await Task.Run(() => GetFirstMonth());
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
        /// Найти пользователя по Id
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns></returns>
        async Task<User> FindUserAsync(int userId)
        {
            return await Task.Run(() => FindUser(userId));
        }
    }
}
