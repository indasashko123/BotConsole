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
         internal User FindAdmin();
         internal User FindUser(long chatId);
         internal User CreateNewUser(string username, string firstName, string lastName, long id);
         internal List<Month> GetMonths();
         internal List<Day> FindDays(int monthId, int userId);
         internal List<Day> FindDays(int monthId);
         internal void AddMonth(int user, Month month);
         internal void CreateDays(int user, int currentDay, Month month);
         internal void CreateDays(int user, Month month);
         internal void UpdateApp(Appointment app);
         internal void UpdateUserStatus(User user);
         internal void DeleteApp(Appointment app);
         internal User MakeAdmin(User user);
         internal List<Appointment> FindAppointments(int dayId, int userId);
         internal List<Appointment> FindAppointments(int dayId);
         internal Appointment FindAppointment(int appId);
         internal Appointment AddAppointment(int dayId);
         internal void AddAppointment(Appointment app);
         internal Day FindDay(int dayid);
         internal Day FindDayByAppoint(int appId);
         internal User FindUserByAppointment(int appId);
         internal User FindUser(int userId);
        internal List<User> FindUsers();
        internal void UpdateDay(Day day);

        #region ASYNC
        async Task<User> FindAdminAsync()
        {
            return await Task.Run(() => FindAdmin());
        }
         async Task<User> FindUserAsync(long chatId)
        {
            return await Task.Run(() => FindUser(chatId));
        }
         async Task<User> CreateNewUserAsync(string username, string firstName, string lastName, long id)
        {
            return await Task.Run(() => CreateNewUser(username, firstName, lastName, id));
        }
         async Task<List<Month>> GetMonthsAsync()
        {
            return await Task.Run(() => GetMonths());
        }
         async Task<List<Day>> FindDaysAsync(int monthId, int userId)
        {
            return await Task.Run(() => FindDays(monthId, userId));
        }
         async Task<List<Day>> FindDaysAsync(int monthId)
        {
            return await Task.Run(() => FindDays(monthId));
        }
         async Task AddMonthAsync(int user, Month month)
        {
            await Task.Run(() => AddMonth(user, month));
        }
         async Task CreateDaysAsync(int user, int currentDay, Month month)
        {
            await Task.Run(() => CreateDays(user, currentDay, month));
        }
         async Task CreateDaysAsync(int user, Month month)
        {
            await Task.Run(() => CreateDays(user, month));
        }
         async Task<User> MakeAdminAsync(User user)
        {
            return await Task.Run(() => MakeAdmin(user));
        }
         async Task<List<Appointment>> FindAppointmentsAsync(int dayId, int userId)
        {
            return await Task.Run(() => FindAppointments(dayId, userId));
        }
         async Task<List<Appointment>> FindAppointmentsAsync(int dayId)
        {
            return await Task.Run(() => FindAppointments(dayId));
        }
         async Task<Appointment> FindAppointmentAsync(int appId)
        {
            return await Task.Run(() => FindAppointment(appId));
        }
         async Task<Day> FindDayAsync(int dayId)
        {
            return await Task.Run(() => FindDay(dayId));
        }
         async Task UpdateAppAsync(Appointment app)
        {
            await Task.Run(() => UpdateApp(app));
        }
         async Task<Day> FindDayByAppointAsync(int appId)
        {
            return await Task.Run(() => FindDayByAppoint(appId));
        }
         async Task UpdateUserStatusAsync(User user)
        {
            await Task.Run(() => UpdateUserStatus(user));
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
         async Task DeleteAppAsync(Appointment app)
        {
            await Task.Run(() => DeleteApp(app));
        }
         async Task<User> FindUserByAppointmentAsync(int appId)
        {
            return await Task.Run(() => FindUserByAppointment(appId));
        }
         async Task<User> FindUserAsync(int userId)
        {
            return await Task.Run(() => FindUser(userId));
        }
         async Task AddAppointmentAsync(Appointment app)
        {
            await Task.Run(() => AddAppointment(app));
        }
        async Task<List<User>> FindUsersAsync ()
        {
            return await Task.Run(() => FindUsers());
        }
        async Task UpdateDayAsync(Day day)
        {
            await Task.Run(() => UpdateDay(day));
        }

        
        #endregion

    }
}
