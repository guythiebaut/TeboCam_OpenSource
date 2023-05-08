using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;


namespace TeboCam
{

    public class versionInfo
    {
        public string version;
        public string released;
        public string info;
        public string url;
        public string product;
        public string news;
        public string file;


        public versionInfo(string version, string released, string info, string url, string product, string news,string file)
        {
            this.version = version;
            this.released = released;
            this.info = info;
            this.url = url;
            this.product = product;
            this.news = news;
            this.file = file;
        }
    }


    public class htmlInfo
    {
        public string version;
        public string released;
        public string info;
        public string url;
        public string product;
        public string news;
        

        public htmlInfo(string version, string released, string info, string url, string product, string news)
        {
            this.version = version;
            this.released = released;
            this.info = info;
            this.url = url;
            this.product = product;
            this.news = news;
        }
    }


    class html
    {

        public static event EventHandler htmlError;
        public static event EventHandler htmlOk;

        static List<htmlInfo> htmlInformation = new List<htmlInfo>();

        public static bool htmlSuccess = true;

        public static ArrayList getHtmlPage(string page)
        {
            try
            {

                // used to build entire input
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                // prepare the web page we will be asking for
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(page);
                request.Timeout = 2500;

                // execute the request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // we will read data via the response stream
                Stream resStream = response.GetResponseStream();

                string tempString = null;
                int count = 0;

                do
                {
                    // fill the buffer with data
                    count = resStream.Read(buf, 0, buf.Length);

                    // make sure we read some data
                    if (count != 0)
                    {
                        // translate from bytes to ASCII text
                        tempString = Encoding.ASCII.GetString(buf, 0, count);

                        // continue building the string
                        sb.Append(tempString);
                    }
                }
                while (count > 0); // any more data to read?

                //return page information
                string tmpStr = sb.ToString();
                string[] lines = Regex.Split(tmpStr, "\r\n", RegexOptions.Compiled);
                ArrayList tmpList = new ArrayList();

                foreach (string line in lines)
                {
                    tmpList.Add(line);
                }

                htmlSuccess = true;
                return tmpList;
            }

            catch (Exception)
            {
                htmlSuccess = false;
                htmlError(null, new EventArgs());
                return null;
            }

        }


        private static List<htmlInfo> getHtmlInfoPage(string page)
        {

            try
            {

                // used to build entire input
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                // prepare the web page we will be asking for#
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(page);
                request.Timeout = 2500;
                request.ReadWriteTimeout = 10000;


                // execute the request

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {

                    // we will read data via the response stream
                    Stream resStream = response.GetResponseStream();

                    string tempString = null;
                    int count = 0;

                    do
                    {
                        // fill the buffer with data
                        count = resStream.Read(buf, 0, buf.Length);

                        // make sure we read some data
                        if (count != 0)
                        {
                            // translate from bytes to ASCII text
                            tempString = Encoding.ASCII.GetString(buf, 0, count);

                            // continue building the string
                            sb.Append(tempString);
                        }
                    }
                    while (count > 0); // any more data to read?

                    resStream.Close();


                }

                //return page information
                string tmpStr = sb.ToString();
                string[] lines = Regex.Split(tmpStr, "\r\n", RegexOptions.Compiled);

                for (int i = 1; i < lines.Length; i++)
                {

                    if (Regex.IsMatch(lines[i], @"\|\|"))
                    {
                        string product = val(lines[i + 1]);
                        string version = val(lines[i + 2]);
                        string released = val(lines[i + 3]);
                        string info = val(lines[i + 4]);
                        string url = val(lines[i + 5]);
                        string news = val(lines[i + 6]);

                        version = Decimal.Parse(version, new System.Globalization.CultureInfo("en-GB")).ToString();

                        htmlInformation.Add(new htmlInfo(version, released, info, url, product, news));
                    }

                }
                htmlSuccess = true;
                htmlOk(null, new EventArgs());
                return htmlInformation;

            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                htmlSuccess = false;
                htmlError(null, new EventArgs());
                return null;
            }

        }

        public static htmlInfo getUpdateInfo(string page, string product)
        {

            List<htmlInfo> info = new List<htmlInfo>();
            info = getHtmlInfoPage(page);


            htmlInfo updateInfo = new htmlInfo("0", "1 Jan 2000", "N/A", "www/teboweb.com", "N/A", "N/A");

            if (htmlSuccess)
            {
                foreach (htmlInfo var in info)
                {
                    if (var.product == product)
                    {
                        updateInfo = var;
                        break;
                    }
                }
            }

            return updateInfo;

        }

        private static string val(string line)
        {
            return line.Substring(4, line.Length - 9);
        }





    }
}
