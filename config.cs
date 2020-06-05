using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace TeboCam
{
    public static class ConfigurationHelper
    {
        //public static ArrayList profiles = new ArrayList();

        public static List<configApplication> profiles; 
        private static int profileGiven = 0;
        private static configApplication CurrentProfile;
        private static string CurrentProfileName = "main";

        public static void LoadProfiles(List<configApplication> profs)
        {
            profiles = profs;
        }


        public static void LoadCurrentProfile(string profileName)
        {
            CurrentProfile = getProfile(profileName);
        }

        public static configApplication GetCurrentProfile()
        {
            return CurrentProfile;
        }

        public static string GetCurrentProfileName()
        {
            return CurrentProfileName;
        }

        public static void SetCurrentProfileName(string val)
        {
            CurrentProfileName = val;
        }

        public static void AddProfile()
        {
            configApplication data = new configApplication(new crypt());
            profiles.Add(data);
        }

        public static void AddProfile(string profileName)
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
            return profiles.FirstOrDefault(x => x.profileName.ToLower() == profileName.ToLower());
        }

        public static configApplication getProfile()
        {
            if (profileGiven <= profiles.Count)
            {
                return profiles[profileGiven - 1];
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
                        //TebocamState.configuration.Configs.Add(newData);
                        break;
                    }
                }


            }
            else
            {
                MessageBox.Show("Cannot copy profile as new name already exists.", "Error");
            }


        }

        public static void renameProfile(string currentName, string newName)
        {
            if (!profileExists(newName))
            {

                foreach (configApplication profile in profiles)
                {
                    if (profile.profileName == currentName)
                    {
                        profile.profileName = newName;

                        foreach (var camConfig in profile.camConfigs)
                        {
                            if (camConfig.profileName == currentName)
                            {
                                camConfig.profileName = newName;
                            }
                        }
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

        public static bool InfoForProfileWebcamExists(string profileName, string webcam)
        {
            return profiles.Where(x => x.profileName == profileName).Select(x => x.camConfigs).First().Where(x => x.webcam == webcam).Any();
        }
        
        public static configWebcam InfoForProfileWebcam(string profileName, string webcam)
        {
            return profiles.Where(x => x.profileName == profileName).Select(x => x.camConfigs).First().Where(x => x.webcam == webcam).First();
        }


    }

}
