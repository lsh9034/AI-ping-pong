using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;

public class KinectBody : MonoBehaviour
{
    [SerializeField]
    private Rigidbody MoveObject_body;
    [SerializeField]
    private Rigidbody RotateObject_rigid;
    [SerializeField]
    private Transform HandTransform;

    [SerializeField]
    private float moveScaleX;
    [SerializeField]
    private float moveScaleY;
    [SerializeField]
    private float RotateScale;
    [SerializeField]
    private float ZSupporter;

    private Vector3 DefaultPosition;
    private Vector3 DefaultSpinePosition=Vector3.zero;
    

    private Vector3 HandPos;
    private Vector3 ElbowPos;

    private Vector3 SpinePos;
    void Awake()
    {
        if(GameManager.Instance.kinectManager_script)
            GameManager.Instance.kinectManager_script.OnBodyRecived += this.OnBodyRecived;
        DefaultPosition = MoveObject_body.transform.position;
    }

    void Update()
    {
        //HandTransform.position = HandPos;
       
        float tempX = (DefaultSpinePosition.x - SpinePos.x + DefaultPosition.x) * -1 * moveScaleX;
        //float TempY = (DefaultSpinePosition.z - SpinePos.z + DefaultPosition.y) * moveScaleY;
        float TempY = (DefaultSpinePosition.z - SpinePos.z) * moveScaleY + DefaultPosition.y;
        MoveObject_body.MovePosition(new Vector3(tempX, TempY, DefaultPosition.z));
        RotateObject_rigid.MovePosition(new Vector3(tempX + 3, TempY, DefaultPosition.z));
        //Debug.Log("Angle 1 : " + Quaternion.LookRotation(ElbowPos, HandPos).eulerAngles.z   );  
        float tempZ = (((Quaternion.LookRotation(ElbowPos, HandPos).eulerAngles.z) + ZSupporter) % 360) * RotateScale;
       // Debug.Log("TempZ :" + tempZ);
        
        RotateObject_rigid.MoveRotation(Quaternion.AngleAxis(tempZ,Vector3.forward));

        //RotateObject_rigid.MoveRotation(Quaternion.LookRotation(ElbowPos,HandPos));
        //Debug.Log("Angle" + Quaternion.LookRotation(ElbowPos, HandPos).eulerAngles);
    }


    private void OnBodyRecived(KinectManager.BodyData bodyData)
    {
        if (bodyData.index == 0)
        {
            Dictionary<JointType, Windows.Kinect.Joint> joints = bodyData.body.Joints;
            if ( joints[JointType.HandRight].TrackingState == TrackingState.Tracked)
            {
                Vector3 rightHandPosition = VectorUtil.GetVector3FromJoint(joints[JointType.HandRight]);
                if (joints[JointType.Head].TrackingState == TrackingState.Tracked && joints[JointType.SpineMid].TrackingState == TrackingState.Tracked)
                {
                    Vector3 bodyDirection = VectorUtil.GetVector3FromJoint(joints[JointType.Head]) - VectorUtil.GetVector3FromJoint(joints[JointType.SpineMid]);
                }
                if (joints[JointType.SpineShoulder].TrackingState == TrackingState.Tracked)
                {
                    Vector3 RightElbow = VectorUtil.GetVector3FromJoint(joints[JointType.ElbowRight]);
                    ElbowPos = RightElbow;
                    HandPos = rightHandPosition;
                }
                if (joints[JointType.SpineMid].TrackingState == TrackingState.Tracked)
                {
                    if (DefaultSpinePosition == Vector3.zero)
                        DefaultSpinePosition = VectorUtil.GetVector3FromJoint(joints[JointType.SpineMid]);
                    SpinePos = VectorUtil.GetVector3FromJoint(joints[JointType.SpineMid]);
                }

            }
        }
    }
}