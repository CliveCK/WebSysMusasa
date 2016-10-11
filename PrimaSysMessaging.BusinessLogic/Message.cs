using System;
using System.Data;
using System.Configuration;
using PrimaSysMessaging.BusinessLogic;

namespace PrimaSysMessaging.BusinessLogic
{
    public class Message
    {
        private int messageId;
        private string senderId;
        private string recieverId;
        private string subject;
        private string body;
        private DateTime date;
        private string status;
        private string receiver;
        private string sender;

        public int MessageId
        {
            set
            {
                messageId = value;
            }
            get
            {
                return messageId;
            }
        }
        public string SenderId
        {
            set
            {
                senderId = value;
            }
            get
            {
                return senderId;
            }
        }
        public string RecieverId
        {
            set
            {
                recieverId = value;
            }
            get
            {
                return recieverId;
            }
        }

        public string Receiver
        {
            set
            {
                receiver = value;
            }
            get
            {
                return receiver;
            }
        }

        public string Sender
        {
            set
            {
                sender = value;
            }
            get
            {
                return sender;
            }
        }

        public string Subject
        {
            set
            {
                subject = value;
            }
            get
            {
                return subject;
            }
        }
        public string Body
        {
            set
            {
                body = value;
            }
            get
            {
                return body;
            }
        }
        public DateTime Date
        {
            set
            {
                date = value;
            }
            get
            {
                return date;
            }
        }
        public string Status
        {
            set
            {
                status = value;
            }
            get
            {
                return status;
            }
        }
    }
}