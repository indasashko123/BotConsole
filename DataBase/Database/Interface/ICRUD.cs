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
        protected internal User FindAdmin();
        protected internal User FindUser(long chatId);
        protected internal User CreateNewUser(string username, string firstName, string lastName, long id);
        protected internal List<Month> GetMonths();
        protected internal List<Day> FindDays(int monthId, int userId);
        protected internal List<Day> FindDays(int monthId);
        protected internal void AddMonth(int user, Month month);
        protected internal void CreateDays(int user, int currentDay, Month month);
        protected internal void CreateDays(int user, Month month);
        protected internal void UpdateApp(Appointment app);
        protected internal void UpdateUserStatus(User user);
        protected internal void DeleteApp(Appointment app);
        protected internal User MakeAdmin(User user);
        protected internal List<Appointment> FindAppointments(int dayId, int userId);
        protected internal List<Appointment> FindAppointments(int dayId);
        protected internal Appointment FindAppointment(int appId);
        protected internal Appointment AddAppointment(int dayId);
        protected internal Day FindDay(int dayid);
        protected internal Day FindDayByAppoint(int appId);




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
        async Task<Appointment> AddAppointmentAsync(int dayId)
        {
            return await Task.Run(() => AddAppointment(dayId));
        }
        async Task DeleteAppAsync(Appointment app)
        {
            await Task.Run(() => DeleteApp(app));
        }
        #endregion

    }
}
