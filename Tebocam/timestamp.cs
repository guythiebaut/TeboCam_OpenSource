using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace TeboCam
{
    public partial class timestamp : Form
    {

        private formDelegateList timestampDelegate;
        private string fromString;
        private bool addTimeStamp;
        private string inFormat;
        private string inColour;
        private string inPosition;
        private bool txtRect;
        private bool toolTip;
        private bool showStats;
        private bool includeStats;
        private string previousImage = "Alert";
        private bool initializing = true;

        private List<List<object>> stampList = new List<List<object>>();


        public timestamp(formDelegateList sender, List<List<object>> from)
        {

            InitializeComponent();

            stampList = from;
            timestampDelegate = sender;
            comboBox1.SelectedIndex = 0;
            populateFromList(comboBox1.Text);
            initializing = false;


        }


        private void populateFromList(string name)
        {

            foreach (List<object> item in stampList)
            {
                if (item[0].ToString() == name)
                {

                    addStamp.Checked = Convert.ToBoolean(item[1]);
                    stampType.SelectedIndex = stampTypesetting(item[2].ToString());
                    stampColour.SelectedIndex = stampColoursetting(item[3].ToString());
                    drawRect.Checked = Convert.ToBoolean(item[5]);
                    statsBox.Enabled = Convert.ToBoolean(item[6]);

                    statsChk.Checked = Convert.ToBoolean(item[7]);
                    tl.Checked = item[4].ToString() == "tl";
                    tr.Checked = item[4].ToString() == "tr";
                    bl.Checked = item[4].ToString() == "bl";
                    br.Checked = item[4].ToString() == "br";

                    groupBox1.Enabled = Convert.ToBoolean(item[1]);
                    groupBox3.Enabled = Convert.ToBoolean(item[1]);
                    stampColour.Enabled = Convert.ToBoolean(item[1]);
                    stampType.Enabled = Convert.ToBoolean(item[1]);


                    if ((bool)item[1])
                    {

                        statsBox.Enabled = (bool)item[6];

                    }
                    else
                    {

                        statsBox.Enabled = false;

                    }

                }

            }


        }


        private void updateList(string name)
        {


            for (int i = 0; i < stampList.Count; i++)
            {

                if (stampList[i][0].ToString() == name)
                {

                    stampList[i][1] = addStamp.Checked;
                    stampList[i][2] = stampTypesetting();
                    stampList[i][3] = stampColoursetting();
                    stampList[i][4] = stampPossetting();
                    stampList[i][5] = drawRect.Checked;
                    //does not need setting as comes from default setting
                    //in main call
                    //stampList[i][6] = statsBox.Enabled;
                    stampList[i][7] = statsChk.Checked;


                }

            }

        }


        private bool showStatsVal(string name)
        {

            foreach (List<object> item in stampList)
            {

                if (item[0].ToString() == name)
                {

                    return (bool)item[6];

                }

            }

            return true;

        }


        private void apply_Click(object sender, EventArgs e)
        {
            //ArrayList i = new ArrayList();
            //i.Add(fromString);

            ////i[1]
            //i.Add(addStamp.Checked);
            ////i[2]
            //i.Add(stampTypesetting());
            ////i[3]
            //i.Add(stampColoursetting());
            ////i[4]
            //if (tl.Checked) i.Add("tl");
            //if (tr.Checked) i.Add("tr");
            //if (bl.Checked) i.Add("bl");
            //if (br.Checked) i.Add("br");
            ////i[5]
            //i.Add(drawRect.Checked);
            ////i[6]
            //i.Add(statsChk.Checked);

            updateList(comboBox1.Text);
            timestampDelegate(stampList);
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private string stampColoursetting()
        {

            switch (stampColour.SelectedIndex)
            {

                case 1:
                    return "black";
                case 0:
                    return "red";
                case 2:
                    return "white";
                default:
                    return "black";
            }

        }

        private int stampColoursetting(string colour)
        {

            switch (colour.ToLower())
            {

                case "black":
                    return 1;
                case "red":
                    return 0;
                case "white":
                    return 2;
                default:
                    return 1;
            }

        }

        private string stampTypesetting()
        {

            switch (stampType.SelectedIndex)
            {

                case 1:
                    return "hhmm";
                case 0:
                    return "ddmmyy";
                case 2:
                    return "ddmmyyhhmm";
                case 3:
                    return "analogue";
                case 4:
                    return "analoguedate";
                default:
                    return "hhmm";
            }

        }

        private int stampTypesetting(string format)
        {

            switch (format.ToLower())
            {

                case "hhmm":
                    return 1;
                case "ddmmyy":
                    return 0;
                case "ddmmyyhhmm":
                    return 2;
                case "analogue":
                    return 3;
                case "analoguedate":
                    return 4;
                default:
                    return 1;
            }

        }


        private string stampPossetting()
        {

            if (tl.Checked) return "tl";
            if (tr.Checked) return "tr";
            if (bl.Checked) return "bl";
            if (br.Checked) return "br";

            return "tl";

        }




        private void timestamp_Load(object sender, EventArgs e)
        {

            //statsBox.Enabled = showStats;

            //label1.Text = fromString + " image";

            //addStamp.Checked = addTimeStamp;
            //groupBox1.Enabled = addStamp.Checked;
            //groupBox3.Enabled = addStamp.Checked;

            //stampColour.Enabled = addStamp.Checked;
            //stampType.Enabled = addStamp.Checked;


            //switch (inFormat)
            //{

            //    case "hhmm":
            //        stampType.SelectedIndex = 1;
            //        break;
            //    case "ddmmyy":
            //        stampType.SelectedIndex = 0;
            //        break;
            //    case "ddmmyyhhmm":
            //        stampType.SelectedIndex = 2;
            //        break;
            //    case "analogue":
            //        stampType.SelectedIndex = 3;
            //        break;
            //    default:
            //        stampType.SelectedIndex = 0;
            //        break;
            //}

            //switch (inColour)
            //{

            //    case "black":
            //        stampColour.SelectedIndex = 1;
            //        break;
            //    case "red":
            //        stampColour.SelectedIndex = 0;
            //        break;
            //    case "white":
            //        stampColour.SelectedIndex = 2;
            //        break;
            //    default:
            //        stampColour.SelectedIndex = 0;
            //        break;
            //}




            //tl.Checked = inPosition == "tl";
            //tr.Checked = inPosition == "tr";
            //bl.Checked = inPosition == "bl";
            //br.Checked = inPosition == "br";

            //drawRect.Checked = txtRect;

            //if (showStats)
            //{
            //    statsChk.Checked = includeStats;
            //}
            //else
            //{
            //    statsChk.Checked = false;
            //}

            toolTip1.Active = toolTip;

        }

        private void addStamp_CheckedChanged(object sender, EventArgs e)
        {

            groupBox1.Enabled = addStamp.Checked;
            groupBox3.Enabled = addStamp.Checked;
            stampColour.Enabled = addStamp.Checked;
            stampType.Enabled = addStamp.Checked;

            if (addStamp.Checked)
            {

                statsBox.Enabled = showStatsVal(comboBox1.Text);

            }
            else
            {

                statsBox.Enabled = false;

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!initializing) updateList(previousImage);
            previousImage = comboBox1.Text;
            populateFromList(comboBox1.Text);

        }




    }
}
