using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeboCam
{
    public static class Valid
    {
        public static bool IsNumeric(string inString)
        {
            System.Text.RegularExpressions.Regex objNotWholePattern = new System.Text.RegularExpressions.Regex("[^0-9]");
            return !objNotWholePattern.IsMatch(inString)
                 && (inString != "");
        }

        public static bool IsDecimal(string inString)
        {
            decimal dec;
            return Decimal.TryParse(inString, out dec);
        }


        public static bool filenamePrefixValid(string inString)
        {
            bool tmpBool = false;

            System.Text.RegularExpressions.Regex valid = new System.Text.RegularExpressions.Regex("[0-9a-zA-Z]");

            string tmpStr = "";

            for (int i = 0; i < inString.Length; i++)
            {
                tmpStr = LeftRightMid.Mid(inString, i, 1);
                tmpBool = valid.IsMatch(tmpStr);
                if (!tmpBool) { break; }
            }

            return tmpBool;

        }



        public static string verifyInt(string inVal, Int64 lowerLimit, Int64 upperLimit, string errorVal)
        {

            try
            {

                if (!Valid.IsNumeric(inVal)) { return errorVal; }

                if (Convert.ToInt32(inVal) >= lowerLimit && Convert.ToInt32(inVal) <= upperLimit)
                { return inVal; }
                else
                { return errorVal; }

            }

            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                return errorVal;
            }

        }


        public static string verifyDouble(string inVal, double lowerLimit, double upperLimit, string errorVal)
        {

            double tmpDouble;

            if (!double.TryParse(inVal, out tmpDouble))
            {
                return errorVal;
            }
            else
            {
                if (tmpDouble >= lowerLimit && tmpDouble <= upperLimit)
                {
                    return inVal;
                }
                else
                {
                    return errorVal;
                }
            }

        }


        public static string doubleConvert(string decString)
        {
            return Decimal.Parse(decString, new System.Globalization.CultureInfo("en-GB")).ToString();
        }

    }
}
