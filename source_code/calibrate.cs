using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace TeboCam
{
    public partial class calibrate : Form
    {
        private BackgroundWorker tw = new BackgroundWorker();
        private formDelegate calibrateDelegate;
        private bool toolTip = new bool();
        private int cam = new int();
        int CountDownFrom = new int();

        public analyse analysis = new analyse();


        public calibrate(formDelegate postCalibrate, ArrayList from)
        {
            InitializeComponent();
            lblCountDown.Text = string.Empty;
            calibrateDelegate = postCalibrate;
            toolTip = (bool)from[0];
            cam = (int)from[1];
            CameraRig.trainCam = cam;
            from.Clear();
            //string tmpStr = "www.teboweb.com/calibrate.html";
            //linkLightSpike.Links.Add(tmpStr);

            //System.Diagnostics.Debug.WriteLine("calibrate received: " + cam.ToString());

        }


        private void startCountdown_Click(object sender, EventArgs e)
        {
            if (!bubble.testImagePublish)
            {
                startCountdown.Text = "Stop Calibration";
                bubble.testImagePublishFirst = true;
                bubble.testImagePublish = true;
                lblCountDown.Visible = true;
                pnlControls.Controls.Clear();
                lblCountDown.Text = string.Empty;
                CountDownFrom = Convert.ToInt32(countVal.Text);

                tw.DoWork -= new DoWorkEventHandler(testMotion);
                tw.DoWork += new DoWorkEventHandler(testMotion);
                tw.WorkerSupportsCancellation = true;
                tw.RunWorkerAsync();
            }
            else
            {
                bubble.testImagePublish = false;
                startCountdown.Text = "Start Calibration";
                lblCountDown.Visible = false;
                tw.CancelAsync();
            }
        }


        private void testMotion(object sender, DoWorkEventArgs e)
        {


            string outFile = bubble.tmpFolder + "motionCalibrate.csv";

            if (File.Exists(outFile))
            {
                File.Delete(outFile);
            }

            int startSecs = time.secondsSinceStart();
            int tm = (int)(Convert.ToDouble(trainVal.Text) * Convert.ToDouble(1000));




            CameraRig.getCam(cam).detectionOn = true;
            CameraRig.getCam(cam).calibrating = true;
            //CameraRig.getCam(CameraRig.trainCam).detectionOn = true;
            //CameraRig.getCam(CameraRig.trainCam).calibrating = true;

            while (bubble.testImagePublish)
            {

                int timeLeft = CountDownFrom - (time.secondsSinceStart() - startSecs);

                lblCountDown.SynchronisedInvoke(() => lblCountDown.Text = "..." + timeLeft.ToString());

                if (timeLeft <= 0)//Convert.ToInt32(countVal.Text))
                {
                    bubble.testImagePublish = false;
                }

                //System.Diagnostics.Debug.WriteLine("sending to bubble: " + cam.ToString());
                bubble.publishTestMotion(tm, cam);

            }

            CameraRig.getCam(cam).detectionOn = false;
            CameraRig.getCam(cam).calibrating = false;
            //CameraRig.getCam(CameraRig.trainCam).detectionOn = false;
            //CameraRig.getCam(CameraRig.trainCam).calibrating = false;


            //analyse analysis = new analyse();


            StreamWriter sw = new StreamWriter(outFile, true);
            sw.WriteLine("Sequence,Motion_Level,lowestValueOverTime,Image_File");

            analysis.images.Clear();

            for (int i = 0; i < bubble.testImagePublishData.Count; i++)
            {

                if (!tw.CancellationPending && File.Exists(bubble.tmpFolder + (string)bubble.testImagePublishData[i + 3]))
                {

                    sw.WriteLine(string.Concat(bubble.testImagePublishData[i], ",",
                                 bubble.testImagePublishData[i + 1], ",",
                                 bubble.testImagePublishData[i + 2], ",",
                                 bubble.testImagePublishData[i + 3]));

                    analysis.newPictureControl(bubble.tmpFolder + (string)bubble.testImagePublishData[i + 3],
                               (string)bubble.testImagePublishData[i + 4],
                               (long)bubble.testImagePublishData[i + 5],
                               Color.DarkOrange,
                               (int)bubble.testImagePublishData[i + 1]);

                    //analysis.newPictureControl(new Bitmap(bubble.tmpFolder + (string)bubble.testImagePublishData[i + 3]),
                    //           (string)bubble.testImagePublishData[i + 4],
                    //           (long)bubble.testImagePublishData[i + 5],
                    //           Color.DarkOrange,
                    //           (int)bubble.testImagePublishData[i + 1]);

                }

                i = i + 5;

            }


            sw.Close();

            populate();

            startCountdown.SynchronisedInvoke(() => startCountdown.Text = "Start Calibration");
            lblCountDown.SynchronisedInvoke(() => lblCountDown.Visible = false);


        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void trainVal_Leave(object sender, EventArgs e)
        {
            trainVal.Text = bubble.verifyDouble(trainVal.Text, 0.25, 9999, "1");
        }

        private void countVal_Leave(object sender, EventArgs e)
        {
            countVal.Text = bubble.verifyInt(countVal.Text, 1, 9999, "1");
        }

        private void calibrate_Load(object sender, EventArgs e)
        {
            toolTip1.Active = toolTip;
        }

        private void calibrate_FormClosing(object sender, FormClosingEventArgs e)
        {
            bubble.testImagePublish = false;

            if (tw.IsBusy)
            {

                tw.CancelAsync();
                tw.Dispose();

            }

        }


        private void populate()
        {

            int lastX = new int();
            int lastY = new int();


            foreach (analysePictureControl item in analysis.images)
            {


                pnlControls.SynchronisedInvoke(() => pnlControls.Controls.Add(item));


                if (lastX + item.Width + item.Width + 5 > pnlControls.Width)
                {

                    int xPos = 5;
                    int yPos = lastY + item.Height + 5;

                    lastX = xPos;
                    lastY = yPos;



                    item.SynchronisedInvoke(() => item.Left = xPos);
                    item.SynchronisedInvoke(() => item.Top = yPos);

                }
                else
                {

                    int xPos;

                    if (lastX == 0)
                    {

                        xPos = 5;

                    }
                    else
                    {

                        xPos = lastX + item.Width + 5;

                    }

                    lastX = xPos;


                    item.SynchronisedInvoke(() => item.Left = xPos);
                    item.SynchronisedInvoke(() => item.Top = lastY);



                }


            }


        }


        private void analyseResults()
        {

            bool alarmed = new bool();


            foreach (analysePictureControl item in analysis.images)
            {

                alarmed = false;

                if (Convert.ToInt32(lblTimeSpike.Text) == 0 || Convert.ToInt32(lblToleranceSpike.Text) == 0)
                {

                    if (item.movLevel >= Convert.ToInt32(lblSensitivity.Text))
                    {

                        alarmed = true;
                        item.borderColour = Color.Red;
                        item.Refresh();

                    }


                }
                else
                {

                    if (item.movLevel >= trkMov.Value)
                    {

                        List<object> lightSpikeResults;

                        bool spike = new bool();
                        spike = false;
                        int spikePerc = new int();


                        lightSpikeResults = statistics.lightSpikeDetected(CameraRig.getCam(item.cam).camNo,
                                                                          item.movLevel,
                                                                          trkTimeSpike.Value,
                                                                          trkToleranceSpike.Value,
                                                                          bubble.profileInUse,
                                                                          item.time);

                        spike = (bool)lightSpikeResults[0];
                        spikePerc = (int)lightSpikeResults[1];

                        if (!spike)
                        {

                            alarmed = true;
                            item.borderColour = Color.Red;
                            item.Refresh();

                        }


                    }

                }


                if (!alarmed)
                {

                    item.borderColour = Color.Empty;
                    item.Refresh();

                }

            }

        }




        private void trkMov_Scroll(object sender, EventArgs e)
        {

            lblSensitivity.Text = trkMov.Value.ToString();
            analyseResults();

        }


        private void trkTimeSpike_Scroll(object sender, EventArgs e)
        {

            lblTimeSpike.Text = trkTimeSpike.Value.ToString();
            analyseResults();

        }

        private void trkToleranceSpike_Scroll(object sender, EventArgs e)
        {

            lblToleranceSpike.Text = trkToleranceSpike.Value.ToString();
            analyseResults();

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {

            ArrayList results = new ArrayList();

            results.Add(trkMov.Value);
            results.Add(trkTimeSpike.Value);
            results.Add(trkToleranceSpike.Value);
            calibrateDelegate(results);

        }

        private void linkLightSpike_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            bubble.openInternetBrowserAt("www.teboweb.com/calibrate.html");

        }




    }
}