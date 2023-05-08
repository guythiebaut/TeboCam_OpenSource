using System;
using System.ComponentModel;
using System.Media;


namespace TeboCam
{
    public static class Sound
    {
        private static SoundPlayer player = new SoundPlayer();

        private static void LoadSoundCompleted(object sender, AsyncCompletedEventArgs args)
        {
            player.Play();
        }

        public static void RingMyBell(bool test)
        {
            if (ConfigurationHelper.GetCurrentProfile().soundAlertOn || test)
            {
                try
                {
                    player.LoadCompleted -= new AsyncCompletedEventHandler(LoadSoundCompleted);
                    player.LoadCompleted += new AsyncCompletedEventHandler(LoadSoundCompleted);
                    player.SoundLocation = ConfigurationHelper.GetCurrentProfile().soundAlert;
                    player.LoadAsync();
                }
                catch (Exception e)
                {
                    TebocamState.tebowebException.LogException(e);
                }
            }
        }
    }
}
