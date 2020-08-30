using UnityEngine;
using System.Collections;

public class start : MonoBehaviour
{
    public int loop=1;
    // Use this for initialization
    void Start()
    {
        if(loop==1) Application.LoadLevel("Game");
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnGUI()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            if (GUI.Button(new Rect(20, 20, 200, 25), "Start Game"))
            {
                Application.LoadLevel("Game");
            }
        }
    }
}
