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
        User FindAdmin();
        User FindUser(long chatId);
        User CreateNewUser(string username, string firstName, string lastName, long id);
        List<Month> GetMonths();
        List<Day> FindDays(int monthId, int userId);
        void AddMonth(int user, Month month);
        void CreateDays(int user, int currentDay, Month month);
        void CreateDays(int user, Month month);
        User MakeAdmin(User user);
        List<Appointment> FindAppointments(int dayId, int userId);
        Appointment FindAppointment(int dayId, int userId);
        Day FindDay(int dayid);


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
        async Task<Appointment> FindAppointmentAsync(int dayId, int UserId)
        {
            return await Task.Run(() => FindAppointment(dayId, UserId));
        }
        async Task<Day> FindDayAsync(int dayId)
        {
            return await Task.Run(() => FindDay(dayId));
        }
        #endregion

    }
}
