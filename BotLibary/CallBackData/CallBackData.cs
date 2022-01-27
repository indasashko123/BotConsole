using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.CallBackData
{
    internal class CallBackData
    {
        internal static CallBackData GetData(string data)
        {
            return new CallBackData(data);
        }
        internal static async Task<CallBackData> GetDataAsync(string data)
        {
            return await Task.Run(() => GetData(data));
        }
        internal CallBackData(string data)
        {
            if (data == "404")
            {
                Error = "404";
                return;
            }
            if (data == "0")
            {
                EmptyButton = true;
                return;
            }            
            string[] callBackData = data.Split('/');
            if (callBackData.Length != 5)
            {
                Error = "405";
                return;
            }
            switch(callBackData[0])
            {
                case "U":
                    {
                        UserRole = UserRole.User;
                        break;
                    }
                case "A":
                    {
                        UserRole = UserRole.Admin;
                        break;
                    }
            }
            switch (callBackData[1])
            {
                case "C":
                    {
                        Action = Action.Choise;
                        break;
                    }
                case "A":
                    {
                        Action = Action.Add;
                        break;
                    }
                case "Q":
                    {
                        Action = Action.Confirm;
                        break;
                    }
                case "D":
                    {
                        Action = Action.Delete;
                        break;
                    }
                case "W":
                    {
                        Action = Action.WeekEnd;
                        break;
                    }
                case "F":
                    {
                        Action = Action.CancelApp;
                        break;
                    }
                case "M":
                    {
                        Action = Action.Mailing;
                        break;
                    }
            }
            switch (callBackData[2])
            {
                case "M":
                    {
                        Stage = Stage.Month;
                        break;
                    }
                case "D":
                    {
                        Stage = Stage.Day;
                        break;
                    }
                case "A":
                    {
                        Stage = Stage.Appointment;
                        break;
                    }
                case "Y":
                    {
                        Stage = Stage.Yes;
                        break;
                    }
                case "N":
                    {
                        Stage = Stage.No;
                        break;
                    }
            }
            EntityId = Convert.ToInt32(callBackData[3]);
            UserId = Convert.ToInt32(callBackData[4]);
            Error = "0";
            EmptyButton = false;
        }
        /// <summary>
        /// Id сущности с которой будет вестись дальнейшая работа.
        /// </summary>
        internal int EntityId { get; set; }
        internal int UserId { get; set; }
        internal UserRole UserRole {get;set;}
        internal Stage Stage { get; set; }
        internal Action Action { get; set; }
        internal string Error { get; set; }
        internal bool EmptyButton { get; set; }
    }
    internal enum Action
    {
        Add,
        Choise,
        Confirm,
        Delete,
        WeekEnd,
        CancelApp,
        Mailing
    }
    internal enum UserRole
    {
        User,
        Admin
    }
    internal enum Stage
    {
        Month,
        Day,
        Appointment,
        Yes,
        No
    }
}
