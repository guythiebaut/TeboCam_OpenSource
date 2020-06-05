using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TeboCam
{

    public class ConnectedCamera
    {
        public string cameraName;
        public string friendlyName;
        public int displayButton;
        public bool selected;
        public Camera camera;
    }


    public class CameraRig
    {
        public static List<configApplication> profiles;
        public static List<ConnectedCamera> ConnectedCameras = new List<ConnectedCamera>();
        //public static List<cameraSpecificInfo> CameraSpecificInformation = new List<cameraSpecificInfo>();
        public static int CurrentlyDisplayingCamera = 0;
        public static int ConfigCam = 0;
        public static int TrainCam = 0;
        private static int infoIdx = -1;
        public static List<bool> camSel = new List<bool>();
        private const int CamLicense = 9;
        //public static bool reconfiguring = false;
        public static void camSelInit()
        {
            for (int i = 1; i < 10; i++)
            {
                camSel.Add(false);
            }
        }

        public static void cameraRemove(int camId, bool removeInfo)
        {
            if (removeInfo)
            {
                removeCameraSpecificInformation(ConfigurationHelper.GetCurrentProfileName(), ConnectedCameras[camId].cameraName);
            }
            ConnectedCameras[camId].camera.motionLevelEvent -= new motionLevelEventHandler(Movement.motionEvent);
            ConnectedCameras[camId].camera.SignalToStop();
            ConnectedCameras[camId].camera.WaitForStop();
            ConnectedCameras.RemoveAt(camId);
        }


        public static void clear()
        {
            CameraRig.ConnectedCameras.Clear();
        }



        public static void ConnectedCameraPopulateForCam(string profileName, string webcam)
        {
            ConnectedCamera selectedCamera = CameraRig.ConnectedCameras.Where(x => x.cameraName == webcam).FirstOrDefault();
            selectedCamera.friendlyName = ConfigurationHelper.InfoForProfileWebcam(profileName, webcam).friendlyName;
            selectedCamera.camera.areaDetection = ConfigurationHelper.InfoForProfileWebcam(profileName, webcam).areaDetection;
            selectedCamera.camera.areaDetectionWithin = ConfigurationHelper.InfoForProfileWebcam(profileName, webcam).areaDetectionWithin;
        }


        public static int idxFromButton(int bttn)
        {
            for (int i = 0; i < ConnectedCameras.Count; i++)
            {

                if (ConnectedCameras[i].displayButton == bttn)
                {
                    return i;
                }

            }

            return 0;

        }

        public static bool CameraIsConnectedToButton(int buttonNo)
        {
            return ConnectedCameras.Where(x => x.displayButton == buttonNo).Count() > 0;
        }

        /// <summary>
        /// swap camera buttons
        /// </summary>
        /// <returns>void</returns>
        public static void changeDisplayButton(string profileName, string camName, int id, int newBttn)
        {

            int swapId = 0;
            string swappingCamName = "";
            var profile = profiles.Find(x => x.profileName == profileName);//Select(x => x.camConfigs).First().Where(x=>x.displayButton==newBttn&& x.webcam!=camName).First();

            foreach (configWebcam infoI in profile.camConfigs)
            {
                //we have found this button is already assigned to another camera
                if (infoI.profileName == profileName && infoI.displayButton == newBttn && infoI.webcam != camName)
                {

                    swapId = ConnectedCameras[id].displayButton;
                    swappingCamName = infoI.webcam;
                    infoI.displayButton = swapId;

                }
            }

            foreach (configWebcam infoI in profile.camConfigs)
            {
                if (profileName == infoI.profileName && infoI.webcam == camName)
                {
                    infoI.displayButton = newBttn;
                }
            }

            foreach (ConnectedCamera item in CameraRig.ConnectedCameras)
            {
                if (item.cameraName == swappingCamName) item.displayButton = swapId;
                if (item.cameraName == camName) item.displayButton = newBttn;
            }
        }


        public static void removeCameraSpecificInformation(string profile, string webcamIdentifier)
        {
            profiles.Where(x => x.profileName == profile).First().camConfigs.RemoveAll(x => x.webcam == webcamIdentifier);
        }


        public static Camera getCam(string webcam)
        {

            foreach (ConnectedCamera item in CameraRig.ConnectedCameras)
            {

                if (item.cameraName == webcam) return item.camera;

            }

            return null;

        }

        public static Camera getCam(int cam)
        {
            return CameraRig.ConnectedCameras[cam].camera;
        }

        public static bool camerasAreConnected()
        {

            return ConnectedCameras.Count >= 1;

        }

        public static bool cameraExists(int id)
        {

            foreach (ConnectedCamera item in ConnectedCameras)
            {

                if (item.camera.camNo == id) return true;

            }

            return false;

        }


        public static int cameraCount()
        {
            return ConnectedCameras.Count;
        }

        public static int cameraCountZeroBased()
        {
            return ConnectedCameras.Count - 1;
        }

        public static void alert(bool alertOn)
        {
            foreach (ConnectedCamera rigI in ConnectedCameras)
            {
                rigI.camera.alert = alertOn;
                rigI.camera.detectionOn = alertOn;
            }
        }


        public static bool camerasAlreadySelected(string name)
        {

            if (!camerasAreConnected()) return false;

            foreach (ConnectedCamera rigI in ConnectedCameras)
            {
                if (rigI.cameraName == name)
                {
                    return true;
                }
            }

            return false;

        }



        public static void AreaOffAtMotionTrigger(int camId)
        {
            if (camerasAreConnected()) ConnectedCameras[camId].camera.areaOffAtMotionTriggered = true;
        }

        public static bool AreaOffAtMotionIsTriggeredCam(int camId)
        {
            if (camerasAreConnected())
            {
                return ConnectedCameras[camId].camera.areaOffAtMotionTriggered;
            }
            else
            {
                return false;
            }
        }


        public static bool Calibrating
        {
            get { return ConnectedCameras[CurrentlyDisplayingCamera].camera.calibrating; }
            set
            {
                if (camerasAreConnected()) ConnectedCameras[CurrentlyDisplayingCamera].camera.calibrating = value;
            }
        }

        public static bool AreaOffAtMotionTriggered
        {
            get { return ConnectedCameras[CurrentlyDisplayingCamera].camera.areaOffAtMotionTriggered; }
            set
            {
                if (camerasAreConnected()) ConnectedCameras[CurrentlyDisplayingCamera].camera.areaOffAtMotionTriggered = value;
            }
        }

        public static bool AreaOffAtMotionReset
        {
            get { return ConnectedCameras[CurrentlyDisplayingCamera].camera.areaOffAtMotionReset; }
            set
            {
                if (camerasAreConnected()) ConnectedCameras[CurrentlyDisplayingCamera].camera.areaOffAtMotionReset = value;
            }
        }

        public static bool AreaDetection
        {
            get { return ConnectedCameras[ConfigCam].camera.areaDetection; }
            set
            {
                if (camerasAreConnected()) ConnectedCameras[ConfigCam].camera.areaDetection = value;
            }
        }

        public static bool AreaDetectionWithin
        {
            get { return ConnectedCameras[ConfigCam].camera.areaDetectionWithin; }
            set
            {
                if (camerasAreConnected())
                    ConnectedCameras[ConfigCam].camera.areaDetectionWithin = value;
            }
        }

        public static bool ExposeArea
        {
            get { return ConnectedCameras[ConfigCam].camera.exposeArea; }
            set
            {
                if (camerasAreConnected()) ConnectedCameras[ConfigCam].camera.exposeArea = value;
            }
        }





    }






}

