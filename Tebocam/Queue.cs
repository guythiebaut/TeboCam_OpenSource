using System;
using System.Collections.Generic;

namespace TeboCam
{

    public class Queue
    {

        public List<QueueItem> QueueItems = new List<QueueItem>();

        public class QueueItem
        {
            public int? CamNo = null;
            public DateTime DateTimeAdded = DateTime.Now;
            public DateTime? DateTimeProcessed;
            public string Instruction;
            public List<string> Parms = new List<string>();
            public List<int> CamsProcessed = new List<int>();
            public bool RemoveFromQueue = false;
        }


    }
}
