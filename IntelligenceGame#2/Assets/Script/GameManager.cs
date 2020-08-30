using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public int loop;
    public float answer_ratio = 2.44f;
    //public float Game_Speed = 1;
    public float Game_Speed
    {
        set
        {
            Time.timeScale = value;
            Time.fixedDeltaTime = value * 0.002f;
        }
        get
        {
            return Time.timeScale;
        }
    }
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (instance)
                return instance;
            else
                return instance = new GameObject("GameManager").AddComponent<GameManager>();
        }
    }
    private KinectManager m_KinectManager;
    public KinectManager kinectManager_script { get { return m_KinectManager; }    }

    void Awake()
    {
        this.m_KinectManager = this.gameObject.AddComponent<KinectManager>();
    }
}
