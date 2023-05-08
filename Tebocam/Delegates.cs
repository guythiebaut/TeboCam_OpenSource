using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeboCam
{
    public class TeboCamDelegates
    {
        public delegate void RunWorkerCompletedDelegate(object sender, RunWorkerCompletedEventArgs e);
        public delegate void EventDelegate<T>(object sender, T e);
    }
}
