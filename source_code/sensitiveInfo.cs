using System;
using System.Collections.Generic;
using System.Text;

//EXAMPLE FOR CODE LIBRARY OPEN SOURCE
using System;
using System.Collections.Generic;
using System.Text;

namespace TeboCam
{
    class sensitiveInfo
    {

        public static string ver = "3.003";
        public const string versionDt = "21/08/2011";
        public const string product = "TeboCam";

        public static string processToEnd = "TeboCam";
        public static string newsFile = "yourNews.Zip";
        public static string versionFile = "yourVersionFile.txt";
        public static string versionFileDev = "yourDevFile.txt";
        public static string downloadsURL = "http:YourSite.com/downloadDirectory/";

        public const string tebowebUrl = "http:www.YourSite.com";
        public static string devMachineFile = "\\yourDevFile.txt";
        public static string databaseTrialFile = "\\yourDatabaseTrialFile.txt";
        public static string dbaseConnectFile = "\\YourDbaseConnectFile.txt";
        public const string updaterPrefix = "YourUpdaterPrefix";


        //database info
        public const string server = "11.111.111.11";
        public const string dbase = "yourDatabase";
        public const string uid = "YourUserId";
        public const string pwd = "yourPassword";
        //database info

        //WARNING - YOU NEED TO CHANGE THIS ENCRYPTION INFO AS THIS IS JUST AN EXAMPLE
        //crypt info
        public static byte[] Key =  { 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123 ,123, 123, 123, 123, 123, 123, 123, 123, 123, 123 };
        public static byte[] Vector = { 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123 };
        //crypt info
        //WARNING - YOU NEED TO CHANGE THIS ENCRYPTION INFO AS THIS IS JUST AN EXAMPLE


    }
}
//EXAMPLE FOR CODE LIBRARY OPEN SOURCE