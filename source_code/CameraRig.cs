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

    }

    class info
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

        public static List<rigItem> rig = new List<rigItem>();
        public static List<info> camInfo = new List<info>();
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

            foreach (info infoI in camInfo)
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

        public static List<string> cameraCredentials(string profileName,string webcam )
        {

            List<string> lst = new List<string>();

            foreach (info infoI in camInfo)
            {

                if (infoI.profileName == profileName && infoI.webcam==webcam)
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



        public static void updateInfo(string profileName, string webcam, string infoType, object val)
        {

            if (camerasAttached())
            {

                foreach (info infoI in camInfo)
                {

                    if (infoI.profileName == profileName && infoI.webcam == webcam)
                    {

                        if (infoType == "friendlyName") { infoI.friendlyName = (string)val; }

                        if (infoType == "areaDetection") { infoI.areaDetection = (bool)val; }
                        if (infoType == "areaDetectionWithin") { infoI.areaDetectionWithin = (bool)val; }
                        if (infoType == "alarmActive") { infoI.alarmActive = (bool)val; }
                        if (infoType == "publishActive") { infoI.publishActive = (bool)val; }


                        //may be of use in future
                        if (infoType == "areaOffAtMotion") { infoI.areaOffAtMotion = (bool)val; }
                        //may be of use in future

                        if (infoType == "rectX") { infoI.rectX = (int)val; }
                        if (infoType == "rectY") { infoI.rectY = (int)val; }
                        if (infoType == "rectWidth") { infoI.rectWidth = (int)val; }
                        if (infoType == "rectHeight") { infoI.rectHeight = (int)val; }
                        if (infoType == "movementVal") { infoI.movementVal = (double)val; }
                        if (infoType == "displayButton") { infoI.displayButton = (int)val; }

                        if (infoType == "pubImage") { infoI.pubImage = (bool)val; }
                        if (infoType == "pubTime") { infoI.pubTime = (int)val; }
                        if (infoType == "pubHours") { infoI.pubHours = (bool)val; }
                        if (infoType == "pubMins") { infoI.pubMins = (bool)val; }
                        if (infoType == "pubSecs") { infoI.pubSecs = (bool)val; }
                        if (infoType == "publishWeb") { infoI.publishWeb = (bool)val; }
                        if (infoType == "publishLocal") { infoI.publishLocal = (bool)val; }
                        if (infoType == "timerOn") { infoI.timerOn = (bool)val; }
                        if (infoType == "fileURLPubWeb") { infoI.fileURLPubWeb = (string)val; }
                        if (infoType == "filenamePrefixPubWeb") { infoI.filenamePrefixPubWeb = (string)val; }
                        if (infoType == "cycleStampCheckedPubWeb") { infoI.cycleStampCheckedPubWeb = (int)val; }
                        if (infoType == "startCyclePubWeb") { infoI.startCyclePubWeb = (int)val; }
                        if (infoType == "endCyclePubWeb") { infoI.endCyclePubWeb = (int)val; }
                        if (infoType == "currentCyclePubWeb") { infoI.currentCyclePubWeb = (int)val; }
                        if (infoType == "stampAppendPubWeb") { infoI.stampAppendPubWeb = (bool)val; }
                        if (infoType == "fileDirPubLoc") { infoI.fileDirPubLoc = (string)val; }
                        if (infoType == "filenamePrefixPubLoc") { infoI.filenamePrefixPubLoc = (string)val; }
                        if (infoType == "fileDirPubCust") { infoI.fileDirPubCust = (bool)val; }
                        if (infoType == "cycleStampCheckedPubLoc") { infoI.cycleStampCheckedPubLoc = (int)val; }
                        if (infoType == "startCyclePubLoc") { infoI.startCyclePubLoc = (int)val; }
                        if (infoType == "endCyclePubLoc") { infoI.endCyclePubLoc = (int)val; }
                        if (infoType == "currentCyclePubLoc") { infoI.currentCyclePubLoc = (int)val; }
                        if (infoType == "stampAppendPubLoc") { infoI.stampAppendPubLoc = (bool)val; }

                        if (infoType == "publishFirst") { infoI.publishFirst = (bool)val; }
                        if (infoType == "lastPublished") { infoI.lastPublished = (int)val; }

                        if (infoType == "ipWebcamAddress") { infoI.ipWebcamAddress = (string)val; }
                        if (infoType == "ipWebcamUser") { infoI.ipWebcamUser = (string)val; }
                        if (infoType == "ipWebcamPassword") { infoI.ipWebcamPassword = (string)val; }

                    }

                }

            }

        }

        public static void updateInfo(string profileName, string infoType, object val)
        {

            if (camerasAttached())
            {

                foreach (info infoI in camInfo)
                {

                    if (infoI.profileName == profileName && infoI.webcam == rig[activeCam].cameraName)
                    {

                        if (infoType == "friendlyName") { infoI.friendlyName = (string)val; }

                        if (infoType == "areaDetection") { infoI.areaDetection = (bool)val; }
                        if (infoType == "areaDetectionWithin") { infoI.areaDetectionWithin = (bool)val; }
                        if (infoType == "alarmActive") { infoI.alarmActive = (bool)val; }
                        if (infoType == "publishActive") { infoI.publishActive = (bool)val; }


                        //may be of use in future
                        if (infoType == "areaOffAtMotion") { infoI.areaOffAtMotion = (bool)val; }
                        //may be of use in future

                        if (infoType == "rectX") { infoI.rectX = (int)val; }
                        if (infoType == "rectY") { infoI.rectY = (int)val; }
                        if (infoType == "rectWidth") { infoI.rectWidth = (int)val; }
                        if (infoType == "rectHeight") { infoI.rectHeight = (int)val; }
                        if (infoType == "movementVal") { infoI.movementVal = (double)val; }
                        if (infoType == "displayButton") { infoI.displayButton = (int)val; }


                        if (infoType == "pubImage") { infoI.pubImage = (bool)val; }
                        if (infoType == "pubTime") { infoI.pubTime = (int)val; }
                        if (infoType == "pubHours") { infoI.pubHours = (bool)val; }
                        if (infoType == "pubMins") { infoI.pubMins = (bool)val; }
                        if (infoType == "pubSecs") { infoI.pubSecs = (bool)val; }
                        if (infoType == "publishWeb") { infoI.publishWeb = (bool)val; }
                        if (infoType == "publishLocal") { infoI.publishLocal = (bool)val; }
                        if (infoType == "timerOn") { infoI.timerOn = (bool)val; }
                        if (infoType == "fileURLPubWeb") { infoI.fileURLPubWeb = (string)val; }
                        if (infoType == "filenamePrefixPubWeb") { infoI.filenamePrefixPubWeb = (string)val; }
                        if (infoType == "cycleStampCheckedPubWeb") { infoI.cycleStampCheckedPubWeb = (int)val; }
                        if (infoType == "startCyclePubWeb") { infoI.startCyclePubWeb = (int)val; }
                        if (infoType == "endCyclePubWeb") { infoI.endCyclePubWeb = (int)val; }
                        if (infoType == "currentCyclePubWeb") { infoI.currentCyclePubWeb = (int)val; }
                        if (infoType == "stampAppendPubWeb") { infoI.stampAppendPubWeb = (bool)val; }
                        if (infoType == "fileDirPubLoc") { infoI.fileDirPubLoc = (string)val; }
                        if (infoType == "filenamePrefixPubLoc") { infoI.filenamePrefixPubLoc = (string)val; }
                        if (infoType == "fileDirPubCust") { infoI.fileDirPubCust = (bool)val; }
                        if (infoType == "cycleStampCheckedPubLoc") { infoI.cycleStampCheckedPubLoc = (int)val; }
                        if (infoType == "startCyclePubLoc") { infoI.startCyclePubLoc = (int)val; }
                        if (infoType == "endCyclePubLoc") { infoI.endCyclePubLoc = (int)val; }
                        if (infoType == "currentCyclePubLoc") { infoI.currentCyclePubLoc = (int)val; }
                        if (infoType == "stampAppendPubLoc") { infoI.stampAppendPubLoc = (bool)val; }

                        if (infoType == "publishFirst") { infoI.publishFirst = (bool)val; }
                        if (infoType == "lastPublished") { infoI.lastPublished = (int)val; }


                        if (infoType == "ipWebcamAddress") { infoI.ipWebcamAddress = (string)val; }
                        if (infoType == "ipWebcamUser") { infoI.ipWebcamUser = (string)val; }
                        if (infoType == "ipWebcamPassword") { infoI.ipWebcamPassword = (string)val; }


                    }

                }

            }
        }


        public static void addInfo(string infoType, object val)
        {

            if (infoType == "webcam")
            {

                infoIdx++;
                info p_item = new info();
                camInfo.Add(p_item);
                camInfo[infoIdx].webcam = (string)val;

            }

            if (camInfo.Count > 0)
            {

                if (infoType == "profileName") { camInfo[infoIdx].profileName = (string)val; }
                if (infoType == "friendlyName") { camInfo[infoIdx].friendlyName = (string)val; }

                if (infoType == "areaDetection") { camInfo[infoIdx].areaDetection = (bool)val; }
                if (infoType == "areaDetectionWithin") { camInfo[infoIdx].areaDetectionWithin = (bool)val; }


                if (infoType == "alarmActive") { camInfo[infoIdx].alarmActive = (bool)val; }
                if (infoType == "publishActive") { camInfo[infoIdx].publishActive = (bool)val; }


                //may be of use in future
                if (infoType == "areaOffAtMotion") { camInfo[infoIdx].areaOffAtMotion = (bool)val; }
                //may be of use in future

                if (infoType == "rectX") { camInfo[infoIdx].rectX = (int)val; }
                if (infoType == "rectY") { camInfo[infoIdx].rectY = (int)val; }
                if (infoType == "rectWidth") { camInfo[infoIdx].rectWidth = (int)val; }
                if (infoType == "rectHeight") { camInfo[infoIdx].rectHeight = (int)val; }
                if (infoType == "movementVal") { camInfo[infoIdx].movementVal = (double)val; }
                if (infoType == "displayButton") { camInfo[infoIdx].displayButton = (int)val; }



                if (infoType == "pubImage") { camInfo[infoIdx].pubImage = (bool)val; }
                if (infoType == "pubTime") { camInfo[infoIdx].pubTime = (int)val; }
                if (infoType == "pubHours") { camInfo[infoIdx].pubHours = (bool)val; }
                if (infoType == "pubMins") { camInfo[infoIdx].pubMins = (bool)val; }
                if (infoType == "pubSecs") { camInfo[infoIdx].pubSecs = (bool)val; }
                if (infoType == "publishWeb") { camInfo[infoIdx].publishWeb = (bool)val; }
                if (infoType == "publishLocal") { camInfo[infoIdx].publishLocal = (bool)val; }
                if (infoType == "timerOn") { camInfo[infoIdx].timerOn = (bool)val; }
                if (infoType == "fileURLPubWeb") { camInfo[infoIdx].fileURLPubWeb = (string)val; }
                if (infoType == "filenamePrefixPubWeb") { camInfo[infoIdx].filenamePrefixPubWeb = (string)val; }
                if (infoType == "cycleStampCheckedPubWeb") { camInfo[infoIdx].cycleStampCheckedPubWeb = (int)val; }
                if (infoType == "startCyclePubWeb") { camInfo[infoIdx].startCyclePubWeb = (int)val; }
                if (infoType == "endCyclePubWeb") { camInfo[infoIdx].endCyclePubWeb = (int)val; }
                if (infoType == "currentCyclePubWeb") { camInfo[infoIdx].currentCyclePubWeb = (int)val; }
                if (infoType == "stampAppendPubWeb") { camInfo[infoIdx].stampAppendPubWeb = (bool)val; }
                if (infoType == "fileDirPubLoc") { camInfo[infoIdx].fileDirPubLoc = (string)val; }
                if (infoType == "filenamePrefixPubLoc") { camInfo[infoIdx].filenamePrefixPubLoc = (string)val; }
                if (infoType == "fileDirPubCust") { camInfo[infoIdx].fileDirPubCust = (bool)val; }
                if (infoType == "cycleStampCheckedPubLoc") { camInfo[infoIdx].cycleStampCheckedPubLoc = (int)val; }
                if (infoType == "startCyclePubLoc") { camInfo[infoIdx].startCyclePubLoc = (int)val; }
                if (infoType == "endCyclePubLoc") { camInfo[infoIdx].endCyclePubLoc = (int)val; }
                if (infoType == "currentCyclePubLoc") { camInfo[infoIdx].currentCyclePubLoc = (int)val; }
                if (infoType == "stampAppendPubLoc") { camInfo[infoIdx].stampAppendPubLoc = (bool)val; }

                if (infoType == "publishFirst") { camInfo[infoIdx].publishFirst = (bool)val; }
                if (infoType == "lastPublished") { camInfo[infoIdx].lastPublished = (int)val; }


                if (infoType == "ipWebcamAddress") { camInfo[infoIdx].ipWebcamAddress = (string)val; }
                if (infoType == "ipWebcamUser") { camInfo[infoIdx].ipWebcamUser = (string)val; }
                if (infoType == "ipWebcamPassword") { camInfo[infoIdx].ipWebcamPassword = (string)val; }


            }

            //}

        }



        public static void rigInfoPopulateForCam(string profileName, string webcam)
        {

            foreach (rigItem item in CameraRig.rig)
            {

                if (item.cameraName == webcam)
                {

                    item.friendlyName = (string)(CameraRig.rigInfoGet(profileName, webcam, "friendlyName"));
                    item.cam.areaDetection = (bool)(CameraRig.rigInfoGet(profileName, webcam, "areaDetection"));
                    item.cam.areaDetectionWithin = (bool)(CameraRig.rigInfoGet(profileName, webcam, "areaDetectionWithin"));

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

            foreach (info infoI in camInfo)
            {

                //we have found this button is already assigned to another camera
                if (infoI.profileName == profileName && infoI.displayButton == newBttn && infoI.webcam != camName)
                {

                    swapId = rig[id].displayButton;
                    swappingCamName = infoI.webcam;
                    infoI.displayButton = swapId;

                }

            }

            foreach (info infoI in camInfo)
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

            foreach (info infoI in camInfo)
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



                    rig[id].cam.alarmActive = infoI.alarmActive;
                    rig[id].cam.publishActive = infoI.publishActive;

                    infoExists = true;

                    break;

                }

            }

            //create the info
            if (!infoExists)
            {

                info infoI = new info();

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


            }

        }





        public static object rigInfoGet(string profile, string property)
        {

            foreach (info infoI in camInfo)
            {

                if (infoI.profileName == profile && infoI.webcam == rig[activeCam].cameraName)
                {

                    if (property == "friendlyName") return infoI.friendlyName;
                    if (property == "areaDetection") return infoI.areaDetection;
                    if (property == "areaDetectionWithin") return infoI.areaDetectionWithin;
                    if (property == "areaOffAtMotion") return infoI.areaOffAtMotion;
                    if (property == "rectX") return infoI.rectX;
                    if (property == "rectY") return infoI.rectY;
                    if (property == "rectWidth") return infoI.rectWidth;
                    if (property == "rectHeight") return infoI.rectHeight;
                    if (property == "movementVal") return infoI.movementVal;
                    if (property == "alarmActive") return infoI.alarmActive;
                    if (property == "publishActive") return infoI.publishActive;

                    if (property == "pubImage") return infoI.pubImage;
                    if (property == "pubTime") return infoI.pubTime;
                    if (property == "pubHours") return infoI.pubHours;
                    if (property == "pubMins") return infoI.pubMins;
                    if (property == "pubSecs") return infoI.pubSecs;
                    if (property == "publishWeb") return infoI.publishWeb;
                    if (property == "publishLocal") return infoI.publishLocal;
                    if (property == "timerOn") return infoI.timerOn;
                    if (property == "fileURLPubWeb") return infoI.fileURLPubWeb;
                    if (property == "filenamePrefixPubWeb") return infoI.filenamePrefixPubWeb;
                    if (property == "cycleStampCheckedPubWeb") return infoI.cycleStampCheckedPubWeb;
                    if (property == "startCyclePubWeb") return infoI.startCyclePubWeb;
                    if (property == "endCyclePubWeb") return infoI.endCyclePubWeb;
                    if (property == "currentCyclePubWeb") return infoI.currentCyclePubWeb;
                    if (property == "stampAppendPubWeb") return infoI.stampAppendPubWeb;
                    if (property == "fileDirPubLoc") return infoI.fileDirPubLoc;
                    if (property == "filenamePrefixPubLoc") return infoI.filenamePrefixPubLoc;
                    if (property == "fileDirPubCust") return infoI.fileDirPubCust;
                    if (property == "cycleStampCheckedPubLoc") return infoI.cycleStampCheckedPubLoc;
                    if (property == "startCyclePubLoc") return infoI.startCyclePubLoc;
                    if (property == "endCyclePubLoc") return infoI.endCyclePubLoc;
                    if (property == "currentCyclePubLoc") return infoI.currentCyclePubLoc;
                    if (property == "stampAppendPubLoc") return infoI.stampAppendPubLoc;

                    if (property == "publishFirst") return infoI.publishFirst;
                    if (property == "lastPublished") return infoI.lastPublished;


                    if (property == "ipWebcamAddress") return infoI.ipWebcamAddress;
                    if (property == "ipWebcamUser") return infoI.ipWebcamUser;
                    if (property == "ipWebcamPassword") return infoI.ipWebcamPassword;

                }

            }

            return null;
        }


        public static object rigInfoGet(string profile, string webcam, string property)
        {

            foreach (info infoI in camInfo)
            {

                if (infoI.profileName == profile && infoI.webcam == webcam)
                {

                    if (property == "friendlyName") return infoI.friendlyName;
                    if (property == "areaDetection") return infoI.areaDetection;
                    if (property == "areaDetectionWithin") return infoI.areaDetectionWithin;
                    if (property == "areaOffAtMotion") return infoI.areaOffAtMotion;
                    if (property == "rectX") return infoI.rectX;
                    if (property == "rectY") return infoI.rectY;
                    if (property == "rectWidth") return infoI.rectWidth;
                    if (property == "rectHeight") return infoI.rectHeight;
                    if (property == "movementVal") return infoI.movementVal;
                    if (property == "alarmActive") return infoI.alarmActive;
                    if (property == "publishActive") return infoI.publishActive;

                    if (property == "pubImage") return infoI.pubImage;
                    if (property == "pubTime") return infoI.pubTime;
                    if (property == "pubHours") return infoI.pubHours;
                    if (property == "pubMins") return infoI.pubMins;
                    if (property == "pubSecs") return infoI.pubSecs;
                    if (property == "publishWeb") return infoI.publishWeb;
                    if (property == "publishLocal") return infoI.publishLocal;
                    if (property == "timerOn") return infoI.timerOn;
                    if (property == "fileURLPubWeb") return infoI.fileURLPubWeb;
                    if (property == "filenamePrefixPubWeb") return infoI.filenamePrefixPubWeb;
                    if (property == "cycleStampCheckedPubWeb") return infoI.cycleStampCheckedPubWeb;
                    if (property == "startCyclePubWeb") return infoI.startCyclePubWeb;
                    if (property == "endCyclePubWeb") return infoI.endCyclePubWeb;
                    if (property == "currentCyclePubWeb") return infoI.currentCyclePubWeb;
                    if (property == "stampAppendPubWeb") return infoI.stampAppendPubWeb;
                    if (property == "fileDirPubLoc") return infoI.fileDirPubLoc;
                    if (property == "filenamePrefixPubLoc") return infoI.filenamePrefixPubLoc;
                    if (property == "fileDirPubCust") return infoI.fileDirPubCust;
                    if (property == "cycleStampCheckedPubLoc") return infoI.cycleStampCheckedPubLoc;
                    if (property == "startCyclePubLoc") return infoI.startCyclePubLoc;
                    if (property == "endCyclePubLoc") return infoI.endCyclePubLoc;
                    if (property == "currentCyclePubLoc") return infoI.currentCyclePubLoc;
                    if (property == "stampAppendPubLoc") return infoI.stampAppendPubLoc;

                    if (property == "publishFirst") return infoI.publishFirst;
                    if (property == "lastPublished") return infoI.lastPublished;

                    if (property == "ipWebcamAddress") return infoI.ipWebcamAddress;
                    if (property == "ipWebcamUser") return infoI.ipWebcamUser;
                    if (property == "ipWebcamPassword") return infoI.ipWebcamPassword;


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

        public static void addCamera(rigItem p_cam)
        {
            rigItem r_item = new rigItem();
            r_item = p_cam;

            rig.Add(r_item);
        }

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

