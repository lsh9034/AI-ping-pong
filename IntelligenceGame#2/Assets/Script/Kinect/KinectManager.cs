using UnityEngine;
using System.Collections.Generic;
using Windows.Kinect;

public class KinectManager : MonoBehaviour
{
    public struct BodyData
    {
        public int index;
        public Body body;

        public BodyData(int index, Body body)
        {
            this.index = index;
            this.body = body;
        }
    }
    public struct CursorData
    {
        public int index;
        public Vector3 position;

        public CursorData(int index, Vector3 position)
        {
            this.index = index;
            this.position = position;
        }
    }

    private static KinectSensor kinectSensor = null;
    private BodyFrameReader bodyFrameReader = null;
    private Body[] bodys = null;

    private Dictionary<int, bool> isPress = null;

    public event EventHandler<BodyData> OnBodyRecived = null;

    public event EventHandler<CursorData> OnCursorPressed = null;
    public event EventHandler<CursorData> OnCursorReleased = null;
    public event EventHandler<CursorData> OnCursorMoved = null;

    private void Start()
    {
        if (kinectSensor == null)
            kinectSensor = KinectSensor.GetDefault();

        this.bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();

        if (!kinectSensor.IsOpen)
            kinectSensor.Open();
        
        this.isPress = new Dictionary<int, bool>();
    }

    private void Update()
    {
        BodyFrame bodyFrame = this.bodyFrameReader.AcquireLatestFrame();
        if (bodyFrame != null)
        {
            if (this.bodys == null)
                this.bodys = new Body[kinectSensor.BodyFrameSource.BodyCount];

            bodyFrame.GetAndRefreshBodyData(this.bodys);

            bodyFrame.Dispose();
            bodyFrame = null;
            
            for (int i = 0; i < this.bodys.Length; ++i)
            {
                for (int j = i + 1; j < this.bodys.Length; ++j)
                {
                    if (this.bodys[i].IsTracked && this.bodys[j].IsTracked)
                    {
                        if (this.bodys[i].Joints[JointType.Head].Position.X > this.bodys[j].Joints[JointType.Head].Position.X)
                        {
                            Body temp = this.bodys[i];
                            this.bodys[i] = this.bodys[j];
                            this.bodys[j] = temp;
                        }
                    }
                    else if (!this.bodys[i].IsTracked && this.bodys[j].IsTracked)
                    {
                        Body temp = this.bodys[i];
                        this.bodys[i] = this.bodys[j];
                        this.bodys[j] = temp;
                    }
                }
            }

            for (int i = 0; i < this.bodys.Length; ++i)
            {
                if (this.bodys[i].IsTracked)
                {
                    if (this.OnBodyRecived != null)
                        this.OnBodyRecived(new BodyData(i, this.bodys[i]));

                    //Vector3 headPosition = VectorUtil.GetVector3FromJoint(this.bodys[i].Joints[JointType.Head]);
                    //Vector3 handPosition = VectorUtil.GetVector3FromJoint(this.bodys[i].Joints[JointType.HandRight]);

                    //bool press = headPosition.z - 0.5F > handPosition.z, lastPress = press;
                    //if (this.isPress.ContainsKey(i))
                    //    lastPress = this.isPress[i];
                    //else
                    //    this.isPress.Add(i, lastPress);

                    //if (press && !lastPress && this.OnCursorPressed != null)
                    //    this.OnCursorPressed(new CursorData(i, handPosition - headPosition));
                    //else if (!press && lastPress && this.OnCursorReleased != null)
                    //    this.OnCursorReleased(new CursorData(i, handPosition - headPosition));
                    //if (this.OnCursorMoved != null)
                    //    this.OnCursorMoved(new CursorData(i, handPosition - headPosition));

                    //this.isPress[i] = press;
                }
            }
        }
    }
}