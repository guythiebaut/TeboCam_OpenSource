using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TeboCam
{
    public class WaitForCam
    {
        public delegate Camera OpenVideoSourceDelegate(VideoCaptureDevice source, AForge.Video.MJPEGStream ipStream, Boolean ip, int cameraNo);
        OpenVideoSourceDelegate OpenVideoSource;
        Configuration configuration;
        FilterInfoCollection filters;
        public delegate void PublishDelegate(int buttonNo);
        PublishDelegate publishCam;

        public WaitForCam(Configuration configuration, OpenVideoSourceDelegate OpenVideoSource, PublishDelegate publishCam)
        {
            this.configuration = configuration;
            this.OpenVideoSource = OpenVideoSource;
            this.publishCam = publishCam;
        }

        public void wait(object sender, DoWorkEventArgs e)
        {
            bool nocam;
            //List<cameraSpecificInfo> expectedCameras = CameraRig.cameraCredentialsListedUnderProfile(TebocamState.profileInUse);
            var profile = configuration.appConfigs.Where(x => x.profileName == ConfigurationHelper.GetCurrentProfileName()).First();
            //*****************************
            //IP Webcams
            //*****************************

            //find if any webcams are present
            for (int i = 0; i < profile.camConfigs.Count; i++)
            {
                //we have an ip webcam in the profile
                if (profile.camConfigs[i].ipWebcamAddress != string.Empty)
                {
                    IPAddress parsedIpAddress;
                    Uri parsedUri;
                    //check that the url resolves

                    //https://www.codeproject.com/Articles/1017223/CaptureManager-SDK-Capturing-Recording-and-Streami
                    //https://test-videos.co.uk/vids/bigbuckbunny/mp4/h264/1080/Big_Buck_Bunny_1080_10s_1MB.mp4

                    if (Uri.TryCreate(profile.camConfigs[i].ipWebcamAddress, UriKind.Absolute, out parsedUri) && IPAddress.TryParse(parsedUri.DnsSafeHost, out parsedIpAddress))
                    {
                        var pingSender = new System.Net.NetworkInformation.Ping();
                        PingReply reply = pingSender.Send(parsedIpAddress);
                        //is ip webcam running?
                        if (reply.Status == IPStatus.Success)
                        {
                            AForge.Video.MJPEGStream stream = new AForge.Video.MJPEGStream(profile.camConfigs[i].ipWebcamAddress);

                            if (profile.camConfigs[i].ipWebcamUser != string.Empty)
                            {
                                stream.Login = profile.camConfigs[i].ipWebcamUser;
                                stream.Password = profile.camConfigs[i].ipWebcamPassword;
                            }

                            Camera cam = OpenVideoSource(null, stream, true, -1);
                            cam.frameRateTrack = ConfigurationHelper.GetCurrentProfile().framerateTrack;

                        }
                    }
                }
            }

            //*****************************
            //IP Webcams
            //*****************************

            //*****************************
            //USB Webcams
            //*****************************
            nocam = false;

            try
            {
                filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (filters.Count == 0) nocam = true;
            }
            catch (ApplicationException)
            {
                nocam = true;
            }


            //we have camera(s) attached so let's connect it/them
            if (!nocam)
            {
                for (int i = 0; i < filters.Count; i++)
                {
                    for (int c = 0; c < profile.camConfigs.Count; c++)
                    {
                        if (profile.camConfigs[c].ipWebcamAddress == string.Empty && filters[i].MonikerString == profile.camConfigs[c].webcam)
                        {
                            Thread.Sleep(1000);
                            VideoCaptureDevice localSource = new VideoCaptureDevice(profile.camConfigs[c].webcam);
                            Camera cam = OpenVideoSource(localSource, null, false, -1);
                            cam.frameRateTrack = ConfigurationHelper.GetCurrentProfile().framerateTrack;
                        }
                    }
                }
            }

            //*****************************
            //USB Webcams
            //*****************************

            CameraRig.ConnectedCameras.ForEach(x => x.camera.frameRateTrack = ConfigurationHelper.GetCurrentProfile().framerateTrack);


            var publishButtonsSet = 0;
            foreach (var connectedCamera in CameraRig.ConnectedCameras)
            {
                if(connectedCamera.camera.publishActive)
                {
                    publishCam(connectedCamera.displayButton);
                    publishButtonsSet++;
                }
            }

            if (publishButtonsSet == 0 && CameraRig.ConnectedCameras.Any())
            {
                publishCam(CameraRig.ConnectedCameras.First().displayButton);
            }
        }
    }
}
