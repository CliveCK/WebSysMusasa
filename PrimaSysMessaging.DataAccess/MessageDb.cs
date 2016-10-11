using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace PrimaSysMessaging.BusinessLogic
{
    public class MessageDb
    {
        public MessageDb()
        {
            new Functions();
        }

        public DataTable GetAllMessages(string userID)
        {
            return Functions.ExecuteParamerizedSelectCommand("FetchMessages", new SqlParameter[] 
            { 
                new SqlParameter("@recieverId", userID) 
            });
        }


        public bool SendMessage(string userid, string sender, string subject, string body)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@recieverId", userid),
                new SqlParameter("@senderId", sender),
                new SqlParameter("@subject", subject),
                new SqlParameter("@body", body)
            };

            return Functions.ExecuteNonQuery("SendMessage", parameters);
        }

        public DataTable GetMessageDetails(string readerId, int messageId)
        {
            SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@id", messageId),
            new SqlParameter("@userId", readerId)
        };

            return Functions.ExecuteParamerizedSelectCommand("ReadMessage", parameters);
        }

        public int GetMessageCount(int userid, int type) //1 = new
        {
            SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@userid", userid),
            new SqlParameter("@type", type)
        };

            return (int)Functions.ExecuteParamerizedSelectCommand("GetMessageCount", parameters).Rows[0]["MsgCount"];
        }

        public DataTable GetSentMessages(string userID)
        {
            return Functions.ExecuteParamerizedSelectCommand("GetSentMessages", new SqlParameter[] 
            { 
                new SqlParameter("@userId", userID) 
            });
        }

        public DataTable GetDeletedMessages(string userID)
        {
            return Functions.ExecuteParamerizedSelectCommand("GetDeletedMessages", new SqlParameter[] 
            { 
                new SqlParameter("@userId", userID) 
            });
        }

        public bool MarkMessageRead(int msgId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", msgId)            
            };

            return Functions.ExecuteNonQuery("MarkAsRead", parameters);
        }
    }
}