using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Collections;


namespace TeboCam
{
    class database
    {
        private const string server = sensitiveInfo.server;
        private const string dbase = sensitiveInfo.dbase;
        private const string uid = sensitiveInfo.uid;
        private const string pwd = sensitiveInfo.pwd;

        public static int database_update_data(string driver, string user, string instance, string option, string value)
        {
            string update = "";
            string cmd = "";

            crypt crypt = new crypt();

            if (option == "statuson")
            {
                update = "set status = 'On'";
                cmd = "update settings " + update + " where user = '" + user + "' and instance_name = '" + instance + "'";
            }
            if (option == "statusoff")
            {
                update = "set status = 'Off'";
                cmd = "update settings " + update + " where user = '" + user + "' and instance_name = '" + instance + "'";
            }
            if (option == "statusactive")
            {
                update = "set status = 'Active'";
                cmd = "update settings " + update + " where user = '" + user + "' and instance_name = '" + instance + "'";
            }
            if (option == "statusinactive")
            {
                update = "set status = 'Inactive'";
                cmd = "update settings " + update + " where user = '" + user + "' and instance_name = '" + instance + "'";
            }
            if (option == "log")
            {
                update = "set log = '" + value + "'";
                cmd = "update settings " + update + " where user = '" + user + "' and instance_name = '" + instance + "'";
            }
            if (option == "reset")
            {
                update = "set online_request = null, command_guid=null , lastTeboCamUpdate = '" + value + "'";
                cmd = "update settings " + update + " where user = '" + user + "' and instance_name = '" + instance + "'";
            }
            if (option == "picLoc")
            {
                update = "set picloc = '" + value + "'";
                cmd = "update settings " + update + " where user = '" + user + "' and instance_name = '" + instance + "'";
            }
            if (option == "poll")
            {
                update = "set teboCamLastPoll = '" + value + "'";
                cmd = "update poll " + update + " where user = '" + user + "'";
            }

            //Console.WriteLine(cmd);

            OdbcConnection con = new OdbcConnection("Driver=" + driver + ";Server=" + server + ";Database=" + dbase + ";UID=" + uid + ";PWD=" + crypt.DecryptString(pwd) + ";OPTION=3");


            OdbcCommand com = new OdbcCommand(cmd, con);

            ArrayList result = new ArrayList();

            try
            {
                con.Open();

                OdbcDataReader dr = com.ExecuteReader();

                dr.Close();
                con.Close();

                return dr.RecordsAffected;

            }
            catch
            {
                return 0;
            }

        }

        public static bool credentials_correct(string driver, string user, string password)
        {
            crypt crypt = new crypt();
            string encPass = crypt.HashString(password);

            try
            {

                //OdbcConnection con = new OdbcConnection("Driver=" + driver + ";Server=" + server + ";Database=" + dbase + ";UID=" + uid + ";PWD=" + crypt.DecryptString(pwd) + ";OPTION=3");
                OdbcConnection con = new OdbcConnection("Driver=" + driver + ";Server=" + server + ";Database=" + dbase + ";Uid=" + uid + ";Password=" + crypt.DecryptString(pwd) + ";Option=3");


                OdbcCommand com = new OdbcCommand("SELECT user FROM users where user = '" + user + "' and password = '" + encPass + "'", con);

                con.Open();
                OdbcDataReader dr = com.ExecuteReader();

                int tmpInt = 0;

                while (dr.Read())
                {
                    tmpInt++;
                }
                dr.Close();
                con.Close();

                return tmpInt > 0;
            }
            catch
            {
                return false;
            }
        }

        public static ArrayList database_get_data(string driver, string user, string instance, string column)
        {

            crypt crypt = new crypt();

            OdbcConnection con = new OdbcConnection("Driver=" + driver + ";Server=" + server + ";Database=" + dbase + ";UID=" + uid + ";PWD=" + crypt.DecryptString(pwd) + ";OPTION=3");

            OdbcCommand com = new OdbcCommand("SELECT " + column + " FROM settings where user = '" + user + "' and instance_name = '" + instance + "'", con);

            //OdbcCommand com = new OdbcCommand("SELECT online_request FROM settings where user = '" + user + "'", con);

            ArrayList result = new ArrayList();

            try
            {
                con.Open();
                OdbcDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    if (dr.GetValue(0) != DBNull.Value)
                    {
                        result.Add(dr.GetString(0));
                    }
                    else
                    {
                        result.Add("NULL");
                    }
                }
                dr.Close();
                con.Close();

                return result;
            }
            catch
            {
                result.Add("NULL");
                return result;
            }

        }

        public static ArrayList get_licence_data(string driver, string query)
        {

            crypt crypt = new crypt();

            OdbcConnection con = new OdbcConnection("Driver=" + driver + ";Server=" + server + ";Database=" + dbase + ";UID=" + uid + ";PWD=" + crypt.DecryptString(pwd) + ";OPTION=3");

            OdbcCommand com = new OdbcCommand(query, con);

            ArrayList result = new ArrayList();

            try
            {
                con.Open();
                OdbcDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    if (dr.GetValue(0) != DBNull.Value)
                    {
                        result.Add(dr.GetString(0));
                    }
                    else
                    {
                        result.Add("NULL");
                    }
                }
                dr.Close();
                con.Close();

                return result;
            }
            catch
            {
                result.Add("NULL");
                return result;
            }

        }


        public static int set_licence_data(string driver, string cmd)
        {

            crypt crypt = new crypt();
            OdbcConnection con = new OdbcConnection("Driver=" + driver + ";Server=" + server + ";Database=" + dbase + ";UID=" + uid + ";PWD=" + crypt.DecryptString(pwd) + ";OPTION=3");
            OdbcCommand com = new OdbcCommand(cmd, con);

            ArrayList result = new ArrayList();

            try
            {
                con.Open();

                OdbcDataReader dr = com.ExecuteReader();

                dr.Close();
                con.Close();

                return dr.RecordsAffected;

            }
            catch
            {
                return 0;
            }

        }

    }
}
