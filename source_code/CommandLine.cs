using System;
using System.Collections.Generic;
using System.Text;

namespace teboweb
{
    class CommandLine
    {

        private static string p_spaceCode = "";

        private static Random rand = new Random();
        protected static int GetRandInt(int min, int max)
        {
            return rand.Next(min, max);
        }

        //add 3 character white space replacement code to the end of the string
        //so that when we restore the white space we can ascertain the code to change to white space
        public static string finaliseCommandLine(string val)
        {

            spaceCode(val);

            string tmpStr = val + p_spaceCode;
            return tmpStr.Replace(" ", p_spaceCode);

        }

        public static string prepareCommandLine(string val)
        {
            
            if (val.Length > 3)
            {

                p_spaceCode = val.Substring(val.Length - 3, 3);
                val.Remove(val.Length - 3, 3);
                val = replaceCodeWithSpaces(val);

            }

            return val;

        }

        //replace white space with 3 character generated code
        private static string replaceCodeWithSpaces(string val)
        {

            string tmpStr = val;
            return tmpStr.Replace(p_spaceCode, " ");

        }

        //we need to replace spaces with 3 characters
        //in order to esnure that we are not using a combination in the character
        //string we are changing we keep generating 3 characters until we hit a unique combination
        private static void spaceCode(string commandStr)
        {

            string buildStr;

            do
            {

                buildStr = "";
                for (int i = 0; i < 3; i++)
                {

                    //we want characters between A-Z
                    int ranNum = GetRandInt(65, 90);

                    char c = (char)ranNum;

                    buildStr += c.ToString();

                }

            } while (commandStr.IndexOf(buildStr) != -1);

            p_spaceCode = buildStr;

        }

    }
}
