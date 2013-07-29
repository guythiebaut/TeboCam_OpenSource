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
        static BackgroundWorker tw = new BackgroundWorker();
        private formDelegate calibrateDelegate;
        private static bool toolTip;
        private static int cam;


        public analyse analysis = new analyse();


        public calibrate(formDelegate sender, ArrayList from)
        {
            InitializeComponent();
            calibrateDelegate = sender;
            toolTip = (bool)from[0];
            cam = (int)from[1];
            from.Clear();
            //System.Diagnostics.Debug.WriteLine("calibrate received: " + cam.ToString());
        }


        private void startCountdown_Click(object sender, EventArgs e)
        {
            if (!bubble.testImagePublish)
            {
                startCountdown.Text = "Stop Calibration";
                bubble.testImagePublishFirst = true;
                bubble.testImagePublish = true;
                tw.DoWork -= new DoWorkEventHandler(testMotion);
                tw.DoWork += new DoWorkEventHandler(testMotion);
                tw.WorkerSupportsCancellation = true;
                tw.RunWorkerAsync();
            }
            else
            {
                bubble.testImagePublish = false;
                startCountdown.Text = "Start Calibration";
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



            CameraRig.getCam(CameraRig.trainCam).detectionOn = true;
            CameraRig.getCam(CameraRig.trainCam).calibrating = true;

            while (bubble.testImagePublish)
            {

                if (time.secondsSinceStart() - startSecs >= Convert.ToInt32(countVal.Text))
                {
                    bubble.testImagePublish = false;
                }

                //System.Diagnostics.Debug.WriteLine("sending to bubble: " + cam.ToString());
                bubble.publishTestMotion(tm, cam);

            }

            CameraRig.getCam(CameraRig.trainCam).detectionOn = false;
            CameraRig.getCam(CameraRig.trainCam).calibrating = false;


            //analyse analysis = new analyse();


            StreamWriter sw = new StreamWriter(outFile, true);
            sw.WriteLine("Sequence,Motion_Level,lowestValueOverTime,Image_File");

            for (int i = 0; i < bubble.testImagePublishData.Count; i++)
            {

                sw.WriteLine(string.Concat(bubble.testImagePublishData[i], ",",
                             bubble.testImagePublishData[i + 1], ",",
                             bubble.testImagePublishData[i + 2], ",",
                             bubble.testImagePublishData[i + 3]));

                analysis.newPictureControl(new Bitmap(bubble.tmpFolder + (string)bubble.testImagePublishData[i + 3]),
                                           (string)bubble.testImagePublishData[i + 4],
                                           (long)bubble.testImagePublishData[i + 5]);

                i = i + 5;

            }


            sw.Close();
            tw.Dispose();


            populate();


            //b            analyseMovement analysisForm = new analyseMovement(analysis);
            //            analysisForm.ShowDialog();



        }

        private void cancel_Click(object sender, EventArgs e)
        {
            bubble.testImagePublish = false;
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
                                        
                    

                    item.SynchronisedInvoke(()=>item.Left = xPos);
                    item.SynchronisedInvoke(()=>item.Top = yPos);

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


                    item.SynchronisedInvoke(()=>item.Left = xPos);
                    item.SynchronisedInvoke(()=>item.Top = lastY);

                                      

                }


            }


        }

        private void label4_Click(object sender, EventArgs e)
        {

        }



    }
}