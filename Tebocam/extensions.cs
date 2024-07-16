using System;
using System.ComponentModel;

namespace TeboCam
{
    static class extensions
    {

        public static void SynchronisedInvoke(this ISynchronizeInvoke synchThis, Action action)
        {
            if (!synchThis.InvokeRequired)
            {
                action();
            }
            else
            {
                synchThis.Invoke(action, new object[] { });
            }
        }
    }
}
