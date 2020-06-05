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
    public partial class image : Form
    {
        private formDelegate jpegDelegate;
        private string fromString;
        private int initialVal;
        private bool toolTip;

        public image(formDelegate sender, ArrayList from)
        {
            jpegDelegate = sender;
            fromString = from[0].ToString();
            initialVal = Convert.ToInt32(from[1]);
            toolTip = Convert.ToBoolean(from[2]);
            InitializeComponent();
        }

        private void image_Load(object sender, EventArgs e)
        {
            label1.Text = fromString + " image";
            trackBar1.Value = initialVal;
            val.Text = initialVal.ToString();
            toolTip1.Active = toolTip;
        }

        private void hideLog_Click(object sender, EventArgs e)
        {
            ArrayList i = new ArrayList();
            i.Add(fromString);
            i.Add(Convert.ToInt32(val.Text));
            jpegDelegate(i); // This will call ReturnMethod in form1 and pass it val.
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void val_TextChanged(object sender, EventArgs e)
        {
            val.Text = Valid.verifyInt(val.Text, 0, 100, "100");
            trackBar1.Value = Convert.ToInt32(val.Text);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            val.Text = trackBar1.Value.ToString();
        }
    }
}