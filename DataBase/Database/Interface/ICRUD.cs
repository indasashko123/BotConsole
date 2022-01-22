﻿using DataBase.Models;
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
        List<Day> FindDays(int monthId);



        //Async
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
        async Task<List<Day>> FindDaysAsync(int monthId)
        {
            return await Task.Run(() => FindDays(monthId));
        }
    }
}
