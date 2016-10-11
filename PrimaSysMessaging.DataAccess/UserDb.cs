using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace PrimaSysMessaging.BusinessLogic
{
    public class UserDb
    {
        public UserDb()
        {
            
        }

        public DataTable GetUserList()
        {
            return Functions.ExecuteSelectCommand("GetUserList");
        }

        public DataTable GetUserDetails(string userId)
        {
            return Functions.ExecuteParamerizedSelectCommand("GetUserDetails", new SqlParameter[] 
            { 
                new SqlParameter("@userId", userId) 
            });
        }

        public bool UpdatePassword(string userId, string password, string newPassword)
        {
            SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@userId", userId),
            new SqlParameter("@password", password),
            new SqlParameter("@newpassword", newPassword)
        };

            return Functions.ExecuteNonQuery("UpdateProfile", parameters);
        }

        public bool CreateUser(string userId, string password, string username, string adminId)
        {
            SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@id", userId),
            new SqlParameter("@pwd", password),
            new SqlParameter("@name", username),
            new SqlParameter("@adminId", adminId)
        };

            return Functions.ExecuteNonQuery("CreateUser", parameters);
        }
    }
}