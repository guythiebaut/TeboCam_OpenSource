 using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using AForge.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace TeboCam
{
    public static class ApiProcess
    {
        private const int databaseTimeOutCount = 5;
        private static string update_result = "";
        public static IMail mail;
        private static bool webCredsJustChecked = false;
        public static bool webFirstTimeThru = true;
        public static int ApiAuthenticationAttemptCount = 0;
        public static bool apiCredentialsValidated = false;
        public static int webUpdLastChecked = 0;
        public static bool LicensedToConnectToApi = false;
        public static List<string> GuidSeen =  new  List<string>();

        const string securityCodeStr = "111+$";
        const string shutDownCmdStr = "^shutdown";
        const string movementStatsCmdStr = "^movementstats$";
        const string deleteAndshutDownCmdStr = "^delete_and_shutdown";
        const string restartCmdStr = "^restart";
        const string activateCmdStr = "^activate$";
        const string inactivateCmdStr = "^inactivate$";
        const string pingonCmdStr = "^pingon";
        const string pingoffCmdStr = "^pingoff";
        const string pollCmdStr = "^poll";
        const string logCmdStr = "^log$";
        const string publishCmdStr = "^publish$";
        const string publishoffCmdStr = @"^publishoff$";
        const string deleteImagesCmdStr = @"^deleteimages$";
        const string statsCmdStr = @"^stats$";
        const string statsAllCmdStr = @"^all_stats$";
        const string lockdownCmdStr = @"^lockdown$";
        const string unlockCmdStr = @"^unlock$";
        const string graphCmdStr = @"^graphs$";
        const string imageCmdStr = @"^images$";

        //private static string user = config.GetCurrentProfile().webUser;
        //private static string instance = config.GetCurrentProfile().webInstance;


        private static void Authenticate(bool renew)
        {

            var endPoint = ConfigurationHelper.GetCurrentProfile().UseRemoteEndpoint ?
                ConfigurationHelper.GetCurrentProfile().AuthenticateEndpoint :
                ConfigurationHelper.GetCurrentProfile().LocalAuthenticateEndpoint;
            var user = ConfigurationHelper.GetCurrentProfile().webUser;
            var password = ConfigurationHelper.GetCurrentProfile().webPass;
            var instance = ConfigurationHelper.GetCurrentProfile().webInstance;
            apiCredentialsValidated = API.LogOn(endPoint, user, password, instance, renew);
        }

        private static bool matched(string doesThis, string matchThis)
        {
            return regex.match(matchThis, doesThis);
        }

        private static bool runNow(DateTime? runAt)
        {
            if (runAt == null)
            {
                return true;
            }

            return runAt?.ToUniversalTime() <= DateTime.UtcNow;
        }

        private static bool hasRunTime(DateTime? runAt)
        {
            return runAt != null;
        }

        private static void ProcessCommand(string commandToRun, string parms, string guid, Preferences preferences, CommandQueueElement rawCommand)
        {
            bool securityCode = matched(parms, securityCodeStr);
            bool shutDownCmd = matched(commandToRun, shutDownCmdStr);
            bool movementStats = matched(commandToRun, movementStatsCmdStr);
            bool deleteAndshutDownCmd = matched(commandToRun, deleteAndshutDownCmdStr);
            bool restartCmd = matched(commandToRun, restartCmdStr);
            bool activateCmd = matched(commandToRun, activateCmdStr);
            bool inactivateCmd = matched(commandToRun, inactivateCmdStr);
            bool pingonCmd = matched(commandToRun, pingonCmdStr);
            bool pingoffCmd = matched(commandToRun, pingoffCmdStr);
            bool pollCmd = matched(commandToRun, pollCmdStr);
            bool logCmd = matched(commandToRun, logCmdStr);
            bool publishCmd = matched(commandToRun, publishCmdStr);
            bool publishoffCmd = matched(commandToRun, publishoffCmdStr);
            bool deleteImagesCmd = matched(commandToRun, deleteImagesCmdStr);
            bool statsCmd = matched(commandToRun, statsCmdStr);
            bool statsAllCmd = matched(commandToRun, statsAllCmdStr);
            bool lockdownCmd = matched(commandToRun, lockdownCmdStr);
            bool unlockCmd = matched(commandToRun, unlockCmdStr);
            bool graphCmd = matched(commandToRun, graphCmdStr);
            bool imageCmd = matched(commandToRun, imageCmdStr);


            if (runNow(rawCommand.requestedRunDtm))
            {
                CleanUp(guid);
            }

            if (DisregardCommand(rawCommand))
            {
                return;
            }

            if (deleteImagesCmd) DeleteImages();
            if (deleteAndshutDownCmd) DeleteImagesAndShutDown(securityCode);
            if (shutDownCmd || restartCmd) ShutdownRestart(shutDownCmd, securityCode, rawCommand.requestedRunDtm, GuidSeen.Contains(guid));
            if (activateCmd) Activate();
            if (inactivateCmd) Inactivate();
            if (pingonCmd) PingOn(commandToRun, preferences.pinger);
            if (pingoffCmd) PingOff(preferences.pinger);
            if (pollCmd) Poll(commandToRun, preferences);
            if (logCmd) Log();
            if (movementStats) MovementStats(guid, preferences, parms);
            if (statsCmd) Statistics(guid);
            if (statsAllCmd) StatsAll(guid);
            if (graphCmd) Graph(preferences.graph, guid, parms);
            if (imageCmd) Image(guid);

            if (hasRunTime(rawCommand.requestedRunDtm))
            {
                //We want to store the guid in a list of already seen guids
                //so that we don't keep processing the same initial commands until the
                //runAt time arrives.

                GuidSeen.Add(guid);
            }
        }

        private static bool DisregardCommand(CommandQueueElement rawCommand)
        {
            if (rawCommand.addedToQueueDtm == null)
            {
                return false;
            }

            var disregard = ConfigurationHelper.GetCurrentProfile().disCommOnline;
            var olderThan = ConfigurationHelper.GetCurrentProfile().disCommOnlineSecs;

            var utcAdded = rawCommand.addedToQueueDtm?.ToUniversalTime();
            var seccondsDiff = Math.Abs((int)(utcAdded - DateTime.UtcNow)?.TotalSeconds);

            return disregard && seccondsDiff > olderThan;
        }

        public class fileObject
        {
            public string fileName;
            public string filePath;
        };

        private static void Image(string guid)
        {
            TebocamState.log.AddLine("Images request sent to API");
            List<Bitmap> bitmapsOld = new List<Bitmap>();
            var bitmaps = new List<List<object>>();
            var files = new List<fileObject>();
            CameraRig.ConnectedCameras.ForEach(x => bitmaps.Add(new List<object> { x.friendlyName, x.camera.pubFrame.Clone() }));

            foreach (var bitmap in bitmaps)
            {
                string filename = $"{TebocamState.apiCameraImgPrefix}{Guid.NewGuid()}{TebocamState.ImgSuffix}";
                string localFilename = Path.Combine(TebocamState.tmpFolder, filename);
                ImageProcessor.SaveBitmap((Bitmap)bitmap[1], localFilename, TebocamState.tebowebException);

                var pubdDir = ConfigurationHelper.GetCurrentProfile().pubFtpRoot;

                ftp.Upload(localFilename,
                    pubdDir,
                    ConfigurationHelper.GetCurrentProfile().ftpUser,
                    ConfigurationHelper.GetCurrentProfile().ftpPass,
                    5
                    );

                files.Add(
                    new fileObject()
                    {
                        fileName = bitmap[0].ToString(),
                        filePath = $"{ ConfigurationHelper.GetCurrentProfile().PickupDirectory.TrimEnd('/')}{"/"}{ filename }"
                    }
                    );
            }

            if (files.Any()) API.UpdateCommandResult("images", files, guid);
            TebocamState.log.AddLine("Images ready for pickup from API");
        }

        private static void Graph(Graph graph, string guid, string parms)
        {
            int days = 0;
            int.TryParse(parms, out days);
            var yyyymmdd = DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            int today = 0;
            int.TryParse(yyyymmdd, out today);
            int datesFrom = today - days;
            TebocamState.log.AddLine("Graphs request sent to API");

            var dates = graph.getGraphDatesAsStrings().Where(x => Convert.ToInt32(x) >= datesFrom);
            List<List<string>> files = new List<List<string>>();
            foreach (var date in dates)
            {
                Bitmap graphBitmap = new Bitmap(476, 228, PixelFormat.Format24bppRgb);
                graphBitmap = graph.GetGraphFromDate(date, new Size(graphBitmap.Width, graphBitmap.Height), TebocamState.tebowebException, string.Empty);
                string filename = $"{TebocamState.apiGraphImgPrefix}{Guid.NewGuid()}{TebocamState.ImgSuffix}";
                string localFilename = Path.Combine(TebocamState.tmpFolder, filename);
                graphBitmap.Save(localFilename);
                ftp.Upload(localFilename, ConfigurationHelper.GetCurrentProfile().ftpRoot, ConfigurationHelper.GetCurrentProfile().ftpUser, ConfigurationHelper.GetCurrentProfile().ftpPass, 5);
                List<string> dataPoint = new List<string> { date, $"{ ConfigurationHelper.GetCurrentProfile().PickupDirectory.TrimEnd('/')}{"/"}{ filename }" };
                files.Add(dataPoint);
            }

            if (files.Any()) API.UpdateCommandResult("graphs", files, guid);
            TebocamState.log.AddLine("Graphs ready for pickup from API");
        }



        private static void DeleteImages()
        {
            TebocamState.log.AddLine("ClearFtp from API");
            FileManager.InitialiseDeletionRegex(true);
            FileManager.clearFtp();
            FileManager.clearFiles(TebocamState.thumbFolder);
            FileManager.clearFiles(TebocamState.imageFolder);
        }

        private static void DeleteImagesAndShutDown(bool securityCode)
        {
            //#TODO need to take into account security code
            if (!securityCode)
            {
                IncorrectSecurityCode("Web request delete and shutdown error - 111 code not issued!");
                return;
            }

            Inactivate();
            var shutdownDelegate = new TeboCamDelegates.EventDelegate<RunWorkerCompletedEventArgs>(ShutDown);
            TebocamState.log.AddLine("ClearFtp from API");
            FileManager.InitialiseDeletionRegex(true);
            FileManager.clearFiles(TebocamState.thumbFolder);
            FileManager.clearFiles(TebocamState.imageFolder);
            FileManager.clearFtp(shutdownDelegate);
        }

        private static void MovementStats(string guid, Preferences preferences, string parms)
        {
            int daysToExport = 1;
            int.TryParse(parms, out daysToExport);
            daysToExport = daysToExport == 0 ? 1 : daysToExport;
            TebocamState.log.AddLine("Movement stats request from API");
            API.UpdateCommandResult("movementstats", preferences.graph.HistoryUiFriendly(daysToExport), guid);
            //API.UpdateCommandResult(preferences.graph, guid);
            TebocamState.log.AddLine("Movement stats update sent to API");
        }

        private static void Statistics(string guid)
        {
            TebocamState.log.AddLine("Stats request from API");
            var statsForCams = new List<statistics.movementResults>();
            TebocamState.log.AddLine("Preparing stats update for API");
            CameraRig.ConnectedCameras.ForEach(x => statsForCams.Add(statistics.statsForCam(x.camera.camNo, ConfigurationHelper.GetCurrentProfileName(), "Publish", x.friendlyName)));
            API.UpdateCommandResult("stats", statsForCams, guid);
            TebocamState.log.AddLine("Stats update sent to API");
        }

        private static void StatsAll(string guid)
        {
            //number of images on computer
            //number of images on web
            //last ping for command was at 
            //time up
            //last restart time
        }

        private static void ShutdownRestart(bool shutDown, bool securityCode, DateTime? runAt, bool firstTime)
        {
            teboDebug.writeline(teboDebug.webUpdateVal + 12);
            if (!securityCode)
            {
                IncorrectSecurityCode("Web request restart/shutdown error - 111 code not issued!");
                return;
            }

            if (!firstTime)
            {
                if (!runNow(runAt))
                {
                    teboDebug.writeline(teboDebug.webUpdateVal + 13);
                    var message = string.Format("Web request restart/shutdown sceduled for {0}", runAt.ToString());
                    TebocamState.log.AddLine(message);
                    TebocamState.log.AddLine("Motion detection inactivated.");
                    Movement.MotionDetectionInactivate();
                }

                teboDebug.writeline(teboDebug.webUpdateVal + 13);
                TebocamState.log.AddLine("Web request restart/shutdown started...");
                TebocamState.log.AddLine("Motion detection inactivated.");
                Movement.MotionDetectionInactivate();
                TebocamState.log.AddLine("Config data saved.");
                TebocamState.configuration.WriteXmlFile(TebocamState.xmlFolder + FileManager.configFile + ".xml", TebocamState.configuration);
                API.UpdateInstance("Off");
            }

            if (shutDown)
            {
                if (!runNow(runAt))
                {
                    return;
                }

                TebocamState.log.AddLine("Shutdown from API");
                ShutDown();
            }
            else
            {
                TebocamState.log.AddLine("Restart from API");
                Restart();
            }
        }

        public static void IncorrectSecurityCode(string message)
        {
            teboDebug.writeline(teboDebug.webUpdateVal + 14);
            TebocamState.log.AddLine(message);
        }

        public static void ShutDown(object sender, RunWorkerCompletedEventArgs e)
        {
            ShutDown();
        }

        public static void ShutDown()
        {
            System.Diagnostics.Process.Start("shutdown", "/f /s /t 0");
        }

        public static void Restart()
        {
            System.Diagnostics.Process.Start("shutdown", "/f /r /t 0");
        }

        private static void Activate()
        {
            TebocamState.log.AddLine("Activate from API");
            teboDebug.writeline(teboDebug.webUpdateVal + 15);
            TebocamState.log.AddLine("Web request motion detection activated.");
            API.UpdateInstance("Active");
            Movement.MotionDetectionActivate();
        }

        private static void Inactivate()
        {
            TebocamState.log.AddLine("Inactivate from API");
            teboDebug.writeline(teboDebug.webUpdateVal + 16);
            TebocamState.log.AddLine("Web request motion detection inactivated.");
            API.UpdateInstance("Inactive");
            Movement.MotionDetectionInactivate();
        }

        private static void PingOn(string tmpStr, Ping ping)
        {
            TebocamState.log.AddLine("PingOn from API");
            teboDebug.writeline(teboDebug.webUpdateVal + 17);
            ConfigurationHelper.GetCurrentProfile().ping = true;
            ping.pingedBefore = false;

            TebocamState.log.AddLine("Web request ping activated.");

            if (tmpStr.Trim().Length > 6)
            {
                teboDebug.writeline(teboDebug.webUpdateVal + 18);
                string trString = tmpStr.Trim();
                string Num = LeftRightMid.Right(trString, trString.Length - 6).Trim();
                if (Valid.IsNumeric(Num))
                {
                    teboDebug.writeline(teboDebug.webUpdateVal + 19);
                    Num = Valid.verifyInt(Num, 1, 9999, ConfigurationHelper.GetCurrentProfile().pingInterval.ToString());
                    TebocamState.log.AddLine("Web request ping every " + Num + " minutes.");
                    ConfigurationHelper.GetCurrentProfile().pingInterval = Convert.ToInt32(Num);
                }
            }

            teboDebug.writeline(teboDebug.webUpdateVal + 20);
        }

        private static void PingOff(Ping ping)
        {
            TebocamState.log.AddLine("PingOff from API");
            teboDebug.writeline(teboDebug.webUpdateVal + 21);
            ConfigurationHelper.GetCurrentProfile().ping = false; ;
            ping.pingedBefore = false;
            TebocamState.log.AddLine("Web request ping inactivated.");
        }

        private static void Poll(string tmpStr, Preferences preferences)
        {
            TebocamState.log.AddLine("Poll from API");
            teboDebug.writeline(teboDebug.webUpdateVal + 22);
            string trString = tmpStr.Trim();
            string Num = LeftRightMid.Right(trString, trString.Length - 4).Trim();
            if (Valid.IsNumeric(Num))
            {
                teboDebug.writeline(teboDebug.webUpdateVal + 23);
                Num = Valid.verifyInt(Num, 5, 9999, "5");
                preferences.onlineSettings.GetSqlPoll().Text = Num;
                TebocamState.log.AddLine("Web request poll every " + Num + " seconds.");
                ConfigurationHelper.GetCurrentProfile().webPoll = Convert.ToInt32(Num);
            }

        }

        private static void Log()
        {
            TebocamState.log.AddLine("Log from API");
            teboDebug.writeline(teboDebug.webUpdateVal + 24);
            TebocamState.log.AddLine("Web log request sent to database.");
        }

        private static void CleanUp(string guid)
        {
            Token.commands.Remove(Token.commands.FirstOrDefault(x => x.commandGuid == guid));
            API.ClearCommands(new List<string>() { guid });
        }

        private static bool TimeToRenewToken(int minutesSafetyMargin)
        {
            DateTime now = DateTime.Now;
            DateTime expires = DateTime.Parse(Token.TokenForSession.validTo);
            DateTime renewBefore = expires.AddMinutes(-minutesSafetyMargin);

            return now >= renewBefore;
        }

        //todo if the authentication is unsuccessfull - email and try again every 10 minutes or renew token?
        public static void webUpdate(Preferences preferences)
        {
            if (
                LicensedToConnectToApi &&
                ApiAuthenticationAttemptCount < databaseTimeOutCount &&
                ConfigurationHelper.GetCurrentProfile().webUpd &&
                (
                    (webCredsJustChecked ||
                    (time.secondsSinceStart() - webUpdLastChecked > ConfigurationHelper.GetCurrentProfile().webPoll)) ||
                    webFirstTimeThru
                )
                )
            {
                if (!apiCredentialsValidated)
                {
                    TeboWeb.PulseEvents.PulseEvent();
                    TebocamState.log.AddLine("Checking API credentials...");
                    Authenticate(false);
                    webUpdLastChecked = time.secondsSinceStart();
                    ApiAuthenticationAttemptCount++;
                    if (apiCredentialsValidated)
                    {
                        TebocamState.log.AddLine("API authentication passed.");
                        webCredsJustChecked = true;
                    }
                    if (ApiAuthenticationAttemptCount >= databaseTimeOutCount)
                    {
                        TebocamState.log.AddLine("API authentication attempted " + databaseTimeOutCount.ToString() +
                                          " times and found to be incorrect!");
                    }
                }
                else
                {
                    webUpdLastChecked = time.secondsSinceStart();
                    TeboWeb.PulseEvents.PulseEvent();
                    webCredsJustChecked = false;
                    ApiAuthenticationAttemptCount = 0;

                    if (webFirstTimeThru)
                    {
                        teboDebug.writeline(teboDebug.webUpdateVal + 7);
                        API.UpdateInstance(TebocamState.Alert.on ? "Active" : "Inactive");
                        webFirstTimeThru = false;
                    }

                    int minutesSafetyMargin = 30;

                    if (TimeToRenewToken(minutesSafetyMargin))
                    {
                        TebocamState.log.AddLine($"API token expires in {minutesSafetyMargin} minutes, renewing token.");
                        TebocamState.log.AddLine("Checking API credentials...");
                        Authenticate(true);
                        TebocamState.log.AddLine("API authentication passed.");
                    }


                    if (!API.RetrieveCommands(1))
                    {
                        return;
                    }
                    else
                    {
                        if (Token.commands.Count == 0) return;

                        for (int i = Token.commands.Count() - 1; i >= 0; i--)
                        //foreach (var command in Token.commands)
                        {
                            if (Token.commands[i].emailConfirmation)
                            {
                                teboDebug.writeline(teboDebug.webUpdateVal + 6);

                                var eml = new EmailFields()
                                {
                                    SentBy = ConfigurationHelper.GetCurrentProfile().sentBy,
                                    SentByName = ConfigurationHelper.GetCurrentProfile().sentByName,
                                    SendTo = ConfigurationHelper.GetCurrentProfile().sendTo,
                                    Subject = "Online Request Confirmation",
                                    BodyText = @"'" + Token.commands[i].commandToRun + @"'" + " being actioned.",
                                    ReplyTo = ConfigurationHelper.GetCurrentProfile().replyTo,
                                    Attachments = false,
                                    CurrentTime = time.secondsSinceStart(),
                                    User = ConfigurationHelper.GetCurrentProfile().emailUser,
                                    Password = ConfigurationHelper.GetCurrentProfile().emailPass,
                                    SmtpHost = ConfigurationHelper.GetCurrentProfile().smtpHost,
                                    SmtpPort = ConfigurationHelper.GetCurrentProfile().smtpPort,
                                    EnableSsl = ConfigurationHelper.GetCurrentProfile().EnableSsl
                                };

                                mail.sendEmail(eml);
                                TebocamState.log.AddLine("Online Request Confirmation email sent.");
                            }

                            ProcessCommand(Token.commands[i].commandToRun, Token.commands[i].parms, Token.commands[i].commandGuid, preferences, Token.commands[i]);
                        }
                    }
                    TeboWeb.PulseEvents.PulseEvent();
                }
            }
        }
    }
}
