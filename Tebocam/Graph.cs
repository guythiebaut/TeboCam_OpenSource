using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Drawing;

namespace TeboCam
{
    public static class GraphToSave
    {
        public static System.Drawing.Bitmap graphBitmap;
    }

    public static class GraphToDisplay
    {
        public static System.Drawing.Bitmap graphBitmap;
    }

    //[Serializable]
    public class graphHist
    {
        public string date;
        public ArrayList vals = new ArrayList();
        //public graphHist() { }
        //public graphHist(string date, ArrayList vals)
        //{
        //    this.date = date;
        //    this.vals = vals;
        //}
    }


    [Serializable]
    public class GraphData
    {

        public List<GraphDataLine> Data;

        public GraphData()
        { }

    }

    [Serializable]
    public class GraphDataLine
    {
        public DateTime DT;
        public List<Int32> Vals = new List<int>();

        public GraphDataLine() { }
    }


    [Serializable]
    public class Graph
    {
        public List<graphHist> graphHistory = new List<graphHist>();
        [XmlIgnore]
        public IException tebowebException;
        [XmlIgnore]
        public int graphSeq = 0;
        [XmlIgnore]
        public string graphCurrentDate;

        public Graph() { }

        public Graph ReadXMLFile(string filename)
        {
            return Serialization.SerializeFromXmlFile<Graph>(filename);
        }

        public void WriteXMLFile(string filename, Graph graph)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            Serialization.SerializeToXmlFile(filename, graph);
        }

        private void AddGraphHist(string date, ArrayList vals)
        {
            ArrayList newVals = new ArrayList();
            newVals.AddRange(vals);
            graphHist hist = new graphHist
            {
                date = date,
                vals = newVals,
            };
            graphHistory.Add(hist);
        }

        public void updateGraphHist(string date, ArrayList vals)
        {

            //date is in format yyyyMMdd

            bool dateAlreadyExists = false;
            int days = graphHistory.Count;
            //ArrayList histVals = new ArrayList();
            int tmpInt1 = 0;
            int tmpInt2 = 0;

            try
            {
                //add data to already existing history if it already exists
                for (int i = 0; i < days; i++)
                {
                    if (graphHistory[i].date == date)
                    {
                        dateAlreadyExists = true;
                        for (int a = 0; a < 12; a++)
                        {
                            tmpInt1 = Convert.ToInt32(graphHistory[i].vals[a].ToString());
                            tmpInt2 = Convert.ToInt32(vals[a].ToString());
                            graphHistory[i].vals[a] = tmpInt1 + tmpInt2;
                        }
                    }
                }

                if (!dateAlreadyExists)
                {
                    AddGraphHist(date, vals);
                }

                for (int i = 0; i < 12; i++)
                {
                    vals[i] = 0;
                }

            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
            }
        }

        public ArrayList getGraphHist(string date)
        {
            //date is in format yyyyMMdd

            int days = graphHistory.Count;

            for (int i = 0; i < days; i++)
            {
                if (graphHistory[i].date == date)
                {
                    return graphHistory[i].vals;
                }
            }

            return null;

        }

        public string getGraphVal(string date, int cellIdx)
        {
            //date is in format yyyyMMdd

            int days = graphHistory.Count;

            for (int i = 0; i < days; i++)
            {
                if (graphHistory[i].date == date)
                {
                    return graphHistory[i].vals[cellIdx].ToString();
                }
            }

            return "0";

        }


        public ArrayList getGraphDates()
        {
            ArrayList dates = new ArrayList();
            int days = graphHistory.Count;

            for (int i = 0; i < days; i++)
            {
                dates.Add(graphHistory[i].date);
            }
            
            //date is in format yyyyMMdd
            return dates;
        }

        public List<string> getGraphDatesAsStrings()
        {
            List<string> dates = new List<string>();
            int days = graphHistory.Count;

            for (int i = 0; i < days; i++)
            {
                dates.Add(graphHistory[i].date);
            }
            
            //date is in format yyyyMMdd
            return dates;
        }

        public bool dataExistsForDate(string date)
        {
            //date is in format yyyyMMdd
            return graphHistory.Any(x => x.date == date);
        }


