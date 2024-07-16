using AForge.Vision.Motion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TeboCam
{
    public class CameraAlarm
    {
        ILog log;
        IException tebowebException;
        string thumbFolder;
        string tmbPrefix;
        ArrayList moveStats;
        static Dictionary<int, Int64> imageLastSaved = new Dictionary<int, long>();

        public CameraAlarm(ILog logger, IException except, string tmbPrfx, string thumbFld, ArrayList stats)
        {
            log = logger;
            tebowebException = except;
            thumbFolder = thumbFld;
            tmbPrefix = tmbPrfx;
            moveStats = stats;
        }

        public void Alarm(object sender, CamIdArgs e, LevelArgs l)
        {
            List<object> lightSpikeResults;
            bool spike = new bool();
            spike = false;
            int spikePerc = new int();

            //are we filtering light spikes?
            if (!CameraRig.ConnectedCameras[e.camNo].camera.triggeredBySpike
                && ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[e.camNo].cameraName).lightSpike)
            {
                //lightSpikeResults = statistics.lightSpikeDetected(e.camNo, l.lvl,
                //                                       config.GetCurrentProfile().timeSpike,
                //                                       config.GetCurrentProfile().toleranceSpike,
                //                                       TebocamState.profileInUse,
                //                                       time.millisecondsSinceStart());

                lightSpikeResults = statistics.lightSpikeDetected(e.camNo, l.lvl,
                    ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[e.camNo].cameraName).timeSpike,
                    ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[e.camNo].cameraName).toleranceSpike,
                    ConfigurationHelper.GetCurrentProfileName(),
                    time.millisecondsSinceStart());

                spike = (bool)lightSpikeResults[0];
                spikePerc = (int)lightSpikeResults[1];
            }

            //movement alarm was not previously triggered by a light spike
            //and a light spike has not been detected with the current alarm inducing movement  
            //or we are not concerned about light spikes
            if ((!CameraRig.ConnectedCameras[e.camNo].camera.triggeredBySpike && !spike)
                || CameraRig.ConnectedCameras[e.camNo].camera.certifiedTriggeredByNonSpike
                || !ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[e.camNo].cameraName).lightSpike)
            {
                CameraRig.ConnectedCameras[e.camNo].camera.certifiedTriggeredByNonSpike = true;

                if (ConfigurationHelper.GetCurrentProfile().areaOffAtMotion && !CameraRig.AreaOffAtMotionIsTriggeredCam(e.camNo))
                {
                    CameraRig.AreaOffAtMotionTrigger(e.camNo);
                }
                
                if (TebocamState.Alert.on && imageSaveTime(true, e.camNo))
                {
                    try
                    {
                        string fName = FileManager.fileNameSet(ConfigurationHelper.GetCurrentProfile().filenamePrefix,
                                                   ConfigurationHelper.GetCurrentProfile().cycleStampChecked,
                                                   ConfigurationHelper.GetCurrentProfile().startCycle,
                                                   ConfigurationHelper.GetCurrentProfile().endCycle,
                                                   ref ConfigurationHelper.GetCurrentProfile().currentCycle,
                                                   true,
                                                   ConfigurationHelper.GetCurrentProfile().includeMotionLevel,
                                                   CameraRig.ConnectedCameras[e.camNo].camera.MotionDetector.MotionDetectionAlgorithm.MotionLevel);

                        //20150110 Claudio asked for the possibility of not saving images
                        if (ConfigurationHelper.GetCurrentProfile().captureMovementImages)
                        {
                            Bitmap saveBmp = null;
                            var stampArgs = new Movement.imageText();
                            stampArgs.bitmap = (Bitmap)CameraRig.ConnectedCameras[e.camNo].camera.pubFrame.Clone();
                            stampArgs.type = "Alert";
                            stampArgs.backingRectangle = ConfigurationHelper.GetCurrentProfile().alertTimeStampRect;
                            stampArgs.position = ConfigurationHelper.GetCurrentProfile().alertTimeStampPosition;
                            stampArgs.format = ConfigurationHelper.GetCurrentProfile().alertTimeStampFormat;
                            stampArgs.colour = ConfigurationHelper.GetCurrentProfile().alertTimeStampColour;
                            saveBmp = ConfigurationHelper.GetCurrentProfile().alertTimeStamp ? ImageProcessor.TimeStampImage(stampArgs) : stampArgs.bitmap;
                            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters myEncoderParameters = new EncoderParameters(1);
                            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, ConfigurationHelper.GetCurrentProfile().alertCompression);
                            myEncoderParameters.Param[0] = myEncoderParameter;
                            saveBmp.Save(TebocamState.imageFolder + fName, jgpEncoder, myEncoderParameters);
                            Bitmap thumb = ImageProcessor.GetThumb(saveBmp);
                            thumb.Save(thumbFolder + tmbPrefix + fName, ImageFormat.Jpeg);
                            ImageThumbs.addThumbToPictureBox(thumbFolder + tmbPrefix + fName);
                            saveBmp.Dispose();
                            thumb.Dispose();
                            ImageSavedArgs a = new ImageSavedArgs();
                            a.image = fName;
                            Movement.imagesSaved.Add(fName);
                        }

                        Movement.updateSeq++;

                        if (Movement.updateSeq > 9999)
                        {
                            Movement.updateSeq = 1;
                        }

                        moveStatsAdd(time.currentTime());
                        log.AddLine("Movement detected");
                        log.AddLine("Movement level: " + l.lvl.ToString() + " spike perc.: " + Convert.ToString(spikePerc));

                        if (ConfigurationHelper.GetCurrentProfile().captureMovementImages)
                        {
                            log.AddLine("Image saved: " + fName);
                        }
                    }
                    catch (Exception ex)
                    {
                        TebocamState.tebowebException.LogException(ex);
                        log.AddLine("Error in saving movement image.");
                        Movement.updateSeq++;
                    }
                }
            }
            else
            {
                //a light spike caused this alarm and we are catching light spikes
                if (ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[e.camNo].cameraName).lightSpike)
                {
                    CameraRig.ConnectedCameras[e.camNo].camera.triggeredBySpike = true;
                }
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            if (codecs.Any(x => x.FormatID == format.Guid))
            {
                return codecs.First(x => x.FormatID == format.Guid);
            }

            return null;
        }

        public void moveStatsAdd(string time)
        {
            //"HHmm"
            //1245

            int hour = Convert.ToInt32(LeftRightMid.Left(time, 2));
            int cellIdx = Convert.ToInt32((int)Math.Floor((decimal)(hour / 2)));
            int cellVal = Convert.ToInt32(moveStats[cellIdx].ToString());
            moveStats[cellIdx] = cellVal + 1;
        }

        public static bool imageSaveTime(bool update, int camNo)
        {
            try
            {
                if (!imageLastSaved.Any(x => x.Key == camNo))
                {
                    imageLastSaved.Add(camNo, time.millisecondsSinceStart());
                    return true;
                }

                bool notify = (double)time.millisecondsSinceStart() - (double)imageLastSaved.First(x => x.Key == camNo).Value >= ConfigurationHelper.GetCurrentProfile().imageSaveInterval * 1000;
                if (update & notify) { imageLastSaved[camNo] = time.millisecondsSinceStart(); }
                return notify;
            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                imageLastSaved[camNo] = time.millisecondsSinceStart(); ;
                return true;
            }
        }
    }
}
