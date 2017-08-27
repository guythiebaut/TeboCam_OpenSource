using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeboCam
{
    public class Interfaces
    {

        public interface IEncryption
        {

            string EncryptToString(string a);
            string DecryptString(string a);

        }

    }
}