        public Bitmap GetGraphFromDate(string graphDate, Size size, IException exception, string displayText = "")
        {
            try
            {
                ArrayList lineXpos = new ArrayList();
                graphCurrentDate = graphDate;
                bool movement = false;
                int windowHeight = size.Height;
                int windowWidth = size.Width;
                int timeSep = (int)((windowWidth - 80) / 12) + 5;
                int curPos = timeSep - 20;
                Bitmap bitmap = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Graphics graphicsObj = Graphics.FromImage(bitmap);
                Pen linePen = new Pen(System.Drawing.Color.LemonChiffon, 3);
                Pen thinPen = new Pen(System.Drawing.Color.LemonChiffon, 1);
                Pen redPen = new Pen(System.Drawing.Color.Red, 4);
                string title = "";

                if (graphDate != null)
                {
                    title = LeftRightMid.Right(graphDate, 2) + "/" + LeftRightMid.Mid(graphDate, 4, 2) + "/" +
                            LeftRightMid.Left(graphDate, 4);
                }
                graphicsObj.DrawString(title, new Font("Tahoma", 8, FontStyle.Bold), Brushes.LemonChiffon,
                    new PointF(5, 5));

                int lineLength = 170;
                int lineWidth = 10;
                int lineHeight = lineLength;
                int topHt = 150;
                int startY = 0;
                int xPos = 0;
                double heightMod = 0;


                if (string.IsNullOrEmpty(displayText))
                {
                    for (int hour = 0; hour < 24; hour++)
                    {
                        int cellIdx = Convert.ToInt32((int)Math.Floor((decimal)(hour / 2)));
                        //Draw times
                        string fromTime = hour < 10 ? $"0{hour}:00" : $"{hour}:00";
                        string ToTime = hour + 1 < 10 ? $"0{hour + 1}:59" : $"{hour + 1}:59";
                        graphicsObj.DrawString(fromTime + "\n" + ToTime, new Font("Tahoma", 8), Brushes.LemonChiffon,
                            new PointF(curPos, windowHeight - 30));
                        int currHour = Convert.ToInt32(LeftRightMid.Left(time.currentTime(), 2));
                        int currHourIdx = Convert.ToInt32((int)Math.Floor((decimal)(currHour / 2)));
                        if (graphDate == time.currentDateYYYYMMDD() && cellIdx == currHourIdx)
                        {
                            Rectangle rect1 = new Rectangle(curPos - 2, windowHeight - 31, 35, 27);
                            graphicsObj.DrawRectangle(thinPen, rect1);
                        }

                        startY = (windowHeight - lineLength) - 35;
                        xPos = curPos + 10;
                        lineXpos.Add(xPos);

                        if (graphDate != null)
                        {
                            //draw lines
                            ArrayList graphData = getGraphHist(graphDate);
                            if (graphData == null)
                            {
                                return bitmap;
                            }

                            if (Convert.ToInt32(graphData[cellIdx]) != 0)
                            {
                                int maxVal = 0;
                                int lastV = 0;
                                int regVals = 0;
                                foreach (int v in graphData)
                                {
                                    if (v > 0) regVals++;
                                    if (v > lastV)
                                    {
                                        maxVal = v;
                                        lastV = v;
                                    }
                                }

                                heightMod = (double)topHt / (double)maxVal;
                                if (regVals > 1) heightMod = 0.5 * heightMod;
                                int gVal = (int)graphData[cellIdx];
                                lineHeight = Convert.ToInt32(Math.Floor((double)gVal * heightMod));
                                startY = startY + lineLength - lineHeight;
                                movement = true;
                                //yellow outline
                                Rectangle rectangleObj = new Rectangle(xPos, startY, lineWidth, lineHeight);
                                graphicsObj.DrawRectangle(linePen, rectangleObj);
                                //red filler
                                Rectangle rectangleObj2 = new Rectangle(xPos + 4, startY + 4, 3, lineHeight - 7);
                                graphicsObj.DrawRectangle(redPen, rectangleObj2);
                                string thisVal = getGraphVal(graphDate, cellIdx);
                                graphicsObj.DrawString(thisVal, new Font("Tahoma", 8), Brushes.LemonChiffon,
                                    new PointF(curPos + 7, lineLength - (lineHeight)));
                            }
                        }
                        //increment i for next two hour slot
                        hour += 1;
                        curPos += timeSep;
                    }
                }

                if (!movement && string.IsNullOrEmpty(displayText))
                {
                    graphicsObj.DrawString("No Movement Detected", new Font("Tahoma", 20), Brushes.LemonChiffon,
                        new PointF(80, windowHeight - 140));
                }

                if (!string.IsNullOrEmpty(displayText))
                {
                    graphicsObj.DrawString(displayText, new Font("Tahoma", 20), Brushes.LemonChiffon,
                        new PointF(80, windowHeight - 140));
                }

                graphicsObj.Dispose();
                return bitmap;
            }
            catch (Exception e)
            {
                exception.LogException(e);
                return null;
            }
        }


    }
}
