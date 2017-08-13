using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TeboCam
{

    class ConnectedCamera
    {

        public string cameraName;
        public string friendlyName;
        public int displayButton;
        public Camera cam;

    }


    class InfoItem
    {

        private object _value;
        public string name;
        public string profile;

        public object value
        {

            set { _value = value; }

            get
            {

                if (name == CameraRig.infoEnum.alarmActive.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.webcam.ToString()) { return (string)_value; }
                if (name == CameraRig.infoEnum.friendlyName.ToString()) { return (string)_value; }
                if (name == CameraRig.infoEnum.areaDetection.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.areaDetectionWithin.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.areaOffAtMotion.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.alarmActive.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.publishActive.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.rectX.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.rectY.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.rectWidth.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.rectHeight.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.displayButton.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.movementVal.ToString()) { return (double)_value; }
                if (name == CameraRig.infoEnum.timeSpike.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.toleranceSpike.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.lightSpike.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.pubImage.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.pubTime.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.pubHours.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.pubMins.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.pubSecs.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.publishWeb.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.publishLocal.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.timerOn.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.fileURLPubWeb.ToString()) { return (string)_value; }
                if (name == CameraRig.infoEnum.filenamePrefixPubWeb.ToString()) { return (string)_value; }
                if (name == CameraRig.infoEnum.cycleStampCheckedPubWeb.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.startCyclePubWeb.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.endCyclePubWeb.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.currentCyclePubWeb.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.stampAppendPubWeb.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.fileDirAlertLoc.ToString()) { return (string)_value; }
                if (name == CameraRig.infoEnum.fileDirAlertCust.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.fileDirPubLoc.ToString()) { return (string)_value; }
                if (name == CameraRig.infoEnum.fileDirPubCust.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.filenamePrefixPubLoc.ToString()) { return (string)_value; }
                if (name == CameraRig.infoEnum.cycleStampCheckedPubLoc.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.startCyclePubLoc.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.endCyclePubLoc.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.currentCyclePubLoc.ToString()) { return (int)_value; }
                if (name == CameraRig.infoEnum.stampAppendPubLoc.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.ipWebcamAddress.ToString()) { return (string)_value; }
                if (name == CameraRig.infoEnum.ipWebcamUser.ToString()) { return (string)_value; }
                if (name == CameraRig.infoEnum.ipWebcamPassword.ToString()) { return (string)_value; }
                if (name == CameraRig.infoEnum.publishFirst.ToString()) { return (bool)_value; }
                if (name == CameraRig.infoEnum.lastPublished.ToString()) { return (int)_value; }

                return null;

            }


        }


        public InfoItem(object _val, string _name, string _profile)
        {

            value = _val;
            name = _name;
            profile = _profile;

        }



    }

    class cameraSpecificInfo
    {

        public string profileName = "";
        public string webcam = "";

        public string friendlyName = "";
        public bool areaDetection = false;
        public bool areaDetectionWithin = false;
        public bool areaOffAtMotion = false;
        public bool alarmActive = false;
        public bool publishActive = false;
        public int rectX = 20;
        public int rectY = 20;
        public int rectWidth = 80;
        public int rectHeight = 80;
        public int displayButton = 1;
        public double movementVal = 0.99;
        public int timeSpike = 100;
        public int toleranceSpike = 50;
        public bool lightSpike = false;


        public bool pubImage = false;
        public int pubTime = 2;
        public bool pubHours = false;
        public bool pubMins = true;
        public bool pubSecs = false;
        public bool publishWeb = false;
        public bool publishLocal = false;
        public bool timerOn = false;

        public string fileURLPubWeb = "";
        public string filenamePrefixPubWeb = "webcamPublish";
        public int cycleStampCheckedPubWeb = 1;
        public int startCyclePubWeb = 1;
        public int endCyclePubWeb = 999;
        public int currentCyclePubWeb = 1;
        public bool stampAppendPubWeb = false;

        public string fileDirAlertLoc = bubble.imageFolder;
        public bool fileDirAlertCust = false;
        public string fileDirPubLoc = bubble.imageFolder;
        public bool fileDirPubCust = false;
        public string filenamePrefixPubLoc = "webcamPublish";
        public int cycleStampCheckedPubLoc = 1;
        public int startCyclePubLoc = 1;
        public int endCyclePubLoc = 999;
        public int currentCyclePubLoc = 1;
        public bool stampAppendPubLoc = false;

        public string ipWebcamAddress = "";
        public string ipWebcamUser = "";
        public string ipWebcamPassword = "";

        //public bool StatsToFileOn = false;
        //public string StatsToFileLocation = string.Empty;
        //public bool StatsToFileTimeStamp;
        //public double StatsToFileMb = 10;



        //for monitoring publishing - does not need to be saved to xml file
        public bool publishFirst = true;
        public int lastPublished = 0;
        //for monitoring publishing - does not need to be saved to xml file        


    }


    class CameraRig
    {

        public enum infoEnum
        {

            webcam,
            profileName,
            friendlyName,
            areaDetection,
            areaDetectionWithin,
            areaOffAtMotion,
            rectX,
            rectY,
            rectWidth,
            rectHeight,
            movementVal,
            timeSpike,
            toleranceSpike,
            lightSpike,
            displayButton,
            alarmActive,
            publishActive,
            pubImage,
            pubTime,
            pubHours,
            pubMins,
            pubSecs,
            publishWeb,
            publishLocal,
            timerOn,
            fileURLPubWeb,
            filenamePrefixPubWeb,
            cycleStampCheckedPubWeb,
            startCyclePubWeb,
            endCyclePubWeb,
            currentCyclePubWeb,
            stampAppendPubWeb,
            fileDirPubLoc,
            fileAlertPubLoc,
            fileAlertPubCust,
            filenamePrefixPubLoc,
            fileDirPubCust,
            cycleStampCheckedPubLoc,
            startCyclePubLoc,
            endCyclePubLoc,
            currentCyclePubLoc,
            stampAppendPubLoc,
            publishFirst,
            lastPublished,
            ipWebcamAddress,
            ipWebcamUser,
            ipWebcamPassword,

            fileDirAlertLoc,
            fileDirAlertCust

        };




        public static List<ConnectedCamera> ConnectedCameras = new List<ConnectedCamera>();
        public static List<cameraSpecificInfo> CamsInfo = new List<cameraSpecificInfo>();
        public static int activeCam = 0;
        public static int drawCam = 0;
        public static int trainCam = 0;
        private static int infoIdx = -1;
        public static List<bool> camSel = new List<bool>();
        private const int camLicense = 9;
        //public static bool reconfiguring = false;


        public static void camSelInit()
        {
            for (int i = 1; i < 10; i++)
            {
                camSel.Add(false);
            }
        }

        public static void renameProfile(string oldProfile, string newProfile)
        {
            CamsInfo.Where(x => x.profileName == oldProfile).FirstOrDefault().profileName = newProfile;
        }

        public static void cameraRemove(int camId)
        {
            ConnectedCameras[camId].cam.motionLevelEvent -= new motionLevelEventHandler(bubble.motionEvent);
            ConnectedCameras[camId].cam.SignalToStop();
            ConnectedCameras[camId].cam.WaitForStop();
            ConnectedCameras.RemoveAt(camId);
        }

        public static List<cameraSpecificInfo> cameraCredentialsListedUnderProfile(string profileName)
        {
            return CamsInfo.Where(x => x.profileName == profileName).ToList();
        }

        public static void clear()
        {
            CameraRig.ConnectedCameras.Clear();
        }

        public static void updateInfo(string profileName, string webcam, infoEnum infoType, object val)
        {

            if (camerasAreConnected())
            {

                foreach (cameraSpecificInfo infoI in CamsInfo)
                {

                    if (infoI.profileName == profileName && infoI.webcam == webcam)
                    {

                        if (infoType == infoEnum.friendlyName) { infoI.friendlyName = (string)val; }

                        if (infoType == infoEnum.areaDetection) { infoI.areaDetection = (bool)val; }
                        if (infoType == infoEnum.areaDetectionWithin) { infoI.areaDetectionWithin = (bool)val; }
                        if (infoType == infoEnum.alarmActive) { infoI.alarmActive = (bool)val; }
                        if (infoType == infoEnum.publishActive) { infoI.publishActive = (bool)val; }


                        //may be of use in future
                        if (infoType == infoEnum.areaOffAtMotion) { infoI.areaOffAtMotion = (bool)val; }
                        //may be of use in future

                        if (infoType == infoEnum.rectX) { infoI.rectX = (int)val; }
                        if (infoType == infoEnum.rectY) { infoI.rectY = (int)val; }
                        if (infoType == infoEnum.rectWidth) { infoI.rectWidth = (int)val; }
                        if (infoType == infoEnum.rectHeight) { infoI.rectHeight = (int)val; }
                        if (infoType == infoEnum.movementVal) { infoI.movementVal = (double)val; }
                        if (infoType == infoEnum.timeSpike) { infoI.timeSpike = (int)val; }
                        if (infoType == infoEnum.toleranceSpike) { infoI.toleranceSpike = (int)val; }
                        if (infoType == infoEnum.lightSpike) { infoI.lightSpike = (bool)val; }
                        if (infoType == infoEnum.displayButton) { infoI.displayButton = (int)val; }

                        if (infoType == infoEnum.pubImage) { infoI.pubImage = (bool)val; }
                        if (infoType == infoEnum.pubTime) { infoI.pubTime = (int)val; }
                        if (infoType == infoEnum.pubHours) { infoI.pubHours = (bool)val; }
                        if (infoType == infoEnum.pubMins) { infoI.pubMins = (bool)val; }
                        if (infoType == infoEnum.pubSecs) { infoI.pubSecs = (bool)val; }
                        if (infoType == infoEnum.publishWeb) { infoI.publishWeb = (bool)val; }
                        if (infoType == infoEnum.publishLocal) { infoI.publishLocal = (bool)val; }
                        if (infoType == infoEnum.timerOn) { infoI.timerOn = (bool)val; }
                        if (infoType == infoEnum.fileURLPubWeb) { infoI.fileURLPubWeb = (string)val; }
                        if (infoType == infoEnum.filenamePrefixPubWeb) { infoI.filenamePrefixPubWeb = (string)val; }
                        if (infoType == infoEnum.cycleStampCheckedPubWeb) { infoI.cycleStampCheckedPubWeb = (int)val; }
                        if (infoType == infoEnum.startCyclePubWeb) { infoI.startCyclePubWeb = (int)val; }
                        if (infoType == infoEnum.endCyclePubWeb) { infoI.endCyclePubWeb = (int)val; }
                        if (infoType == infoEnum.currentCyclePubWeb) { infoI.currentCyclePubWeb = (int)val; }
                        if (infoType == infoEnum.stampAppendPubWeb) { infoI.stampAppendPubWeb = (bool)val; }
                        if (infoType == infoEnum.fileDirPubLoc) { infoI.fileDirPubLoc = (string)val; }
                        if (infoType == infoEnum.filenamePrefixPubLoc) { infoI.filenamePrefixPubLoc = (string)val; }
                        if (infoType == infoEnum.fileDirPubCust) { infoI.fileDirPubCust = (bool)val; }
                        if (infoType == infoEnum.cycleStampCheckedPubLoc) { infoI.cycleStampCheckedPubLoc = (int)val; }
                        if (infoType == infoEnum.startCyclePubLoc) { infoI.startCyclePubLoc = (int)val; }
                        if (infoType == infoEnum.endCyclePubLoc) { infoI.endCyclePubLoc = (int)val; }
                        if (infoType == infoEnum.currentCyclePubLoc) { infoI.currentCyclePubLoc = (int)val; }
                        if (infoType == infoEnum.stampAppendPubLoc) { infoI.stampAppendPubLoc = (bool)val; }

                        if (infoType == infoEnum.publishFirst) { infoI.publishFirst = (bool)val; }
                        if (infoType == infoEnum.lastPublished) { infoI.lastPublished = (int)val; }

                        if (infoType == infoEnum.ipWebcamAddress) { infoI.ipWebcamAddress = (string)val; }
                        if (infoType == infoEnum.ipWebcamUser) { infoI.ipWebcamUser = (string)val; }
                        if (infoType == infoEnum.ipWebcamPassword) { infoI.ipWebcamPassword = (string)val; }


                    }

                }

            }

        }



        public static void addInfo(infoEnum infoType, object val)
        {


            if (infoType == infoEnum.webcam)
            {

                //newcode
                //rigItem rig_item = new rigItem();
                //rig_item.cameraName = (string)val;
                //rig.Add(rig_item);
                //newcode

                infoIdx++;
                cameraSpecificInfo p_item = new cameraSpecificInfo();
                CamsInfo.Add(p_item);
                CamsInfo[infoIdx].webcam = (string)val;

            }

            if (CamsInfo.Count > 0)
            {


                if (infoType == infoEnum.profileName) { CamsInfo[infoIdx].profileName = (string)val; }
                if (infoType == infoEnum.friendlyName) { CamsInfo[infoIdx].friendlyName = (string)val; }

                if (infoType == infoEnum.areaDetection) { CamsInfo[infoIdx].areaDetection = (bool)val; }
                if (infoType == infoEnum.areaDetectionWithin) { CamsInfo[infoIdx].areaDetectionWithin = (bool)val; }


                if (infoType == infoEnum.alarmActive) { CamsInfo[infoIdx].alarmActive = (bool)val; }
                if (infoType == infoEnum.publishActive) { CamsInfo[infoIdx].publishActive = (bool)val; }


                //may be of use in future
                if (infoType == infoEnum.areaOffAtMotion) { CamsInfo[infoIdx].areaOffAtMotion = (bool)val; }
                //may be of use in future

                if (infoType == infoEnum.rectX) { CamsInfo[infoIdx].rectX = (int)val; }
                if (infoType == infoEnum.rectY) { CamsInfo[infoIdx].rectY = (int)val; }
                if (infoType == infoEnum.rectWidth) { CamsInfo[infoIdx].rectWidth = (int)val; }
                if (infoType == infoEnum.rectHeight) { CamsInfo[infoIdx].rectHeight = (int)val; }
                if (infoType == infoEnum.movementVal) { CamsInfo[infoIdx].movementVal = (double)val; }
                if (infoType == infoEnum.timeSpike) { CamsInfo[infoIdx].timeSpike = (int)val; }
                if (infoType == infoEnum.toleranceSpike) { CamsInfo[infoIdx].toleranceSpike = (int)val; }
                if (infoType == infoEnum.lightSpike) { CamsInfo[infoIdx].lightSpike = (bool)val; }
                if (infoType == infoEnum.displayButton) { CamsInfo[infoIdx].displayButton = (int)val; }



                if (infoType == infoEnum.pubImage) { CamsInfo[infoIdx].pubImage = (bool)val; }
                if (infoType == infoEnum.pubTime) { CamsInfo[infoIdx].pubTime = (int)val; }
                if (infoType == infoEnum.pubHours) { CamsInfo[infoIdx].pubHours = (bool)val; }
                if (infoType == infoEnum.pubMins) { CamsInfo[infoIdx].pubMins = (bool)val; }
                if (infoType == infoEnum.pubSecs) { CamsInfo[infoIdx].pubSecs = (bool)val; }
                if (infoType == infoEnum.publishWeb) { CamsInfo[infoIdx].publishWeb = (bool)val; }
                if (infoType == infoEnum.publishLocal) { CamsInfo[infoIdx].publishLocal = (bool)val; }
                if (infoType == infoEnum.timerOn) { CamsInfo[infoIdx].timerOn = (bool)val; }
                if (infoType == infoEnum.fileURLPubWeb) { CamsInfo[infoIdx].fileURLPubWeb = (string)val; }
                if (infoType == infoEnum.filenamePrefixPubWeb) { CamsInfo[infoIdx].filenamePrefixPubWeb = (string)val; }
                if (infoType == infoEnum.cycleStampCheckedPubWeb) { CamsInfo[infoIdx].cycleStampCheckedPubWeb = (int)val; }
                if (infoType == infoEnum.startCyclePubWeb) { CamsInfo[infoIdx].startCyclePubWeb = (int)val; }
                if (infoType == infoEnum.endCyclePubWeb) { CamsInfo[infoIdx].endCyclePubWeb = (int)val; }
                if (infoType == infoEnum.currentCyclePubWeb) { CamsInfo[infoIdx].currentCyclePubWeb = (int)val; }
                if (infoType == infoEnum.stampAppendPubWeb) { CamsInfo[infoIdx].stampAppendPubWeb = (bool)val; }
                if (infoType == infoEnum.fileDirPubLoc) { CamsInfo[infoIdx].fileDirPubLoc = (string)val; }
                if (infoType == infoEnum.filenamePrefixPubLoc) { CamsInfo[infoIdx].filenamePrefixPubLoc = (string)val; }
                if (infoType == infoEnum.fileDirPubCust) { CamsInfo[infoIdx].fileDirPubCust = (bool)val; }
                if (infoType == infoEnum.cycleStampCheckedPubLoc) { CamsInfo[infoIdx].cycleStampCheckedPubLoc = (int)val; }
                if (infoType == infoEnum.startCyclePubLoc) { CamsInfo[infoIdx].startCyclePubLoc = (int)val; }
                if (infoType == infoEnum.endCyclePubLoc) { CamsInfo[infoIdx].endCyclePubLoc = (int)val; }
                if (infoType == infoEnum.currentCyclePubLoc) { CamsInfo[infoIdx].currentCyclePubLoc = (int)val; }
                if (infoType == infoEnum.stampAppendPubLoc) { CamsInfo[infoIdx].stampAppendPubLoc = (bool)val; }

                if (infoType == infoEnum.publishFirst) { CamsInfo[infoIdx].publishFirst = (bool)val; }
                if (infoType == infoEnum.lastPublished) { CamsInfo[infoIdx].lastPublished = (int)val; }


                if (infoType == infoEnum.ipWebcamAddress) { CamsInfo[infoIdx].ipWebcamAddress = (string)val; }
                if (infoType == infoEnum.ipWebcamUser) { CamsInfo[infoIdx].ipWebcamUser = (string)val; }
                if (infoType == infoEnum.ipWebcamPassword) { CamsInfo[infoIdx].ipWebcamPassword = (string)val; }

            }


        }



        public static void rigInfoPopulateForCam(string profileName, string webcam)
        {
            ConnectedCamera selectedCamera = CameraRig.ConnectedCameras.Where(x => x.cameraName == webcam).FirstOrDefault();
            selectedCamera.friendlyName = (string)(CameraRig.rigInfoGet(profileName, webcam, infoEnum.friendlyName));
            selectedCamera.cam.areaDetection = (bool)(CameraRig.rigInfoGet(profileName, webcam, infoEnum.areaDetection));
            selectedCamera.cam.areaDetectionWithin = (bool)(CameraRig.rigInfoGet(profileName, webcam, infoEnum.areaDetectionWithin));

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



        /// <summary>
        /// swap camera buttons
        /// </summary>
        /// <returns>void</returns>
        public static void changeDisplayButton(string profileName, string camName, int id, int newBttn)
        {

            int swapId = 0;
            string swappingCamName = "";

            foreach (cameraSpecificInfo infoI in CamsInfo)
            {

                //we have found this button is already assigned to another camera
                if (infoI.profileName == profileName && infoI.displayButton == newBttn && infoI.webcam != camName)
                {

                    swapId = ConnectedCameras[id].displayButton;
                    swappingCamName = infoI.webcam;
                    infoI.displayButton = swapId;

                }

            }

            foreach (cameraSpecificInfo infoI in CamsInfo)
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


        public static void rigInfoPopulate(string profileName, int id)
        {
            bool infoExists = false;

            foreach (cameraSpecificInfo infoI in CamsInfo)
            {

                if (profileName == infoI.profileName && infoI.webcam == ConnectedCameras[id].cameraName)
                {

                    ConnectedCameras[id].friendlyName = infoI.friendlyName;
                    ConnectedCameras[id].displayButton = infoI.displayButton;


                    ConnectedCameras[id].cam.areaDetectionWithin = infoI.areaDetectionWithin;
                    ConnectedCameras[id].cam.areaDetection = infoI.areaDetection;

                    ConnectedCameras[id].cam.rectX = infoI.rectX;
                    ConnectedCameras[id].cam.rectY = infoI.rectY;
                    ConnectedCameras[id].cam.rectHeight = infoI.rectHeight;
                    ConnectedCameras[id].cam.rectWidth = infoI.rectWidth;

                    ConnectedCameras[id].cam.movementVal = infoI.movementVal;
                    //rig[id].cam.timeSpike = infoI.timeSpike;
                    //rig[id].cam.toleranceSpike = infoI.toleranceSpike;



                    ConnectedCameras[id].cam.alarmActive = infoI.alarmActive;
                    ConnectedCameras[id].cam.publishActive = infoI.publishActive;

                    infoExists = true;

                    break;

                }

            }

            //create the info
            if (!infoExists)
            {

                cameraSpecificInfo infoI = new cameraSpecificInfo();

                infoI.profileName = profileName;
                infoI.webcam = ConnectedCameras[id].cameraName;
                CamsInfo.Add(infoI);

                ConnectedCameras[id].displayButton = infoI.displayButton;

                ConnectedCameras[id].cam.areaDetectionWithin = infoI.areaDetectionWithin;
                ConnectedCameras[id].cam.areaDetection = infoI.areaDetection;
                ConnectedCameras[id].cam.rectX = infoI.rectX;
                ConnectedCameras[id].cam.rectY = infoI.rectY;
                ConnectedCameras[id].cam.rectHeight = infoI.rectHeight;
                ConnectedCameras[id].cam.rectWidth = infoI.rectWidth;


                ConnectedCameras[id].cam.movementVal = infoI.movementVal;
                //rig[id].cam.timeSpike = infoI.timeSpike;
                //rig[id].cam.toleranceSpike = infoI.toleranceSpike;


            }

        }


        public static void framerateGet()
        {

            foreach (var item in ConnectedCameras)
            {

            }



        }


        public static object rigInfoGet(string profile, string webcamIdentifier, infoEnum property)
        {
            cameraSpecificInfo infoI = CamsInfo.Where(x => x.profileName == profile && x.webcam == webcamIdentifier).FirstOrDefault();

            if (property == infoEnum.friendlyName) return infoI.friendlyName;
            if (property == infoEnum.areaDetection) return infoI.areaDetection;
            if (property == infoEnum.areaDetectionWithin) return infoI.areaDetectionWithin;
            if (property == infoEnum.areaOffAtMotion) return infoI.areaOffAtMotion;
            if (property == infoEnum.rectX) return infoI.rectX;
            if (property == infoEnum.rectY) return infoI.rectY;
            if (property == infoEnum.rectWidth) return infoI.rectWidth;
            if (property == infoEnum.rectHeight) return infoI.rectHeight;
            if (property == infoEnum.movementVal) return infoI.movementVal;
            if (property == infoEnum.timeSpike) return infoI.timeSpike;
            if (property == infoEnum.toleranceSpike) return infoI.toleranceSpike;
            if (property == infoEnum.lightSpike) return infoI.lightSpike;
            if (property == infoEnum.alarmActive) return infoI.alarmActive;
            if (property == infoEnum.publishActive) return infoI.publishActive;
            if (property == infoEnum.pubImage) return infoI.pubImage;
            if (property == infoEnum.pubTime) return infoI.pubTime;
            if (property == infoEnum.pubHours) return infoI.pubHours;
            if (property == infoEnum.pubMins) return infoI.pubMins;
            if (property == infoEnum.pubSecs) return infoI.pubSecs;
            if (property == infoEnum.publishWeb) return infoI.publishWeb;
            if (property == infoEnum.publishLocal) return infoI.publishLocal;
            if (property == infoEnum.timerOn) return infoI.timerOn;
            if (property == infoEnum.fileURLPubWeb) return infoI.fileURLPubWeb;
            if (property == infoEnum.filenamePrefixPubWeb) return infoI.filenamePrefixPubWeb;
            if (property == infoEnum.cycleStampCheckedPubWeb) return infoI.cycleStampCheckedPubWeb;
            if (property == infoEnum.startCyclePubWeb) return infoI.startCyclePubWeb;
            if (property == infoEnum.endCyclePubWeb) return infoI.endCyclePubWeb;
            if (property == infoEnum.currentCyclePubWeb) return infoI.currentCyclePubWeb;
            if (property == infoEnum.stampAppendPubWeb) return infoI.stampAppendPubWeb;
            if (property == infoEnum.fileDirPubLoc) return infoI.fileDirPubLoc;
            if (property == infoEnum.filenamePrefixPubLoc) return infoI.filenamePrefixPubLoc;
            if (property == infoEnum.fileDirPubCust) return infoI.fileDirPubCust;
            if (property == infoEnum.cycleStampCheckedPubLoc) return infoI.cycleStampCheckedPubLoc;
            if (property == infoEnum.startCyclePubLoc) return infoI.startCyclePubLoc;
            if (property == infoEnum.endCyclePubLoc) return infoI.endCyclePubLoc;
            if (property == infoEnum.currentCyclePubLoc) return infoI.currentCyclePubLoc;
            if (property == infoEnum.stampAppendPubLoc) return infoI.stampAppendPubLoc;
            if (property == infoEnum.publishFirst) return infoI.publishFirst;
            if (property == infoEnum.lastPublished) return infoI.lastPublished;
            if (property == infoEnum.ipWebcamAddress) return infoI.ipWebcamAddress;
            if (property == infoEnum.ipWebcamUser) return infoI.ipWebcamUser;
            if (property == infoEnum.ipWebcamPassword) return infoI.ipWebcamPassword;

            return null;
        }


        public static Camera getCam(string webcam)
        {

            foreach (ConnectedCamera item in CameraRig.ConnectedCameras)
            {

                if (item.cameraName == webcam) return item.cam;

            }

            return null;

        }

        public static Camera getCam(int cam)
        {

            return CameraRig.ConnectedCameras[cam].cam;
        }

        public static bool camerasAreConnected()
        {

            return ConnectedCameras.Count >= 1;

        }

        public static bool cameraExists(int id)
        {

            foreach (ConnectedCamera item in ConnectedCameras)
            {

                if (item.cam.camNo == id) return true;

            }

            return false;

        }


        public static int cameraCount()
        {
            return ConnectedCameras.Count;
        }


        public static void alert(bool alt)
        {
            foreach (ConnectedCamera rigI in ConnectedCameras)
            {
                rigI.cam.alert = alt;
                rigI.cam.detectionOn = alt;
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
            if (camerasAreConnected()) ConnectedCameras[camId].cam.areaOffAtMotionTriggered = true;
        }

        public static bool AreaOffAtMotionIsTriggeredCam(int camId)
        {
            if (camerasAreConnected())
            {
                return ConnectedCameras[camId].cam.areaOffAtMotionTriggered;
            }
            else
            {
                return false;
            }
        }


        public static bool Calibrating
        {
            get { return ConnectedCameras[activeCam].cam.calibrating; }
            set
            {
                if (camerasAreConnected()) ConnectedCameras[activeCam].cam.calibrating = value;
            }
        }

        public static bool AreaOffAtMotionTriggered
        {
            get { return ConnectedCameras[activeCam].cam.areaOffAtMotionTriggered; }
            set
            {
                if (camerasAreConnected()) ConnectedCameras[activeCam].cam.areaOffAtMotionTriggered = value;
            }
        }

        public static bool AreaOffAtMotionReset
        {
            get { return ConnectedCameras[activeCam].cam.areaOffAtMotionReset; }
            set
            {
                if (camerasAreConnected()) ConnectedCameras[activeCam].cam.areaOffAtMotionReset = value;
            }
        }

        public static bool AreaDetection
        {
            get { return ConnectedCameras[drawCam].cam.areaDetection; }
            set
            {
                if (camerasAreConnected()) ConnectedCameras[drawCam].cam.areaDetection = value;
            }
        }

        public static bool AreaDetectionWithin
        {
            get { return ConnectedCameras[drawCam].cam.areaDetectionWithin; }
            set
            {
                if (camerasAreConnected())
                    ConnectedCameras[drawCam].cam.areaDetectionWithin = value;
            }
        }

        public static bool ExposeArea
        {
            get { return ConnectedCameras[drawCam].cam.exposeArea; }
            set
            {
                if (camerasAreConnected()) ConnectedCameras[drawCam].cam.exposeArea = value;
            }
        }





    }






}

