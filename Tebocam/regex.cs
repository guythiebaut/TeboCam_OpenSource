using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TeboCam
{
    class regex
    {

        public static bool match(string pattern, string val)
        {
            if (val == null) return false;

            try
            {
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                return regex.IsMatch(val);
            }
            catch
            { return false; }

        }

    }
}
