//using UnityEngine;
//using System.Collections;
//using System;


//public class GameManager : MonoBehaviour
//{
//    private static GameManager instance = null;

//    private AIManager aiManager_script = null;
//    private KinectManager kinectManager_script = null;
//    private TouchManager touchManager_script = null;
//    private GyroscopeManager gyroscopeManager_script = null;

//    public static GameManager Instance
//    {   
//        get
//        {
//            if (instance)
//                return instance;
//            else
//                return instance = new GameObject("GameManager").AddComponent<GameManager>();
//        }
//    }

//    public AIManager AIManager_script { get { return this.aiManager_script; } }
//    public KinectManager KinectManager_script { get { return this.kinectManager_script; } }
//    public TouchManager TouchManager_script { get { return this.touchManager_script; } }
//    public GyroscopeManager GyroscopeManager_script { get { return this.gyroscopeManager_script; } }

//    public GameManager()
//    {
//        this.aiManager_script = this.gameObject.AddComponent<AIManager>();
//        if ((InputConfig.typeFlag & InputConfig.TypeFlag.Kinect) != 0)
//            this.kinectManager_script = this.gameObject.AddComponent<KinectManager>();
//        if ((InputConfig.typeFlag & InputConfig.TypeFlag.Touch) != 0)
//            this.touchManager_script = this.gameObject.AddComponent<TouchManager>();
//        if ((InputConfig.typeFlag & InputConfig.TypeFlag.Gyroscope) != 0)
//            this.gyroscopeManager_script = this.gameObject.AddComponent<GyroscopeManager>();
//    }
//}