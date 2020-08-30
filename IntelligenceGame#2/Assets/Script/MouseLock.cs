using UnityEngine;
using System.Collections;

public class MouseLock : MonoBehaviour {

    private bool pressESC = false;
    private bool flag = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) { pressESC = true; }
        if (Input.GetKeyUp(KeyCode.Escape) && pressESC) { flag = true; }
        if (Input.GetKeyDown(KeyCode.Escape) && flag) { pressESC = false; flag = false; }
        if (pressESC)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            //player.setAIMOn(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
           // player.setAIMOn(true);
        }
    }
}
