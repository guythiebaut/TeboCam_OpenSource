using NUnit.Framework;
using System;

namespace TeboCam.Tests
{
    [TestFixture]
    public class Tests
    {

        [Test]
        [Description("SendEmailTest")]
        public static void SendEmailTest()
        {
            var secure = EmailSecurity();
            IMail email = new Mail();
            var eml = new EmailFields()
            {
                SentBy = secure.SentBy,
                SentByName = secure.SentByName,
                SendTo = "guythiebaut@gmail.com",
                Subject = "Unit Test SendEmailTest",
                BodyText = $"{DateTime.Now} - This is a unit test for SendEmailTest from Tebocam for mailOLD",
                ReplyTo = secure.ReplyTo,
                Attachments = false,
                CurrentTime = 1,
                User = secure.User,
                Password = secure.Password,
                SmtpHost = secure.SmtpHost,
                SmtpPort = secure.SmtpPort,
                EnableSsl = secure.EnableSsl
            };
            email.sendEmail(eml);
        }

        [Test]
        [Description("SendEmailOLDTest")]
        public static void SendEmailOLDTest()
        {
            var secure = EmailSecurity();
            IMail email = new mailOLD();
            var eml = new EmailFields()
            {
                SentBy = secure.SentBy,
                SentByName = secure.SentByName,
                SendTo = "guythiebaut@gmail.com",
                Subject = "Unit Test SendEmailTest",
                BodyText = $"{DateTime.Now} - This is a unit test for SendEmailTest from Tebocam",
                ReplyTo = secure.ReplyTo,
                Attachments = false,
                CurrentTime = 1,
                User = secure.User,
                Password = secure.Password,
                SmtpHost = secure.SmtpHost,
                SmtpPort = secure.SmtpPort,
                EnableSsl = secure.EnableSsl
            };
            email.sendEmail(eml);
        }

        [Test]
        [Description("DummyTest")]
        public static void DummyTest()
        {
            Assert.AreEqual(1, 1);
        }

        private static EmailSecure EmailSecurity()
        {
            return new EmailSecure()
            {
                SmtpHost = "smtp.livemail.co.uk",
                User = "mail@teboweb.com",
                Password = "karuna123",
                SentBy = "mail@teboweb.com",
                SentByName = "Guy Thiebaut",
                SendTo = "guythiebaut@gmail.com",
                ReplyTo = "mail@teboweb.com",
                SmtpPort = 25,
                EnableSsl = true
            };

        }

        private class EmailSecure
        {
            public string SmtpHost;
            public string User;
            public String Password;
            public string SentBy;
            public string SentByName ;
            public string SendTo;
            public string ReplyTo;
            public int SmtpPort;
            public bool EnableSsl;
        }


    }
}
