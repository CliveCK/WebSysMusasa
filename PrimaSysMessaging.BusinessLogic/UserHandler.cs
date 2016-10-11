using System;
using System.Data;
using System.Configuration;
using PrimaSysMessaging.BusinessLogic;

namespace PrimaSysMessaging.BusinessLogic
{
    public class UserHandler
    {
        UserDb userDb = null;

        public UserHandler()
        {
            userDb = new UserDb();
        }

        public DataTable GetUserList()
        {
            return userDb.GetUserList();
        }

        public bool IsValidUser(string userId)
        {
            DataTable table = userDb.GetUserDetails(userId);

            if (table.Rows.Count == 0)
            {
                return false;
            }
            return true;
        }

        public bool ValidateUser(string name, string password)
        {
            DataTable table = userDb.GetUserDetails(name);

            try
            {
                if (table.Rows[0][1].ToString() == password)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public string GetUserName(string userId)
        {
            DataTable table = userDb.GetUserDetails(userId);

            if (table.Rows[0][1] != null)
            {
                return table.Rows[0]["username"].ToString();
            }
            return string.Empty;
        }

        public bool UpdatePassword(string userId, string password, string newPassword)
        {
            return userDb.UpdatePassword(userId, password, newPassword);
        }

        public bool CreateNewUser(string userId, string password, string username, string adminId)
        {
            if (IsValidUser(userId) == true)
            {
                //this user already exist so dont do anything
                return false;
            }

            return userDb.CreateUser(userId, password, username, adminId);
        }
    }
}