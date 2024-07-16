namespace TeboCam
{
    public interface IMail
    {
        void SetExceptionHandler(IException exceptionHandler);
        void addAttachment(string a);
        void clearAttachments();
        bool validEmail(string emailAddress);
        void sendEmail(EmailFields eml);
        bool SpamAlert(int p_emails, int p_mins, bool p_deSpamify, int p_currTime);
        bool SpamIsStopped();
        void StopSpam(bool stop);
        void SetTestStatus(int status);
        int GetTestStatus();
    }
}
