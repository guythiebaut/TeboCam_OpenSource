using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TeboCam
{

    class rigItem
    {

        public string cameraName;
        public string friendlyName;
        public int displayButton;
        public Camera cam;

        public List<InfoItem> InfoItems = new List<InfoItem>();

    }


    class InfoItem
    {

        public object value;
        public string name;

        public InfoItem(object _value, string _name)
        {

            value = _value;
            name = _name;

        }


    }

    class cameraSpecificInfo
    {

        public string profileName = "";
        public string webcam = "";

        public string IpAddress = "";


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



        //for monitoring publishing - does not need to be saved to xml file
        public bool publishFirst = true;
        public int lastPublished = 0;
        //for monitoring publishing - does not need to be saved to xml file        


    }


    class CameraRig
    {

        public enum infoItem
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




        public static List<rigItem> rig = new List<rigItem>();
        public static List<cameraSpecificInfo> camInfo = new List<cameraSpecificInfo>();
        public static int activeCam = 0;
        public static int drawCam = 0;
        public static int trainCam = 0;
        private static int infoIdx = -1;
        public static List<bool> camSel = new List<bool>();
        private const int camLicense = 9;
        public static bool reconfiguring = false;


        public static void camSelInit()
        {

            for (int i = 1; i < 10; i++)
            {

                camSel.Add(false);

            }

        }

        public static void renameProfile(string oldProfile, string newProfile)
        {

            for (int i = 0; i < camInfo.Count; i++)
            {

                if (camInfo[i].profileName == oldProfile)
                {
                    camInfo[i].profileName = newProfile;
                    break;
                }

            }


        }

        public static void cameraRemove(int camId)
        {

            rig[camId].cam.motionLevelEvent -= new motionLevelEventHandler(bubble.motionEvent);
            rig[camId].cam.SignalToStop();
            rig[camId].cam.WaitForStop();

            rig.RemoveAt(camId);

        }



        public static List<List<string>> cameraCredentialsListedUnderProfile(string profileName)
        {

            List<List<string>> lst = new List<List<string>>();

            foreach (cameraSpecificInfo infoI in camInfo)
            {


                if (infoI.profileName == profileName)
                {

                    List<string> item = new List<string>();
                    item.Add(infoI.webcam);
                    item.Add(infoI.ipWebcamAddress);
                    item.Add(infoI.ipWebcamUser);
                    item.Add(infoI.ipWebcamPassword);
                    lst.Add(item);

                }



            }

            return lst;

        }

        public static List<string> cameraCredentials(string profileName, string webcam)
        {

            List<string> lst = new List<string>();

            foreach (cameraSpecificInfo infoI in camInfo)
            {

                if (infoI.profileName == profileName && infoI.webcam == webcam)
                {

                    lst.Add(infoI.webcam);
                    lst.Add(infoI.ipWebcamAddress);
                    lst.Add(infoI.ipWebcamUser);
                    lst.Add(infoI.ipWebcamPassword);

                }


            }

            return lst;

        }


        public static void clear()
        {
            reconfiguring = true;
            CameraRig.rig.Clear();
            reconfiguring = false;
        }



        public static void updateInfo(string profileName, string webcam, infoItem infoType, object val)
        {

            if (camerasAttached())
            {

                foreach (cameraSpecificInfo infoI in camInfo)
                {

                    if (infoI.profileName == profileName && infoI.webcam == webcam)
                    {

                        if (infoType == infoItem.friendlyName) { infoI.friendlyName = (string)val; }

                        if (infoType == infoItem.areaDetection) { infoI.areaDetection = (bool)val; }
                        if (infoType == infoItem.areaDetectionWithin) { infoI.areaDetectionWithin = (bool)val; }
                        if (infoType == infoItem.alarmActive) { infoI.alarmActive = (bool)val; }
                        if (infoType == infoItem.publishActive) { infoI.publishActive = (bool)val; }


                        //may be of use in future
                        if (infoType == infoItem.areaOffAtMotion) { infoI.areaOffAtMotion = (bool)val; }
                        //may be of use in future

                        if (infoType == infoItem.rectX) { infoI.rectX = (int)val; }
                        if (infoType == infoItem.rectY) { infoI.rectY = (int)val; }
                        if (infoType == infoItem.rectWidth) { infoI.rectWidth = (int)val; }
                        if (infoType == infoItem.rectHeight) { infoI.rectHeight = (int)val; }
                        if (infoType == infoItem.movementVal) { infoI.movementVal = (double)val; }
                        if (infoType == infoItem.timeSpike) { infoI.timeSpike = (int)val; }
                        if (infoType == infoItem.toleranceSpike) { infoI.toleranceSpike = (int)val; }
                        if (infoType == infoItem.lightSpike) { infoI.lightSpike = (bool)val; }
                        if (infoType == infoItem.displayButton) { infoI.displayButton = (int)val; }

                        if (infoType == infoItem.pubImage) { infoI.pubImage = (bool)val; }
                        if (infoType == infoItem.pubTime) { infoI.pubTime = (int)val; }
                        if (infoType == infoItem.pubHours) { infoI.pubHours = (bool)val; }
                        if (infoType == infoItem.pubMins) { infoI.pubMins = (bool)val; }
                        if (infoType == infoItem.pubSecs) { infoI.pubSecs = (bool)val; }
                        if (infoType == infoItem.publishWeb) { infoI.publishWeb = (bool)val; }
                        if (infoType == infoItem.publishLocal) { infoI.publishLocal = (bool)val; }
                        if (infoType == infoItem.timerOn) { infoI.timerOn = (bool)val; }
                        if (infoType == infoItem.fileURLPubWeb) { infoI.fileURLPubWeb = (string)val; }
                        if (infoType == infoItem.filenamePrefixPubWeb) { infoI.filenamePrefixPubWeb = (string)val; }
                        if (infoType == infoItem.cycleStampCheckedPubWeb) { infoI.cycleStampCheckedPubWeb = (int)val; }
                        if (infoType == infoItem.startCyclePubWeb) { infoI.startCyclePubWeb = (int)val; }
                        if (infoType == infoItem.endCyclePubWeb) { infoI.endCyclePubWeb = (int)val; }
                        if (infoType == infoItem.currentCyclePubWeb) { infoI.currentCyclePubWeb = (int)val; }
                        if (infoType == infoItem.stampAppendPubWeb) { infoI.stampAppendPubWeb = (bool)val; }
                        if (infoType == infoItem.fileDirPubLoc) { infoI.fileDirPubLoc = (string)val; }
                        if (infoType == infoItem.filenamePrefixPubLoc) { infoI.filenamePrefixPubLoc = (string)val; }
                        if (infoType == infoItem.fileDirPubCust) { infoI.fileDirPubCust = (bool)val; }
                        if (infoType == infoItem.cycleStampCheckedPubLoc) { infoI.cycleStampCheckedPubLoc = (int)val; }
                        if (infoType == infoItem.startCyclePubLoc) { infoI.startCyclePubLoc = (int)val; }
                        if (infoType == infoItem.endCyclePubLoc) { infoI.endCyclePubLoc = (int)val; }
                        if (infoType == infoItem.currentCyclePubLoc) { infoI.currentCyclePubLoc = (int)val; }
                        if (infoType == infoItem.stampAppendPubLoc) { infoI.stampAppendPubLoc = (bool)val; }

                        if (infoType == infoItem.publishFirst) { infoI.publishFirst = (bool)val; }
                        if (infoType == infoItem.lastPublished) { infoI.lastPublished = (int)val; }

                        if (infoType == infoItem.ipWebcamAddress) { infoI.ipWebcamAddress = (string)val; }
                        if (infoType == infoItem.ipWebcamUser) { infoI.ipWebcamUser = (string)val; }
                        if (infoType == infoItem.ipWebcamPassword) { infoI.ipWebcamPassword = (string)val; }

                    }

                }

            }

        }


        public static object getInfoItem(int idx,string name)
        {

            foreach (InfoItem item in rig[idx].InfoItems)
            {

                if (item.name == name)
                {

                    return item.value;

                }

            }

            return null;

        }


        public static void addInfo(infoItem infoType, object val)
        {


            if (infoType == infoItem.webcam)
            {

                //newcode
                rigItem rig_item = new rigItem();
                rig_item.cameraName = (string)val;
                rig.Add(rig_item);
                //newcode

                infoIdx++;
                cameraSpecificInfo p_item = new cameraSpecificInfo();
                camInfo.Add(p_item);
                camInfo[infoIdx].webcam = (string)val;

            }

            if (camInfo.Count > 0)
            {


                InfoItem item = new InfoItem(val, infoType.ToString());
                rig[rig.Count - 1].InfoItems.Add(item);


                if (infoType == infoItem.profileName) { camInfo[infoIdx].profileName = (string)val; }
                if (infoType == infoItem.friendlyName) { camInfo[infoIdx].friendlyName = (string)val; }

                if (infoType == infoItem.areaDetection) { camInfo[infoIdx].areaDetection = (bool)val; }
                if (infoType == infoItem.areaDetectionWithin) { camInfo[infoIdx].areaDetectionWithin = (bool)val; }


                if (infoType == infoItem.alarmActive) { camInfo[infoIdx].alarmActive = (bool)val; }
                if (infoType == infoItem.publishActive) { camInfo[infoIdx].publishActive = (bool)val; }


                //may be of use in future
                if (infoType == infoItem.areaOffAtMotion) { camInfo[infoIdx].areaOffAtMotion = (bool)val; }
                //may be of use in future

                if (infoType == infoItem.rectX) { camInfo[infoIdx].rectX = (int)val; }
                if (infoType == infoItem.rectY) { camInfo[infoIdx].rectY = (int)val; }
                if (infoType == infoItem.rectWidth) { camInfo[infoIdx].rectWidth = (int)val; }
                if (infoType == infoItem.rectHeight) { camInfo[infoIdx].rectHeight = (int)val; }
                if (infoType == infoItem.movementVal) { camInfo[infoIdx].movementVal = (double)val; }
                if (infoType == infoItem.timeSpike) { camInfo[infoIdx].timeSpike = (int)val; }
                if (infoType == infoItem.toleranceSpike) { camInfo[infoIdx].toleranceSpike = (int)val; }
                if (infoType == infoItem.lightSpike) { camInfo[infoIdx].lightSpike = (bool)val; }
                if (infoType == infoItem.displayButton) { camInfo[infoIdx].displayButton = (int)val; }



                if (infoType == infoItem.pubImage) { camInfo[infoIdx].pubImage = (bool)val; }
                if (infoType == infoItem.pubTime) { camInfo[infoIdx].pubTime = (int)val; }
                if (infoType == infoItem.pubHours) { camInfo[infoIdx].pubHours = (bool)val; }
                if (infoType == infoItem.pubMins) { camInfo[infoIdx].pubMins = (bool)val; }
                if (infoType == infoItem.pubSecs) { camInfo[infoIdx].pubSecs = (bool)val; }
                if (infoType == infoItem.publishWeb) { camInfo[infoIdx].publishWeb = (bool)val; }
                if (infoType == infoItem.publishLocal) { camInfo[infoIdx].publishLocal = (bool)val; }
                if (infoType == infoItem.timerOn) { camInfo[infoIdx].timerOn = (bool)val; }
                if (infoType == infoItem.fileURLPubWeb) { camInfo[infoIdx].fileURLPubWeb = (string)val; }
                if (infoType == infoItem.filenamePrefixPubWeb) { camInfo[infoIdx].filenamePrefixPubWeb = (string)val; }
                if (infoType == infoItem.cycleStampCheckedPubWeb) { camInfo[infoIdx].cycleStampCheckedPubWeb = (int)val; }
                if (infoType == infoItem.startCyclePubWeb) { camInfo[infoIdx].startCyclePubWeb = (int)val; }
                if (infoType == infoItem.endCyclePubWeb) { camInfo[infoIdx].endCyclePubWeb = (int)val; }
                if (infoType == infoItem.currentCyclePubWeb) { camInfo[infoIdx].currentCyclePubWeb = (int)val; }
                if (infoType == infoItem.stampAppendPubWeb) { camInfo[infoIdx].stampAppendPubWeb = (bool)val; }
                if (infoType == infoItem.fileDirPubLoc) { camInfo[infoIdx].fileDirPubLoc = (string)val; }
                if (infoType == infoItem.filenamePrefixPubLoc) { camInfo[infoIdx].filenamePrefixPubLoc = (string)val; }
                if (infoType == infoItem.fileDirPubCust) { camInfo[infoIdx].fileDirPubCust = (bool)val; }
                if (infoType == infoItem.cycleStampCheckedPubLoc) { camInfo[infoIdx].cycleStampCheckedPubLoc = (int)val; }
                if (infoType == infoItem.startCyclePubLoc) { camInfo[infoIdx].startCyclePubLoc = (int)val; }
                if (infoType == infoItem.endCyclePubLoc) { camInfo[infoIdx].endCyclePubLoc = (int)val; }
                if (infoType == infoItem.currentCyclePubLoc) { camInfo[infoIdx].currentCyclePubLoc = (int)val; }
                if (infoType == infoItem.stampAppendPubLoc) { camInfo[infoIdx].stampAppendPubLoc = (bool)val; }

                if (infoType == infoItem.publishFirst) { camInfo[infoIdx].publishFirst = (bool)val; }
                if (infoType == infoItem.lastPublished) { camInfo[infoIdx].lastPublished = (int)val; }


                if (infoType == infoItem.ipWebcamAddress) { camInfo[infoIdx].ipWebcamAddress = (string)val; }
                if (infoType == infoItem.ipWebcamUser) { camInfo[infoIdx].ipWebcamUser = (string)val; }
                if (infoType == infoItem.ipWebcamPassword) { camInfo[infoIdx].ipWebcamPassword = (string)val; }


            }

            //}

        }



        public static void rigInfoPopulateForCam(string profileName, string webcam)
        {

            foreach (rigItem item in CameraRig.rig)
            {

                if (item.cameraName == webcam)
                {

                    item.friendlyName = (string)(CameraRig.rigInfoGet(profileName, webcam, infoItem.friendlyName));
                    item.cam.areaDetection = (bool)(CameraRig.rigInfoGet(profileName, webcam, infoItem.areaDetection));
                    item.cam.areaDetectionWithin = (bool)(CameraRig.rigInfoGet(profileName, webcam, infoItem.areaDetectionWithin));

                }

            }

        }


        public static int idFromButton(int bttn)
        {

            foreach (rigItem item in CameraRig.rig)
            {

                if (item.displayButton == bttn) return item.cam.cam;

            }

            return 0;

        }

        public static int idxFromButton(int bttn)
        {

            for (int i = 0; i < rig.Count; i++)
            {

                if (rig[i].displayButton == bttn)
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

            foreach (cameraSpecificInfo infoI in camInfo)
            {

                //we have found this button is already assigned to another camera
                if (infoI.profileName == profileName && infoI.displayButton == newBttn && infoI.webcam != camName)
                {

                    swapId = rig[id].displayButton;
                    swappingCamName = infoI.webcam;
                    infoI.displayButton = swapId;

                }

            }

            foreach (cameraSpecificInfo infoI in camInfo)
            {

                if (profileName == infoI.profileName && infoI.webcam == camName)
                {

                    infoI.displayButton = newBttn;

                }

            }

            foreach (rigItem item in CameraRig.rig)
            {

                if (item.cameraName == swappingCamName) item.displayButton = swapId;
                if (item.cameraName == camName) item.displayButton = newBttn;

            }



        }


        public static void rigInfoPopulate(string profileName, int id)
        {
            bool infoExists = false;

            foreach (cameraSpecificInfo infoI in camInfo)
            {

                if (profileName == infoI.profileName && infoI.webcam == rig[id].cameraName)
                {

                    rig[id].friendlyName = infoI.friendlyName;
                    rig[id].displayButton = infoI.displayButton;


                    rig[id].cam.areaDetectionWithin = infoI.areaDetectionWithin;
                    rig[id].cam.areaDetection = infoI.areaDetection;

                    rig[id].cam.rectX = infoI.rectX;
                    rig[id].cam.rectY = infoI.rectY;
                    rig[id].cam.rectHeight = infoI.rectHeight;
                    rig[id].cam.rectWidth = infoI.rectWidth;

                    rig[id].cam.movementVal = infoI.movementVal;
                    //rig[id].cam.timeSpike = infoI.timeSpike;
                    //rig[id].cam.toleranceSpike = infoI.toleranceSpike;



                    rig[id].cam.alarmActive = infoI.alarmActive;
                    rig[id].cam.publishActive = infoI.publishActive;

                    infoExists = true;

                    break;

                }

            }

            //create the info
            if (!infoExists)
            {

                cameraSpecificInfo infoI = new cameraSpecificInfo();

                infoI.profileName = profileName;
                infoI.webcam = rig[id].cameraName;
                camInfo.Add(infoI);

                rig[id].displayButton = infoI.displayButton;

                rig[id].cam.areaDetectionWithin = infoI.areaDetectionWithin;
                rig[id].cam.areaDetection = infoI.areaDetection;
                rig[id].cam.rectX = infoI.rectX;
                rig[id].cam.rectY = infoI.rectY;
                rig[id].cam.rectHeight = infoI.rectHeight;
                rig[id].cam.rectWidth = infoI.rectWidth;


                rig[id].cam.movementVal = infoI.movementVal;
                //rig[id].cam.timeSpike = infoI.timeSpike;
                //rig[id].cam.toleranceSpike = infoI.toleranceSpike;


            }

        }





        public static object rigInfoGet(string profile, string webcam, infoItem property)
        {

            foreach (cameraSpecificInfo infoI in camInfo)
            {

                if (infoI.profileName == profile && infoI.webcam == webcam)
                {
                    
                    if (property == infoItem.friendlyName) return infoI.friendlyName;
                    if (property == infoItem.areaDetection) return infoI.areaDetection;
                    if (property == infoItem.areaDetectionWithin) return infoI.areaDetectionWithin;
                    if (property == infoItem.areaOffAtMotion) return infoI.areaOffAtMotion;
                    if (property == infoItem.rectX) return infoI.rectX;
                    if (property == infoItem.rectY) return infoI.rectY;
                    if (property == infoItem.rectWidth) return infoI.rectWidth;
                    if (property == infoItem.rectHeight) return infoI.rectHeight;
                    if (property == infoItem.movementVal) return infoI.movementVal;
                    if (property == infoItem.timeSpike) return infoI.timeSpike;
                    if (property == infoItem.toleranceSpike) return infoI.toleranceSpike;
                    if (property == infoItem.lightSpike) return infoI.lightSpike;
                    if (property == infoItem.alarmActive) return infoI.alarmActive;
                    if (property == infoItem.publishActive) return infoI.publishActive;

                    if (property == infoItem.pubImage) return infoI.pubImage;
                    if (property == infoItem.pubTime) return infoI.pubTime;
                    if (property == infoItem.pubHours) return infoI.pubHours;
                    if (property == infoItem.pubMins) return infoI.pubMins;
                    if (property == infoItem.pubSecs) return infoI.pubSecs;
                    if (property == infoItem.publishWeb) return infoI.publishWeb;
                    if (property == infoItem.publishLocal) return infoI.publishLocal;
                    if (property == infoItem.timerOn) return infoI.timerOn;
                    if (property == infoItem.fileURLPubWeb) return infoI.fileURLPubWeb;
                    if (property == infoItem.filenamePrefixPubWeb) return infoI.filenamePrefixPubWeb;
                    if (property == infoItem.cycleStampCheckedPubWeb) return infoI.cycleStampCheckedPubWeb;
                    if (property == infoItem.startCyclePubWeb) return infoI.startCyclePubWeb;
                    if (property == infoItem.endCyclePubWeb) return infoI.endCyclePubWeb;
                    if (property == infoItem.currentCyclePubWeb) return infoI.currentCyclePubWeb;
                    if (property == infoItem.stampAppendPubWeb) return infoI.stampAppendPubWeb;
                    if (property == infoItem.fileDirPubLoc) return infoI.fileDirPubLoc;
                    if (property == infoItem.filenamePrefixPubLoc) return infoI.filenamePrefixPubLoc;
                    if (property == infoItem.fileDirPubCust) return infoI.fileDirPubCust;
                    if (property == infoItem.cycleStampCheckedPubLoc) return infoI.cycleStampCheckedPubLoc;
                    if (property == infoItem.startCyclePubLoc) return infoI.startCyclePubLoc;
                    if (property == infoItem.endCyclePubLoc) return infoI.endCyclePubLoc;
                    if (property == infoItem.currentCyclePubLoc) return infoI.currentCyclePubLoc;
                    if (property == infoItem.stampAppendPubLoc) return infoI.stampAppendPubLoc;

                    if (property == infoItem.publishFirst) return infoI.publishFirst;
                    if (property == infoItem.lastPublished) return infoI.lastPublished;

                    if (property == infoItem.ipWebcamAddress) return infoI.ipWebcamAddress;
                    if (property == infoItem.ipWebcamUser) return infoI.ipWebcamUser;
                    if (property == infoItem.ipWebcamPassword) return infoI.ipWebcamPassword;


                }

            }

            return null;
        }


        public static Camera getCam(string webcam)
        {

            foreach (rigItem item in CameraRig.rig)
            {

                if (item.cameraName == webcam) return item.cam;

            }

            return null;

        }

        public static Camera getCam(int cam)
        {

            return CameraRig.rig[cam].cam;
        }

        //public static void addCamera(rigItem p_cam)
        //{

        //    rigItem r_item = new rigItem();
        //    r_item = p_cam;
        //    rig.Add(r_item);


        //}

        public static bool camerasAttached()
        {

            return rig.Count >= 1;

        }

        public static bool cameraExists(int id)
        {

            foreach (rigItem item in rig)
            {

                if (item.cam.cam == id) return true;

            }

            return false;

        }


        public static int cameraCount()
        {

            return rig.Count;

        }


        public static void alert(bool alt)
        {
            foreach (rigItem rigI in rig)
            {
                rigI.cam.alert = alt;
                rigI.cam.detectionOn = alt;
            }
        }


        public static bool camerasAlreadySelected(string name)
        {

            if (!camerasAttached()) return false;

            foreach (rigItem rigI in rig)
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
            if (camerasAttached()) rig[camId].cam.areaOffAtMotionTriggered = true;
        }

        public static bool AreaOffAtMotionIsTriggeredCam(int camId)
        {
            if (camerasAttached())
            {
                return rig[camId].cam.areaOffAtMotionTriggered;
            }
            else
            {
                return false;
            }
        }


        public static bool Calibrating
        {
            get { return rig[activeCam].cam.calibrating; }
            set
            {
                if (camerasAttached()) rig[activeCam].cam.calibrating = value;
            }
        }

        public static bool AreaOffAtMotionTriggered
        {
            get { return rig[activeCam].cam.areaOffAtMotionTriggered; }
            set
            {
                if (camerasAttached()) rig[activeCam].cam.areaOffAtMotionTriggered = value;
            }
        }

        public static bool AreaOffAtMotionReset
        {
            get { return rig[activeCam].cam.areaOffAtMotionReset; }
            set
            {
                if (camerasAttached()) rig[activeCam].cam.areaOffAtMotionReset = value;
            }
        }

        public static bool AreaDetection
        {
            get { return rig[drawCam].cam.areaDetection; }
            set
            {
                if (camerasAttached()) rig[drawCam].cam.areaDetection = value;
            }
        }

        public static bool AreaDetectionWithin
        {
            get { return rig[drawCam].cam.areaDetectionWithin; }
            set
            {
                if (camerasAttached())
                    rig[drawCam].cam.areaDetectionWithin = value;
            }
        }

        public static bool ExposeArea
        {
            get { return rig[drawCam].cam.exposeArea; }
            set
            {
                if (camerasAttached()) rig[drawCam].cam.exposeArea = value;
            }
        }



    }






}

