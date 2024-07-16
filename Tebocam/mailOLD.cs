using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;


namespace TeboCam
{
    public class mailOLD : IMail
    {
        public static bool spamStopped = false;
        public static ArrayList attachments = new ArrayList();
        public static List<int> emailTimeSent = new List<int>();
        public static List<EmailSent> EmailsSent = new List<EmailSent>();
        public IException tebowebException;
        public static int emailTestOk = 0;

        public void SetExceptionHandler(IException exceptionHandler)
        {
            tebowebException = exceptionHandler;
        }

        public void addAttachment(string file)
        {
            try
            {
                attachments.Add(file);
            }
            catch (Exception)
            {
                TebocamState.log.AddLine("Error adding file to email: " + file);
            }
        }

        public void clearAttachments()
        {
            try
            {
                attachments.Clear();
            }
            catch (Exception e)
            {
                tebowebException.LogException(e);
                TebocamState.log.AddLine("Error adding clearing attachments.");
            }
        }

        public bool SpamIsStopped()
        {
            return spamStopped;
        }

        public void StopSpam(bool stop)
        {
            spamStopped = stop;
        }

        public bool validEmail(string emailAddress)
        {

            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return regex.match(pattern, emailAddress);

        }

        public void sendEmail(EmailFields eml)
        {

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            string msgBody = string.Empty;
            System.Net.Mail.SmtpClient smtp = new SmtpClient();
            mail.From = new System.Net.Mail.MailAddress(eml.SentBy, eml.SentByName);
            string[] emails = eml.SendTo.Split(';');

            foreach (string email in emails)
            {
                mail.To.Add(email);
            }

            mail.Subject = eml.Subject;
            mail.Body = eml.BodyText;
            if (!eml.Attachments && emailTestOk != 9)
            {
                mail.Body += Environment.NewLine + "No images attached option selected.";
            }
            mail.IsBodyHtml = true;
            mail.ReplyTo = new MailAddress(eml.ReplyTo);

            if (eml.Attachments)
            {
                int tmpCnt = 0;

                foreach (string file in attachments)
                {
                    try
                    {
                        tmpCnt++;
                        TebocamState.log.AddLine("Adding file to email... " + tmpCnt.ToString());
                        mail.Attachments.Add(new Attachment(file));
                    }
                    catch (Exception e)
                    {
                        tebowebException.LogException(e);
                        TebocamState.log.AddLine("Error adding file to email... " + tmpCnt.ToString());
                    }
                }
            }

            smtp.Host = eml.SmtpHost;
            smtp.Port = eml.SmtpPort;
            smtp.EnableSsl = eml.EnableSsl;
            smtp.Credentials = new System.Net.NetworkCredential(eml.User, eml.Password);
            try
            {

                //20160627 fix to prevent a bug where sometimes the same email is sent continuously
                EmailSent emsent = new EmailSent();
                string contactInfo = eml.SentBy + eml.SendTo + eml.Subject + eml.BodyText + eml.ReplyTo + eml.User + eml.Password + eml.SmtpHost;

                foreach (EmailSent item in EmailsSent)
                {
                    if (item.ConcatInfo == contactInfo && time.secondsSinceStart() - item.TimeSent < 4)
                    {
                        return;
                    }
                }

                emsent.ConcatInfo = contactInfo;
                emsent.TimeSent = eml.CurrentTime;
                EmailsSent.Add(emsent);
                smtp.Send(mail);
                emailTimeSent.Add(eml.CurrentTime);
                emailTestOk = 1;
                TebocamState.log.AddLine("Email sent.");
            }

            catch (System.Exception ex)
            {
                tebowebException.LogException(ex);
                emailTestOk = 2;
                TebocamState.log.AddLine("Error in sending email.");
            }
        }

        private int mailsSentOverTime(int p_timeSpan, int p_currTime)
        {
            int emailsSent = 0;

            foreach (int time in emailTimeSent)
            {
                if (p_currTime - time <= p_timeSpan)
                {
                    emailsSent++;
                }
            }

            return emailsSent;
        }


        public bool SpamAlert(int p_emails, int p_mins, bool p_deSpamify, int p_currTime)
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

        public void SetTestStatus(int status)
        {
            emailTestOk = status;
        }

        public int GetTestStatus()
        {
            return emailTestOk;
        }
    }

    public class EmailFields
    {
        public string SentBy;
        public string SentByName;
        public string SendTo;
        public string Subject;
        public string BodyText;
        public string ReplyTo;
        public bool Attachments;
        public int CurrentTime;
        public string User;
        public string Password;
        public string SmtpHost;
        public int SmtpPort;
        public bool EnableSsl;
    }

    public class EmailSent
    {
        public string ConcatInfo = string.Empty;
        public int TimeSent;
    }
}

