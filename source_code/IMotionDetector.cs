// Motion Detector
//
// Copyright © Andrew Kirillov, 2005
// andrew.kirillov@gmail.com
//

namespace TeboCam
{
    using System;
    using System.Drawing;

    /// <summary>
    /// IMotionDetector interface
    /// </summary>
    public interface IMotionDetector
    {
        /// <summary>
        /// Motion level calculation - calculate or not motion level
        /// </summary>
        bool MotionLevelCalculation { set; get; }

        /// <summary>
        /// Motion level - amount of changes in percents
        /// </summary>
        double MotionLevel { get; }

        /// <summary>
        /// Process new frame
        /// </summary>
        void ProcessFrame(ref Bitmap image);

        /// <summary>
        /// Reset detector to initial state
        /// </summary>
        void Reset();

        bool calibrating { set;get;}
        bool areaOffAtMotionTriggered { set;get;}
        bool areaOffAtMotionReset { set;get;}
        bool areaDetection { set;get;}
        bool areaDetectionWithin { set;get;}
        bool exposeArea { set;get;}
        int rectWidth { set;get;}
        int rectHeight { set;get;}
        int rectX { set;get;}
        int rectY { set;get;}
        int id { set;get;}

    }
}
