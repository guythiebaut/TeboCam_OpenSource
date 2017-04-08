using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace TeboCam
{

    [Serializable]
    public class graphHist
    {
        public string date;
        public ArrayList vals = new ArrayList();
        public graphHist() { }
        public graphHist(string date, ArrayList vals)
        {
            this.date = date;
            this.vals = vals;
        }
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

        public Graph() { }

        public Graph ReadXMLFile(string filename)
        {
            return (Graph)Serialization.SerializeFromXMLFile(filename, this);
        }

        public void WriteXMLFile(string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            Serialization.SerializeToXMLFile(filename, this);
        }

        private void AddGraphHist(string date, ArrayList vals)
        {
            ArrayList newVals = new ArrayList();
            newVals.AddRange(vals);
            graphHistory.Add(new graphHist(date, newVals));
        }

        public void updateGraphHist(string date, ArrayList vals)
        {

            //date is in format yyyyMMdd

            bool dateAlreadyExists = false;
            int days = graphHistory.Count;
            ArrayList histVals = new ArrayList();
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
                        histVals = graphHistory[i].vals;
                        for (int a = 0; a < 12; a++)
                        {
                            tmpInt1 = Convert.ToInt32(histVals[a].ToString());
                            tmpInt2 = Convert.ToInt32(vals[a].ToString());
                            //histVals[a] = tmpInt2;
                            //old code 20091226
                            histVals[a] = tmpInt1 + tmpInt2;
                            //old code 20091226
                        }
                        graphHistory[i].vals = histVals;
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
            catch
            { }
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

        public bool dataExistsForDate(string date)
        {
            //date is in format yyyyMMdd

            int days = graphHistory.Count;

            for (int i = 0; i < days; i++)
            {
                if (graphHistory[i].date == date)
                {
                    return true;
                }
            }

            return false;

        }





    }
}
