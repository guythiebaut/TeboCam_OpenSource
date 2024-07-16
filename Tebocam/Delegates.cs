using System.ComponentModel;

namespace TeboCam
{
    public class TeboCamDelegates
    {
        public delegate void RunWorkerCompletedDelegate(object sender, RunWorkerCompletedEventArgs e);
        public delegate void EventDelegate<T>(object sender, T e);
    }
}
