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
    public partial class train : Form
    {

        private formDelegate trainDelegate;
        private bool toolTip;
        private ArrayList returnList = new ArrayList();

        public train(formDelegate sender, ArrayList from)
        {
            trainDelegate = sender;
            toolTip = Convert.ToBoolean(from[0]);
            from.Clear();
            InitializeComponent();
        }

        private void startCountdown_Click(object sender, EventArgs e)
        {

            returnList.Clear();

            returnList.Add(Convert.ToInt32(countVal.Text));
            returnList.Add(Convert.ToInt32(trainVal.Text));

            trainDelegate(returnList);
            Close();

        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void countVal_TextChanged(object sender, EventArgs e)
        {
            countVal.Text = bubble.verifyInt(countVal.Text, 1, 9999, "10");
        }

        private void trainVal_TextChanged(object sender, EventArgs e)
        {
            trainVal.Text = bubble.verifyInt(trainVal.Text, 1, 9999, "10");
        }

        private void train_Load(object sender, EventArgs e)
        {
            toolTip1.Active = toolTip;
        }

    }
}