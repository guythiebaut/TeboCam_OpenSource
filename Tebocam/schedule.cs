using System;
using System.Collections;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class schedule : Form
    {
        private formDelegate scheduleDelegate;
        private string fromString;
        private bool toolTip;
        private string p_start;
        private string p_stop;
        private string o_start;
        private string o_stop;

        public schedule(formDelegate sender, ArrayList from)
        {
            scheduleDelegate = sender;
            fromString = from[0].ToString();
            toolTip = Convert.ToBoolean(from[1]);
            p_start = from[2].ToString();
            p_stop = from[3].ToString();

            InitializeComponent();
        }

        private void schedule_Load(object sender, EventArgs e)
        {

            lblTitle.Text = fromString + " Schedule";
            numericUpDown6.Value = Convert.ToDecimal(LeftRightMid.Left(p_start, 2));
            numericUpDown5.Value = Convert.ToDecimal(LeftRightMid.Right(p_start, 2));
            numericUpDown8.Value = Convert.ToDecimal(LeftRightMid.Left(p_stop, 2));
            numericUpDown7.Value = Convert.ToDecimal(LeftRightMid.Right(p_stop, 2));

            toolTip1.Active = toolTip;

        }


        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown6.Value == 24) { numericUpDown6.Value = 0; }
            if (numericUpDown6.Value == -1) { numericUpDown6.Value = 23; }

            o_start = numericUpDown6.Value.ToString().PadLeft(2, '0') + numericUpDown5.Value.ToString().PadLeft(2, '0');

        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown5.Value == 60) { numericUpDown5.Value = 0; }
            if (numericUpDown5.Value == -1) { numericUpDown5.Value = 59; }

            o_start = numericUpDown6.Value.ToString().PadLeft(2, '0') + numericUpDown5.Value.ToString().PadLeft(2, '0');

        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown8.Value == 24) { numericUpDown8.Value = 0; }
            if (numericUpDown8.Value == -1) { numericUpDown8.Value = 23; }

            o_stop = numericUpDown8.Value.ToString().PadLeft(2, '0') + numericUpDown7.Value.ToString().PadLeft(2, '0');

        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown7.Value == 60) { numericUpDown7.Value = 0; }
            if (numericUpDown7.Value == -1) { numericUpDown7.Value = 59; }

            o_stop = numericUpDown8.Value.ToString().PadLeft(2, '0') + numericUpDown7.Value.ToString().PadLeft(2, '0');

        }


        private void schedule_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (numericUpDown6.Value == 24) { numericUpDown6.Value = 0; }
            if (numericUpDown6.Value == -1) { numericUpDown6.Value = 23; }
            if (numericUpDown5.Value == 60) { numericUpDown5.Value = 0; }
            if (numericUpDown5.Value == -1) { numericUpDown5.Value = 59; }
            o_start = numericUpDown6.Value.ToString().PadLeft(2, '0') + numericUpDown5.Value.ToString().PadLeft(2, '0');

            if (numericUpDown8.Value == 24) { numericUpDown8.Value = 0; }
            if (numericUpDown8.Value == -1) { numericUpDown8.Value = 23; }
            if (numericUpDown7.Value == 60) { numericUpDown7.Value = 0; }
            if (numericUpDown7.Value == -1) { numericUpDown7.Value = 59; }

            o_stop = numericUpDown8.Value.ToString().PadLeft(2, '0') + numericUpDown7.Value.ToString().PadLeft(2, '0');


            ArrayList i = new ArrayList();

            i.Add(fromString);
            i.Add(o_start);
            i.Add(o_stop);

            scheduleDelegate(i); // This will call ReturnMethod in form1 and pass it val.

        }




    }
}