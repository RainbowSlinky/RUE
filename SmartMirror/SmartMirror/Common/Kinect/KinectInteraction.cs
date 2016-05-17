using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
namespace SmartMirror.Common.Kinect
{
    public class KinectInteraction
    {
        #region "Variables and constants"
        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Reader for body frames
        /// </summary>
        private BodyFrameReader bodyFrameReader = null;

        /// <summary>
        /// Array for the bodies
        /// </summary>
        private Body[] bodies = null;


        public float KinectFrameHeight, KinectFrameWidth;
        /// <summary>
        /// Constant for clamping Z values of camera space points from being negative
        /// </summary>
        private const float InferredZPositionClamp = 0.1f;
        public delegate void HandsDeteced(List<Hands> allHands);
        public event HandsDeteced newHandsFrameDetected;
        #endregion


        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public KinectInteraction()
        {
            // one sensor is currently supported
            this.kinectSensor = KinectSensor.GetDefault();
            
            // get the coordinate mapper
            this.coordinateMapper = this.kinectSensor.CoordinateMapper;

            // get the depth (display) extents
            FrameDescription frameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;
            KinectFrameHeight = frameDescription.Height;
            KinectFrameWidth = frameDescription.Width;


            // open the reader for the body frames
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // open the sensor
            this.kinectSensor.Open();
        }

        /// <summary>
        /// Execute start up tasks
        /// </summary>
        public void finializeInit()
        {
            if (this.bodyFrameReader != null)
            {
                this.bodyFrameReader.FrameArrived += this.Reader_FrameArrived;
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        public void closeKinect(object sender, CancelEventArgs e)
        {
            if (this.bodyFrameReader != null)
            {
                // BodyFrameReader is IDisposable
                this.bodyFrameReader.Dispose();
                this.bodyFrameReader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        /// <summary>
        /// Handles the body frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                List<Hands> allHands = new List<Hands>();
                foreach (Body body in this.bodies)
                {
                    Hands hands = null;
                    if (body.IsTracked)
                    {
                        IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
                        CameraSpacePoint positionLeft = joints[JointType.HandLeft].Position;
                        CameraSpacePoint positionRight = joints[JointType.HandRight].Position;
                        if (positionRight.Z < 0)
                        {
                            positionRight.Z = InferredZPositionClamp;
                        }
                        if (positionLeft.Z < 0)
                        {
                            positionLeft.Z = InferredZPositionClamp;
                        }
                        DepthSpacePoint depthSpacePointLeft = this.coordinateMapper.MapCameraPointToDepthSpace(positionLeft);
                        DepthSpacePoint depthSpacePointRight = this.coordinateMapper.MapCameraPointToDepthSpace(positionRight);
                        hands = new Hands(new Point(depthSpacePointLeft.X, depthSpacePointLeft.Y),
                            new Point(depthSpacePointRight.X, depthSpacePointRight.Y));
                    }

                    if (hands != null)
                    {
                        allHands.Add(hands);
                    }
                }
                if (allHands.Count > 0)
                { newHandsFrameDetected(allHands); }

            }
        }
    }

    public class Hands
    {
       public Point HandLeft, HandRight;
        public Hands(Point HandLeft, Point HandRight)
        {
            this.HandLeft = HandLeft;
            this.HandRight = HandRight;
        }
    }

}




