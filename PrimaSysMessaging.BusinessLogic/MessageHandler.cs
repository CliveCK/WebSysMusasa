using System;
using System.Data;
using BusinessLogic;
using System.Collections.Generic;
using OneApi.Config;
using OneApi.Client.Impl;
using OneApi.Model;

namespace PrimaSysMessaging.BusinessLogic
{
    public class MessageHandler
    {
        MessageDb messageDb = null;        

        public MessageHandler()
        {
            messageDb = new MessageDb();
        }

        public int GetMessageCount(int userID, int type)
        {
            return messageDb.GetMessageCount(userID, type);
        }

        public DataTable GetAllMessages(string userID)
        {
            return messageDb.GetAllMessages(userID);
        }

        public DataTable GetDeletedMessages(string userID)
        {
            return messageDb.GetDeletedMessages(userID);
        }

        public bool SendMessage(string userid, string sender, string subject, string body)
        {
            return messageDb.SendMessage(userid, sender, subject, body);
        }

        public Message GetMessageDetails(string readerId, int messageId, bool markasread)
        {
            DataTable table = messageDb.GetMessageDetails(readerId, messageId);

            if (table.Rows.Count == 0)
            {
                return null;
            }

            Message msg = new Message();

            msg.Date = Convert.ToDateTime(table.Rows[0]["datentime"].ToString());
            msg.MessageId = Convert.ToInt32(table.Rows[0]["MessageID"].ToString());
            msg.RecieverId = table.Rows[0]["recieverID"].ToString();
            msg.Status = table.Rows[0]["status"].ToString();
            msg.SenderId = table.Rows[0]["senderID"].ToString();
            msg.Subject = table.Rows[0]["subject"].ToString();
            msg.Body = table.Rows[0]["body"].ToString();
            msg.Receiver = table.Rows[0]["receiver"].ToString();
            msg.Sender = table.Rows[0]["sender"].ToString();

            //Before returning lets mark this message as read
            if (markasread == true) { 
                   messageDb.MarkMessageRead(messageId);
            }

            return msg;
        }

        public DataTable GetSentMessages(string userID)
        {
            return messageDb.GetSentMessages(userID);
        }

        public void SendEmail(long[] UserID, string Message, string Subject)
        {
            //Get the User Email address
            SecurityPolicy.UserManager User = new SecurityPolicy.UserManager(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID);
            List<string> email = new List<string>();
            string[] Cell = new string[] { };

            foreach (long u in UserID)
            {
                var _with1 = User;

                _with1.Retrieve(u);
                email.Add(_with1.EmailAddress);

                SendMessage(u.ToString(), CookiesWrapper.thisUserID.ToString(), Subject, Message);
;            }

            EmailSMSDistribution objCampaign = new EmailSMSDistribution(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID);


            if (objCampaign.CreateCampaign((User.Retrieve(CookiesWrapper.thisUserID) ? User.EmailAddress : ""), Cell, email.ToArray() , Message, Subject, ""))
            {
            }

        }

        public bool SendEmail_SMS(string[] UserID, string[] Emailaddress, string[] CellNumbers, string Message, string Subject, long SendType)
        {

            EmailSMSDistribution objCampaign = new EmailSMSDistribution(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID);
            //Get the User Email address
            if (SendType == 1)
            { //Send Email
                foreach (string e in UserID)
                {

                    SendMessage(e.ToString(), CookiesWrapper.thisUserID.ToString(), Subject, Message);

                }

                if (objCampaign.CreateCampaign("", CellNumbers, Emailaddress, Message, Subject, ""))
                {
                }

                return true;
            }

            if (SendType == 2) { //Send SMS

                if (SendSMS(CellNumbers, Message))
                    return true;
                else
                    return false;

            }

            if (SendType == 3) //Send Both SMS and Email
            {
                foreach (string e in UserID)
                {

                    SendMessage(e.ToString(), CookiesWrapper.thisUserID.ToString(), Subject, Message);

                }

                if (objCampaign.CreateCampaign("", CellNumbers, Emailaddress, Message, Subject, ""))
                {
                }

                if (SendSMS(CellNumbers, Message))
                    return true;
                else
                    return false;

            }

            return false;
        }

        public string SendSMS(string[] receiverAddress, string SMSMessage, string senderAddress)
        {
            var username = Properties.Settings.Default.OneAPI_Auth_Username;
            var password = Properties.Settings.Default.OneAPI_Auth_Password;

            try
            {

                Configuration configuration = new Configuration(username, password);
                SMSClient smsClient = new SMSClient(configuration);

                SMSRequest smsRequest = new SMSRequest(senderAddress, SMSMessage, receiverAddress);

                string requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest).ToString();

                return requestId;
            }

            catch
            {
                Exception ex;
                return "";
            }
        }

        public bool SendSMS(string[] CellNumbers, string Message)
        {

            SMSLogs objSMSLog = new SMSLogs(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID);
            string request_result = SendSMS(CellNumbers, Message, Properties.Settings.Default.SMS_Sender);
            long count = 0;

            foreach (string cell in CellNumbers)
            {

                objSMSLog.SMSLogID = 0;
                objSMSLog.SenderID = CookiesWrapper.thisUserID;
                objSMSLog.ReceiverAddress = cell;
                objSMSLog.SMSMessage = Message;
                objSMSLog.SMSRequestID = request_result;
                objSMSLog.Status = request_result != "" ? "Sent" : "Pending";
                objSMSLog.DeliveryStatus = "Pending";
                objSMSLog.TimeSent = DateTime.Now.ToString();

                if (objSMSLog.Save())
                    count++;

            }

            if (count > 0)
                return true;
            else
                return false;
        }
                    
    }
}