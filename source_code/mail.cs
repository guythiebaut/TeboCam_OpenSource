using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TeboCam
{
    public class mail
    {

        public static bool spamStopped = false;
        public static ArrayList attachments = new ArrayList();
        public static List<int> emailTimeSent = new List<int>();

        public static void addAttachment(string file)
        {
            try
            {
                attachments.Add(file);
            }
            catch (Exception)
            {
                bubble.logAddLine("Error adding file to email: " + file);
            }
        }

        public static void clearAttachments()
        {
            try
            {
                attachments.Clear();
            }
            catch (Exception)
            {
                bubble.logAddLine("Error adding clearing attachments.");
            }
        }




        public static bool validEmail(string emailAddress)
        {

            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return regex.match(pattern, emailAddress);

        }

        public static void sendEmail(string by, string to, string subj, string body,
                                     string replyTo, bool hasAttachments, int curTime,
                                     string emailUser, string emailPass, string smtpHost,
                                     int smtpPort, bool EnableSsl)
        {

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            string msgBody = string.Empty;
            System.Net.Mail.SmtpClient smtp = new SmtpClient();

            mail.From = new System.Net.Mail.MailAddress(by, config.getProfile(bubble.profileInUse).sentByName);

            string[] emails = to.Split(';');

            foreach (string email in emails)
            {

                mail.To.Add(email);

            }

            mail.Subject = subj;
            mail.Body = body;
            if (!hasAttachments && bubble.emailTestOk != 9)
            {
                mail.Body += Environment.NewLine + "No images attached option selected.";
            }
            mail.IsBodyHtml = true;
            mail.ReplyTo = new MailAddress(replyTo); 



            if (hasAttachments)
            {
                int tmpCnt = 0;

                foreach (string file in attachments)
                {
                    try
                    {
                        tmpCnt++;
                        bubble.logAddLine("Adding file to email... " + tmpCnt.ToString());
                        mail.Attachments.Add(new Attachment(file));
                    }

                    catch
                    {
                        bubble.logAddLine("Error adding file to email... " + tmpCnt.ToString());
                    }

                }

            }



            smtp.Host = smtpHost;
            smtp.Port = smtpPort;
            smtp.EnableSsl = EnableSsl;
            smtp.Credentials = new System.Net.NetworkCredential(emailUser, emailPass);

            try
            {
                smtp.Send(mail);
                emailTimeSent.Add(curTime);
                bubble.emailTestOk = 1;
                bubble.logAddLine("Email sent.");
            }

            catch (System.Exception ex)
            {
                bubble.emailTestOk = 2;
                bubble.logAddLine("Error in sending email.");
            }
        }

        private static int mailsSentOverTime(int p_timeSpan, int p_currTime)
        {

            int emailsSent = 0;

            foreach (int time in mail.emailTimeSent)
            {

                if (p_currTime - time <= p_timeSpan)
                {

                    emailsSent++;

                }

            }

            return emailsSent;

        }


        public static bool SpamAlert(int p_emails, int p_mins, bool p_deSpamify, int p_currTime)
        {

            if (p_deSpamify)
            {

                int emailsSent = mailsSentOverTime(p_mins * 60, p_currTime);

                if (emailsSent >= p_emails)
                {

                    spamStopped = true;

                }

                return emailsSent >= p_emails;

            }

            return false;

        }

    }


}

