using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace TeboCam
{
    public static class config
    {
        //public static ArrayList profiles = new ArrayList();

        public static List<configApplication> profiles = bubble.configuration.appConfigs;
        private static int profileGiven = 0;

        public static void addProfile()
        {
            configApplication data = new configApplication(new crypt());
            //data.configDataInit();
            profiles.Add(data);
        }

        public static void addProfile(string profileName)
        {

            if (!profileExists(profileName))
            {

                configApplication data = new configApplication(new crypt());
                //data.configDataInit();
                data.profileName = profileName.ToLower();
                profiles.Add(data);

            }
            else
            {
                MessageBox.Show("Cannot create profile as name already exists.", "Error");
            }

        }

        public static List<string> getProfileList()
        {
            return profiles.Select(x => x.profileName).ToList();
        }

        public static configApplication getProfile(string profileName)
        {
            return profiles.Where(x => x.profileName.ToLower() == profileName.ToLower()).FirstOrDefault();
        }

        public static configApplication getProfile()
        {
            if (profileGiven <= profiles.Count)
            {
                return (configApplication)profiles[profileGiven - 1];
            }
            return null;
        }


        public static void getFirstProfile()
        {
            profileGiven = 0;
        }

        public static bool moreProfiles()
        {

            if (profileGiven < profiles.Count)
            {
                profileGiven++;
                return true;
            }

            return false;

        }

        public static void deleteProfile(string profileName)
        {
            if (profileExists(profileName))
            {
                if (profiles.Count <= 1)
                {
                    MessageBox.Show("Cannot delete profile as at least one profile must exist.", "Error");
                }
                else
                {
                    for (int i = 0; i < profiles.Count; i++)
                    {
                        configApplication tmpData = (configApplication)profiles[i];
                        if (tmpData.profileName == profileName)
                        {
                            profiles.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }


        public static void copyProfile(string copyFrom, string copyTo)
        {

            if (!profileExists(copyTo))
            {

                //configData tmpData = (configData)profiles[i]
                foreach (configApplication data in profiles)
                {
                    if (data.profileName == copyFrom)
                    {
                        configApplication newData = (configApplication)data.Clone();
                        //configData newData = new configData();
                        //newData = data;
                        newData.profileName = copyTo;
                        profiles.Add(newData);
                        //bubble.configuration.Configs.Add(newData);
                        break;
                    }
                }


            }
            else
            {
                MessageBox.Show("Cannot copy profile as new name already exists.", "Error");
            }


        }

        public static void renameProfile(string profile, string NewName)
        {

            if (!profileExists(NewName))
            {

                foreach (configApplication data in profiles)
                {
                    if (data.profileName == profile)
                    {
                        data.profileName = NewName;
                        break;
                    }
                }

            }
            else
            {
                MessageBox.Show("Cannot rename profile as name already exists.", "Error");
            }

        }


        public static bool profileExists(string profileName)
        {
            bool profileExists = false;

            foreach (configApplication data in profiles)
            {
                if (data.profileName.ToLower() == profileName.ToLower())
                {
                    profileExists = true;
                    break;
                }
            }

            return profileExists;

        }


        public static void WebcamSettingsConfigDataPopulate()
        {
            foreach (configApplication profile in profiles)
            {
                profile.camConfigs.Clear();
                foreach (cameraSpecificInfo infoI in CameraRig.CamsInfo)
                {
                    if (infoI.profileName == profile.profileName)
                    {
                        configWebcam webcamConfig = new configWebcam();
                        webcamConfig.webcam = infoI.webcam;
                        webcamConfig.profileName = infoI.profileName;
                        webcamConfig.alarmActive = infoI.alarmActive;
                        webcamConfig.publishActive = infoI.publishActive;
                        webcamConfig.friendlyName = infoI.friendlyName;
                        webcamConfig.displayButton = infoI.displayButton;
                        webcamConfig.areaDetection = infoI.areaDetection;
                        webcamConfig.areaDetectionWithin = infoI.areaDetectionWithin;
                        webcamConfig.areaOffAtMotion = infoI.areaOffAtMotion;
                        webcamConfig.rectX = infoI.rectX;
                        webcamConfig.rectY = infoI.rectY;
                        webcamConfig.rectWidth = infoI.rectWidth;
                        webcamConfig.rectHeight = infoI.rectHeight;
                        webcamConfig.movementVal = infoI.movementVal;
                        webcamConfig.timeSpike = infoI.timeSpike;
                        webcamConfig.toleranceSpike = infoI.toleranceSpike;
                        webcamConfig.lightSpike = infoI.lightSpike;
                        webcamConfig.pubTime = infoI.pubTime;
                        webcamConfig.pubHours = infoI.pubHours;
                        webcamConfig.pubMins = infoI.pubMins;
                        webcamConfig.pubSecs = infoI.pubSecs;
                        webcamConfig.publishWeb = infoI.publishWeb;
                        webcamConfig.publishLocal = infoI.publishLocal;
                        webcamConfig.timerOn = infoI.timerOn;
                        webcamConfig.fileURLPubWeb = infoI.fileURLPubWeb;
                        webcamConfig.filenamePrefixPubWeb = infoI.filenamePrefixPubWeb;
                        webcamConfig.cycleStampCheckedPubLoc = infoI.cycleStampCheckedPubLoc;
                        webcamConfig.startCyclePubWeb = infoI.startCyclePubWeb;
                        webcamConfig.endCyclePubWeb = infoI.endCyclePubWeb;
                        webcamConfig.currentCyclePubWeb = infoI.currentCyclePubWeb;
                        webcamConfig.stampAppendPubWeb = infoI.stampAppendPubWeb;
                        webcamConfig.fileDirAlertLoc = infoI.fileDirAlertLoc;
                        webcamConfig.fileDirPubCust = infoI.fileDirPubCust;
                        webcamConfig.fileDirPubLoc = infoI.fileDirPubLoc;
                        webcamConfig.fileDirPubCust = infoI.fileDirPubCust;
                        webcamConfig.filenamePrefixPubLoc = infoI.filenamePrefixPubLoc;
                        webcamConfig.cycleStampCheckedPubWeb = infoI.cycleStampCheckedPubWeb;
                        webcamConfig.startCyclePubLoc = infoI.startCyclePubLoc;
                        webcamConfig.endCyclePubLoc = infoI.endCyclePubLoc;
                        webcamConfig.currentCyclePubLoc = infoI.currentCyclePubLoc;
                        webcamConfig.stampAppendPubLoc = infoI.stampAppendPubLoc;
                        webcamConfig.ipWebcamAddress = infoI.ipWebcamAddress;
                        webcamConfig.ipWebcamUser = infoI.ipWebcamUser;
                        webcamConfig.ipWebcamPassword = infoI.ipWebcamPassword;
                        profile.camConfigs.Add(webcamConfig);
                    }
                }
            }
        }

        public static void WebcamSettingsPopulate()
        {
            foreach (configApplication profile in profiles)
            {
                foreach (configWebcam webcamConfig in profile.camConfigs)
                {
                    CameraRig.addInfo(CameraRig.infoEnum.webcam, webcamConfig.webcam);
                    CameraRig.addInfo(CameraRig.infoEnum.profileName, webcamConfig.profileName);
                    CameraRig.addInfo(CameraRig.infoEnum.alarmActive, webcamConfig.alarmActive);
                    CameraRig.addInfo(CameraRig.infoEnum.publishActive, webcamConfig.publishActive);
                    CameraRig.addInfo(CameraRig.infoEnum.friendlyName, webcamConfig.friendlyName);
                    CameraRig.addInfo(CameraRig.infoEnum.displayButton, webcamConfig.displayButton);
                    CameraRig.addInfo(CameraRig.infoEnum.areaDetection, webcamConfig.areaDetection);
                    CameraRig.addInfo(CameraRig.infoEnum.areaDetectionWithin, webcamConfig.areaDetectionWithin);
                    CameraRig.addInfo(CameraRig.infoEnum.areaOffAtMotion, webcamConfig.areaOffAtMotion);
                    CameraRig.addInfo(CameraRig.infoEnum.rectX, webcamConfig.rectX);
                    CameraRig.addInfo(CameraRig.infoEnum.rectY, webcamConfig.rectY);
                    CameraRig.addInfo(CameraRig.infoEnum.rectWidth, webcamConfig.rectWidth);
                    CameraRig.addInfo(CameraRig.infoEnum.rectHeight, webcamConfig.rectHeight);
                    CameraRig.addInfo(CameraRig.infoEnum.movementVal, webcamConfig.movementVal);
                    CameraRig.addInfo(CameraRig.infoEnum.timeSpike, webcamConfig.timeSpike);
                    CameraRig.addInfo(CameraRig.infoEnum.toleranceSpike, webcamConfig.toleranceSpike);
                    CameraRig.addInfo(CameraRig.infoEnum.lightSpike, webcamConfig.lightSpike);
                    CameraRig.addInfo(CameraRig.infoEnum.pubTime, webcamConfig.pubTime);
                    CameraRig.addInfo(CameraRig.infoEnum.pubHours, webcamConfig.pubHours);
                    CameraRig.addInfo(CameraRig.infoEnum.pubMins, webcamConfig.pubMins);
                    CameraRig.addInfo(CameraRig.infoEnum.pubSecs, webcamConfig.pubSecs);
                    CameraRig.addInfo(CameraRig.infoEnum.publishWeb, webcamConfig.publishWeb);
                    CameraRig.addInfo(CameraRig.infoEnum.publishLocal, webcamConfig.publishLocal);
                    CameraRig.addInfo(CameraRig.infoEnum.timerOn, webcamConfig.timerOn);
                    CameraRig.addInfo(CameraRig.infoEnum.fileURLPubWeb, webcamConfig.fileURLPubWeb);
                    CameraRig.addInfo(CameraRig.infoEnum.filenamePrefixPubWeb, webcamConfig.filenamePrefixPubWeb);
                    CameraRig.addInfo(CameraRig.infoEnum.cycleStampCheckedPubWeb, webcamConfig.cycleStampCheckedPubWeb);
                    CameraRig.addInfo(CameraRig.infoEnum.startCyclePubWeb, webcamConfig.startCyclePubWeb);
                    CameraRig.addInfo(CameraRig.infoEnum.endCyclePubWeb, webcamConfig.endCyclePubWeb);
                    CameraRig.addInfo(CameraRig.infoEnum.currentCyclePubWeb, webcamConfig.currentCyclePubWeb);
                    CameraRig.addInfo(CameraRig.infoEnum.stampAppendPubWeb, webcamConfig.stampAppendPubWeb);
                    CameraRig.addInfo(CameraRig.infoEnum.fileAlertPubLoc, webcamConfig.fileDirAlertLoc);
                    CameraRig.addInfo(CameraRig.infoEnum.fileAlertPubCust, webcamConfig.fileDirAlertCust);
                    CameraRig.addInfo(CameraRig.infoEnum.fileDirPubLoc, webcamConfig.fileDirPubLoc);
                    CameraRig.addInfo(CameraRig.infoEnum.fileDirPubCust, webcamConfig.fileDirPubCust);
                    CameraRig.addInfo(CameraRig.infoEnum.filenamePrefixPubLoc, webcamConfig.filenamePrefixPubLoc);
                    CameraRig.addInfo(CameraRig.infoEnum.cycleStampCheckedPubLoc, webcamConfig.cycleStampCheckedPubLoc);
                    CameraRig.addInfo(CameraRig.infoEnum.startCyclePubLoc, webcamConfig.startCyclePubLoc);
                    CameraRig.addInfo(CameraRig.infoEnum.endCyclePubLoc, webcamConfig.endCyclePubLoc);
                    CameraRig.addInfo(CameraRig.infoEnum.currentCyclePubLoc, webcamConfig.currentCyclePubLoc);
                    CameraRig.addInfo(CameraRig.infoEnum.stampAppendPubLoc, webcamConfig.stampAppendPubLoc);
                    CameraRig.addInfo(CameraRig.infoEnum.ipWebcamAddress, webcamConfig.ipWebcamAddress);
                    CameraRig.addInfo(CameraRig.infoEnum.ipWebcamUser, webcamConfig.ipWebcamUser);
                    CameraRig.addInfo(CameraRig.infoEnum.ipWebcamPassword, webcamConfig.ipWebcamPassword);
                }
            }
        }

    }

}
