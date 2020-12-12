using System;
using System.Collections;
using System.Collections.Generic;
using MailKit.Net.Smtp;
using MimeKit;

//https://stackoverflow.com/a/46203185

namespace TeboCam
{
    class Mail : IMail
    {
        public static bool spamStopped = false;
        public static ArrayList attachments = new ArrayList();
        public static List<int> emailTimeSent = new List<int>();
        public static List<EmailSent> EmailsSent = new List<EmailSent>();
        public IException tebowebException;

        public void SetExceptionHandler(IException exceptionHandler)
        {
            tebowebException = exceptionHandler;
        }

        public void addAttachment(string a)
        {
            throw new NotImplementedException();
        }

        public void clearAttachments()
        {
            throw new NotImplementedException();
        }

        public void sendEmail(EmailFields eml)
        {
            var client = new SmtpClient();
            client.Connect(eml.SmtpHost, eml.SmtpPort, false);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(eml.User, eml.Password);
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(eml.ReplyTo));
            msg.To.Add(new MailboxAddress(eml.SendTo));
            msg.Subject = eml.Subject;

            msg.Body = new TextPart("plain")
            {
                Text = eml.BodyText
            };

            client.Send(msg);
            client.Disconnect(true);

        }

        public bool SpamAlert(int p_emails, int p_mins, bool p_deSpamify, int p_currTime)
        {
            throw new NotImplementedException();
        }

        public bool SpamIsStopped()
        {
            throw new NotImplementedException();
        }

        public void StopSpam(bool stop)
        {
            throw new NotImplementedException();
        }

        public bool validEmail(string emailAddress)
        {
            throw new NotImplementedException();
        }

        public void SetTestStatus(int status)
        {
            throw new NotImplementedException();
        }

        public int GetTestStatus()
        {
            throw new NotImplementedException();
        }

        public class EmailSent
        {
            public string ConcatInfo = string.Empty;
            public int TimeSent;
        }

    }
}
