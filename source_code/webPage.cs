using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TeboCam
{
    class webPage
    {

        public static void writePage(string prefix, string suffix, int from, int to, string fileName)
        {
            int startNum = from;
            int endNum = to;

            string trStart = "<tr>";
            string trEnd = "</tr>";
            //string tdStart = @"<td><img src='";
            //string tdEnd = @"' width=""100"" height=""100"" name=""webcam""></td>";

            TextWriter tw = new StreamWriter(fileName);

            // write a line of text to the file

            tw.WriteLine("<html>");
            tw.WriteLine("<head>");
            tw.WriteLine("</head>");
            tw.WriteLine(@"<table border=""1"">");
            tw.WriteLine(trStart);

            for (int i = startNum; i <= endNum; i++)
            {
                //tw.WriteLine(tdStart + prefix + i.ToString() + suffix + tdEnd);

                tw.WriteLine
                (
                "<td><a href='" + prefix + i.ToString() + suffix + "'>" +
                "<img src='" + prefix + i.ToString() + suffix + @"' width=""100"" height=""100"" name=""webcam""></a></td>"
                );

                if (i % 10 == 0)
                {
                    tw.WriteLine(trEnd);
                    tw.WriteLine(trStart);
                }
            }

            tw.WriteLine(trEnd);
            tw.WriteLine("</table>");
            tw.WriteLine("</body>");
            tw.WriteLine("</html>");

            // close the stream
            tw.Close();

        }



    }
}
