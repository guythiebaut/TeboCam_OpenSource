using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace TeboCam
{
    public class Ping
    {
        //todo these need wiring up
        public event EventHandler pulseEvent;
        public event EventHandler redrawGraph;
        public event EventHandler takePingPicture;
        public delegate void pingGraphDelegate(string a);


        public string pingGraphDate;
        double pingLast = time.secondsSinceStart();


        pingGraphDelegate pingGraph;
        Graph graph;
        IMail mail;
        Log log;
        string tmpFolder;
        string xmlFolder;
        string profileInUse;
        public bool pingedBefore = false;


        public Ping(IMail email, Log logger, Graph grph,
            string tempFolder, string xmlFld, string profile, pingGraphDelegate pinGraphDel)
        {
            graph = grph;
            mail = email;
            log = logger;
            tmpFolder = tempFolder;
            xmlFolder = xmlFld;
            profileInUse = profile;
            pingGraph = pinGraphDel;
        }

        public void Send(bool webcamAttached, Size windowSize)
        {
            if (
                (
                webcamAttached && ConfigurationHelper.GetCurrentProfile().ping
                && ConfigurationHelper.GetCurrentProfile().pingInterval > 0
                && !pingedBefore
                ) ||
                (
                webcamAttached && ConfigurationHelper.GetCurrentProfile().ping
                && ConfigurationHelper.GetCurrentProfile().pingInterval > 0
                && Math.Abs(pingLast - time.secondsSinceStart()) >= Convert.ToDouble(ConfigurationHelper.GetCurrentProfile().pingInterval * 60)
                )
            )
            {

                teboDebug.writeline(teboDebug.pingVal + 1);
                pulseEvent(null, new EventArgs());
                takePingPicture(null, new EventArgs());
                Thread.Sleep(2000);
                teboDebug.writeline(teboDebug.pingVal + 2);
                log.AddLine("Preparing ping email.");
                pingedBefore = true;
                mail.clearAttachments();
                log.AddLine("Attachments cleared.");
                graph.graphSeq++;

                if (graph.graphCurrentDate != time.currentDateYYYYMMDD())
                {
                    teboDebug.writeline(teboDebug.pingVal + 3);
                    string tmpDate = graph.graphCurrentDate;
                    pingGraphDate = time.currentDateYYYYMMDD();
                    pingGraph(pingGraphDate);
                    Size size = new Size(windowSize.Width, windowSize.Height);
                    GraphToSave.graphBitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb);
                    GraphToSave.graphBitmap.Save(tmpFolder + "graphCurrent" + graph.graphSeq.ToString() + ".jpg", ImageFormat.Jpeg);
                    log.AddLine("Adding graph attachment.");

                    if (File.Exists(tmpFolder + "graphCurrent" + graph.graphSeq.ToString() + ".jpg"))
                    {
                        mail.addAttachment(tmpFolder + "graphCurrent" + graph.graphSeq.ToString() + ".jpg");
                    }

                    pingGraphDate = tmpDate;
                    pingGraph(pingGraphDate);
                }
                else
                {
                    teboDebug.writeline(teboDebug.pingVal + 4);
                    redrawGraph(null, new EventArgs());
                    GraphToSave.graphBitmap.Save(tmpFolder + "graphCurrent" + graph.graphSeq.ToString() + ".jpg", ImageFormat.Jpeg);
                    log.AddLine("Adding graph attachment.");

                    if (File.Exists(tmpFolder + "graphCurrent" + graph.graphSeq.ToString() + ".jpg"))
                    {
                        mail.addAttachment(tmpFolder + "graphCurrent" + graph.graphSeq.ToString() + ".jpg");
                    }
                }

                teboDebug.writeline(teboDebug.pingVal + 5);
                pulseEvent(null, new EventArgs());
                log.WriteXMLFile(xmlFolder + "LogData" + ".xml", log);

                if (File.Exists(xmlFolder + "LogData.xml"))
                {
                    File.Copy(xmlFolder + "LogData.xml", tmpFolder + "pinglog" + graph.graphSeq.ToString() + ".xml", true);
                }

                log.AddLine("Adding log attachment.");

                if (File.Exists(tmpFolder + "pinglog" + graph.graphSeq.ToString() + ".xml"))
                {
                    mail.addAttachment(tmpFolder + "pinglog" + graph.graphSeq.ToString() + ".xml");
                }

                File.Copy(tmpFolder + "pingPicture.jpg", tmpFolder + "pingPicture" + graph.graphSeq.ToString() + ".jpg", true);
                log.AddLine("Adding image attachment.");

                if (File.Exists(tmpFolder + "pingPicture" + graph.graphSeq.ToString() + ".jpg"))
                {
                    mail.addAttachment(tmpFolder + "pingPicture" + graph.graphSeq.ToString() + ".jpg");
                }

                File.Delete(tmpFolder + "pingPicture.jpg");
                Thread.Sleep(2000);

                var eml = new EmailFields()
                {
                    SentBy = ConfigurationHelper.GetCurrentProfile().sentBy,
                    SentByName = ConfigurationHelper.GetCurrentProfile().sentByName,
                    SendTo = ConfigurationHelper.GetCurrentProfile().sendTo,
                    Subject = ConfigurationHelper.GetCurrentProfile().pingSubject,
                    BodyText = "Log and graph attached." + "Next ping email will be sent in " + ConfigurationHelper.GetCurrentProfile().pingInterval.ToString() + " minutes.",
                    ReplyTo = ConfigurationHelper.GetCurrentProfile().replyTo,
                    Attachments = true,
                    CurrentTime = time.secondsSinceStart(),
                    User = ConfigurationHelper.GetCurrentProfile().emailUser,
                    Password = ConfigurationHelper.GetCurrentProfile().emailPass,
                    SmtpHost = ConfigurationHelper.GetCurrentProfile().smtpHost,
                    SmtpPort = ConfigurationHelper.GetCurrentProfile().smtpPort,
                    EnableSsl = ConfigurationHelper.GetCurrentProfile().EnableSsl
                };

                mail.sendEmail(eml);

                //#todo too late to update pinglast?
                pingLast = time.secondsSinceStart();
                Thread.Sleep(2000);
                log.AddLine("Ping email sent.");

                //}

                teboDebug.writeline(teboDebug.pingVal + 6);
            }
        }
    }
}


