using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;


namespace TeboCam
{
    public class Update_Version_Being_Used
    {
        public const string updateVersion = "201202022100";
    }

    public static class TebocamState
    {
        public static string imageParentFolder = Application.StartupPath + @"\images\";
        public static string imageFolder = imageParentFolder + @"fullSize\";
        public static string thumbFolder = imageParentFolder + @"thumb\";
        public static string logFolder = Application.StartupPath + @"\logs\";
        public static string exceptionFolder = Application.StartupPath + @"\exceptions\";
        public static string xmlFolder = Application.StartupPath + @"\xml\";
        public static string tmpFolder = Application.StartupPath + @"\temp\";
        public static string resourceFolder = Application.StartupPath + @"\resources\";
        public static string resourceDownloadFolder = resourceFolder + @"download\";
        public static string vaultFolder = Application.StartupPath + @"\vault\";
        public static string pulseApp = Application.StartupPath + @"\FreezeGuard.exe";
        public static string pulseProcessName = "FreezeGuard";
        public const string thumbPrefix = "tmb";
        public const string apiGraphImgPrefix = "apiGraphImg";
        public const string apiCameraImgPrefix = "apiCameraImg";
        public const string ImgSuffix = ".jpg";
        public const string mosaicFile = "mosaic.jpg";
        public static bool testImagePublishFirst = false;
        public static List<ImagePublishData> testImagePublishData = new List<ImagePublishData>();
        public static Movement.AlertClass Alert = new Movement.AlertClass();
        public static Configuration configuration;
        public static Log log = new Log();
        public static IException tebowebException;
        public static ArrayList training = new ArrayList();
    }

    public class ImagePublishData
    {
        public int Sequence;
        public int MotionLevel;
        public int LowestValueOverTime;
        public string ImageFile;
        public string CameraName;
        public long MillisecondsSinceStart;
    }
}
