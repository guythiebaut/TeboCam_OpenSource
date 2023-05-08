using AForge.Video.DirectShow;
using AForge.Vision.Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeboCam
{
    public class OpenVideo
    {
        public delegate void camButtonSetColoursDelegate();
        camButtonSetColoursDelegate camButtonSetColours;
        public delegate void webcamAttachedDelegate(bool a);
        webcamAttachedDelegate webcamAttached;
        public delegate void cameraSwitchDelegate(int a, bool b, bool c);
        cameraSwitchDelegate cameraSwitch;
        public delegate void btnConfigWebcamEnabledDelegate(bool a);
        btnConfigWebcamEnabledDelegate btnConfigWebcamEnabled;
        Configuration configuration;
        List<GroupCameraButton> NotConnectedCameras;
        Queue CommandQueue;
        CameraAlarm cameraAlarm;
        public delegate void publishOnDelegate(int buttonId);
        publishOnDelegate publishOn;

        public OpenVideo(CameraAlarm cameraAlarm, 
                         Configuration configuration, 
                         List<GroupCameraButton> NotConnectedCameras, 
                         Queue CommandQueue, 
                         camButtonSetColoursDelegate camButtonSetColours, 
                         cameraSwitchDelegate cameraSwitch, 
                         webcamAttachedDelegate webcamAttached, 
                         btnConfigWebcamEnabledDelegate btnConfigWebcamEnabled, 
                         publishOnDelegate publishOn)
        {
            this.cameraAlarm = cameraAlarm;
            this.configuration = configuration;
            this.NotConnectedCameras = NotConnectedCameras;
            this.CommandQueue = CommandQueue;
            this.camButtonSetColours = camButtonSetColours;
            this.cameraSwitch = cameraSwitch;
            this.webcamAttached = webcamAttached;
            this.btnConfigWebcamEnabled = btnConfigWebcamEnabled;
            this.publishOn = publishOn;
        }

        // Open video source
        //#ToDo check adding new cameras after removing
        public Camera OpenVideoSource(VideoCaptureDevice source, AForge.Video.MJPEGStream ipStream, Boolean ip, int cameraNo) //(VideoCaptureDevice source)
        {

            try
            {
                MotionDetector detector = new MotionDetector(new SimpleBackgroundModelingDetector());
                string camSource;

                // create camera
                Camera camera;

                if (!ip)
                {
                    camSource = source.Source;
                    camera = new Camera(source, detector, camSource);
                }
                else
                {
                    camSource = ipStream.Source;
                    camera = new Camera(ipStream, detector, camSource);
                }

                camera.tebowebException = TebocamState.tebowebException;
                camera.motionLevelEvent -= new motionLevelEventHandler(Movement.motionEvent);
                camera.motionLevelEvent += new motionLevelEventHandler(Movement.motionEvent);

                // start camera
                camera.Start();
                camera.ConnectedAt = DateTime.Now;
                var cameraExistsUnderProfile = configuration.appConfigs.First(x => x.profileName == ConfigurationHelper.GetCurrentProfileName()).camConfigs.Any(x => x.webcam == camSource);

                configWebcam configForWebcam;
                if (cameraExistsUnderProfile)
                {
                    configForWebcam = configuration.appConfigs.First(x => x.profileName == ConfigurationHelper.GetCurrentProfileName()).camConfigs.First(x => x.webcam == camSource);
                }
                else
                {
                    configForWebcam = new configWebcam();
                    configuration.appConfigs.First(x => x.profileName == ConfigurationHelper.GetCurrentProfileName()).camConfigs
                        .Add(configForWebcam);
                }

                var connectedCamera = new ConnectedCamera();
                CameraRig.ConnectedCameras.Add(connectedCamera);
                connectedCamera.cameraName = camSource;
                connectedCamera.friendlyName = configForWebcam.friendlyName;
                connectedCamera.camera = camera;
                connectedCamera.camera.camNo = cameraNo == -1 ? CameraRig.cameraCountZeroBased() : cameraNo;
                connectedCamera.camera.movementVal = configForWebcam.movementVal;
                configForWebcam.profileName = ConfigurationHelper.GetCurrentProfileName();
                configForWebcam.webcam = camSource;
                connectedCamera.friendlyName = configForWebcam.friendlyName;
                connectedCamera.displayButton = configForWebcam.displayButton;
                connectedCamera.camera.areaDetectionWithin = configForWebcam.areaDetectionWithin;
                connectedCamera.camera.areaDetection = configForWebcam.areaDetection;
                connectedCamera.camera.rectX = configForWebcam.rectX;
                connectedCamera.camera.rectY = configForWebcam.rectY;
                connectedCamera.camera.rectHeight = configForWebcam.rectHeight;
                connectedCamera.camera.rectWidth = configForWebcam.rectWidth;
                connectedCamera.camera.movementVal = configForWebcam.movementVal;
                connectedCamera.camera.motionAlarm -= new alarmEventHandler(cameraAlarm.Alarm);
                connectedCamera.camera.motionAlarm += new alarmEventHandler(cameraAlarm.Alarm);
                connectedCamera.camera.publishActive = configForWebcam.publishActive;

                CameraRig.CurrentlyDisplayingCamera = CameraRig.cameraCountZeroBased();
                ConfigurationHelper.GetCurrentProfile().webcam = camSource;

                Queue.QueueItem queueItem =
                    CommandQueue.QueueItems.FirstOrDefault(x => x.Instruction == "selcam" && x.Parms[0] == "all");
                if (queueItem != null && !queueItem.CamsProcessed.Contains(connectedCamera.camera.camNo))
                {
                    //selcam(connectedCamera.cam.camNo, true);
                    NotConnectedCameras.First(x => x.id == connectedCamera.displayButton).ActiveButtonIsActive();
                    CameraRig.ConnectedCameras[connectedCamera.camera.camNo].camera.alert = true;
                    CameraRig.ConnectedCameras[connectedCamera.camera.camNo].camera.alarmActive = true;
                    ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(),CameraRig.ConnectedCameras[connectedCamera.camera.camNo].cameraName).alarmActive = true;
                    CameraRig.ConnectedCameras[connectedCamera.camera.camNo].camera.detectionOn = true;
                    //CameraRig.alert(true);
                    queueItem.CamsProcessed.Add(connectedCamera.camera.camNo);
                }

                //get desired button or first available button
                //int desiredButton = CameraRig.ConnectedCameras[newCameraIdx].displayButton;
                //check if the desxt frired button is free and return the next button if one is available

                var firstFreeButton = NotConnectedCameras.FirstOrDefault(x =>
                    x.CameraButtonState == GroupCameraButton.ButtonState.NotConnected);
                int camButton = firstFreeButton != null ? firstFreeButton.id : 999;
                bool freeCamsExist = camButton != 999;

                //if a free camera button exists assign the camera
                if (freeCamsExist)
                {
                    connectedCamera.displayButton = camButton;
                }

                //update info for camera
                ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), ConfigurationHelper.GetCurrentProfile().webcam)
                    .displayButton = camButton;

                camButtonSetColours();
                // the false refresh option is important here otherwise we get an exception thrown 
                //and any other commands from here are not executed
                cameraSwitch(camButton, false, false);
                CameraRig.alert(TebocamState.Alert.on);
                connectedCamera.camera.exposeArea = false;
                webcamAttached(true);
                btnConfigWebcamEnabled(CameraRig.camerasAreConnected());

                //var publishActive = ConfigurationHelper.InfoForProfileWebcam(ConfigurationHelper.GetCurrentProfileName(), CameraRig.ConnectedCameras[connectedCamera.camera.camNo].cameraName).publishActive;
                //if (publishActive)
                //{
                //    publishOn(camButton);
                //}

                return camera;
            }
            catch (Exception ex)
            {
                TebocamState.tebowebException.LogException(ex);
                return null;
            }

        }
    }
}
