using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TeboCam
{
    public partial class News : Form
    {

        public static ArrayList newsTxt;
        public static ArrayList infoTxt;
        public static ArrayList whatsNewTxt;
        public static ArrayList licenseTxt;

        public News(ArrayList newsIn, ArrayList infoIn, ArrayList whatsNewIn, ArrayList licenseIn)
        {
            InitializeComponent();


            var version = Double.Parse(sensitiveInfo.ver, new System.Globalization.CultureInfo("en-GB")).ToString();
            this.Text += "(version " + version + ")";

            newsTxt = newsIn;
            infoTxt = infoIn;
            whatsNewTxt = whatsNewIn;
            licenseTxt = licenseIn;

            foreach (string line in newsIn)
            {
                info.Text = info.Text + line + "\n";
            }

            info.Invalidate();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ArrayList tmpStr;

            if (radioButton1.Checked)
            {
                tmpStr = newsTxt;
                info.Clear();

                foreach (string line in tmpStr)
                {
                    info.Text = info.Text + line + "\n";
                }

                info.Invalidate();
            }



        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

            ArrayList tmpStr;

            if (radioButton3.Checked)
            {
                tmpStr = whatsNewTxt;
                info.Clear();

                foreach (string line in tmpStr)
                {
                    info.Text = info.Text + line + "\n";
                }

                info.Invalidate();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            ArrayList tmpStr;

            if (radioButton2.Checked)
            {
                tmpStr = infoTxt;

                info.Clear();

                foreach (string line in tmpStr)
                {
                    info.Text = info.Text + line + "\n";
                }

                info.Invalidate();
            }

        }


        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

            ArrayList tmpStr;

            if (radioButton4.Checked)
            {
                tmpStr = licenseTxt;

                info.Clear();

                foreach (string line in tmpStr)
                {
                    info.Text = info.Text + line + "\n";
                }

                info.Invalidate();
            }

        }


        private void info_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Internet.openInternetBrowserAt(e.LinkText);
        }

    }
}