using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace TeboCam
{
    public partial class ProfilesCntl : UserControl
    {
        public delegate void populateFromProfileDelegate(string profileName);
        populateFromProfileDelegate populateFromProfile;
        public delegate void cameraNewProfileDelegate();
        cameraNewProfileDelegate cameraNewProfile;
        public delegate void saveChangesDelegate();
        saveChangesDelegate saveChanges;
        Configuration configuration;

        public ProfilesCntl(populateFromProfileDelegate populate,
                            cameraNewProfileDelegate newProfile,
                            saveChangesDelegate save,
                            Configuration config)
        {
            InitializeComponent();
            populateFromProfile = populate;
            cameraNewProfile = newProfile;
            saveChanges = save;
            configuration = config;
        }

        private void btnRunProfileCommand_Click(object sender, EventArgs e)
        {
            string command = ProfileCommandList.SelectedItem.ToString();
            switch (command)
            {
                case "Vault profiles":
                    VaultProfiles();
                    break;
                case "New profile":
                    NewProfile();
                    break;
                case "Copy profile":
                    CopyProfile();
                    break;
                case "Rename profile":
                    RenameProfile();
                    break;
                case "Delete profile":
                    DeleteProfile();
                    break;
                case "Remove unused camera profiles":
                    RemoveUnusedCameraProfiles();
                    break;
                default:
                    break;
            }
        }

        private void profileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveChanges();
            ConfigurationHelper.SetCurrentProfileName( profileList.SelectedItem.ToString());
            ConfigurationHelper.LoadCurrentProfile(profileList.SelectedItem.ToString());
            configuration.profileInUse = profileList.SelectedItem.ToString();
            populateFromProfile(configuration.profileInUse);
            cameraNewProfile();
        }

        private void VaultProfiles()
        {
            if (!Directory.Exists(TebocamState.vaultFolder)) Directory.CreateDirectory(TebocamState.vaultFolder);

            string configVlt = TebocamState.vaultFolder + FileManager.configFile + "_" + DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture) + ".xml";
            string configXml = TebocamState.xmlFolder + FileManager.configFile + ".xml";
            File.Copy(configXml, configVlt, true);
            MessageBox.Show(FileManager.configFile + ".xml has been successfully vaulted in the vault folder.", "File Vaulted");
        }

        private void CopyProfile()
        {
            string tmpStr = InputBox("New Profile Name", "Copy Profile", "").ToLower().Replace(" ", "");

            if (tmpStr.Trim() != "")
            {
                ConfigurationHelper.copyProfile(profileList.SelectedItem.ToString(), tmpStr);
                ProfileListRefresh(ConfigurationHelper.GetCurrentProfileName());
            }
            else
            {
                MessageBox.Show("Profile name must have 1 or more characters.", "Error");
            }
        }

        private void RenameProfile()
        {

            string origName = profileList.SelectedItem.ToString();
            string tmpStr = InputBox("New Profile Name", "Rename Profile", "").Trim().Replace(" ", "");

            if (tmpStr.Trim() != "")
            {
                ConfigurationHelper.renameProfile(origName, tmpStr);
                ConfigurationHelper.SetCurrentProfileName(tmpStr);
                ConfigurationHelper.LoadCurrentProfile(tmpStr);
                ProfileListRefresh(tmpStr);
            }
            else
            {
                MessageBox.Show("Profile name must have 1 or more characters.", "Error");
            }
        }

        private void DeleteProfile()
        {
            ConfigurationHelper.deleteProfile(profileList.SelectedItem.ToString());
            profileList.Items.Clear();

            foreach (string profile in ConfigurationHelper.getProfileList())
            {
                profileList.Items.Add(profile);
            }

            profileList.SelectedIndex = 0;
        }

        private void RemoveUnusedCameraProfiles()
        {
            var connectedCams = CameraRig.ConnectedCameras.Select(x => x.camera.name).ToList<string>();
            //var deleteList = CameraRig.CameraSpecificInformation.Where(x => x.profileName == TebocamState.profileInUse && !connectedCams.Contains(x.webcam));
            var deleteList = configuration.appConfigs.Where(x => x.profileName == ConfigurationHelper.GetCurrentProfileName()).Where(x => !connectedCams.Contains(x.webcam));
            var webcamsToDelete = deleteList.Select(x => x.webcam).ToList<string>();
            webcamsToDelete.ForEach(x => CameraRig.removeCameraSpecificInformation(ConfigurationHelper.GetCurrentProfileName(), x));
        }

        public void ProfileListSelectFirst()
        {
            ProfileCommandList.SelectedIndex = 0;
        }

        public void ProfileListRefresh(string selectProfile)
        {
            profileList.Items.Clear();
            int tmpInt = 0;

            foreach (string profile in ConfigurationHelper.getProfileList())
            {
                profileList.Items.Add(profile);
                if (profile == selectProfile) tmpInt = profileList.Items.Count - 1;
            }

            profileList.SelectedIndex = tmpInt;
        }

        private void NewProfile()
        {
            string newProfile = InputBox("Profile Name", "New Profile", "").Trim().Replace(" ", "");

            if (newProfile.Trim() != "")
            {
                ConfigurationHelper.AddProfile(newProfile);
                ProfileListRefresh(ConfigurationHelper.GetCurrentProfileName());
            }
            else
            {
                MessageBox.Show("Profile name must have 1 or more characters.", "Error");
            }
        }

        public static string InputBox(string prompt, string title, string defaultValue)
        {
            InputBoxDialog ib = new InputBoxDialog();
            ib.FormPrompt = prompt;
            ib.FormCaption = title;
            ib.DefaultValue = defaultValue;
            ib.ShowDialog();
            string s = ib.InputResponse;
            ib.Close();
            return s;
        }

    }
}
