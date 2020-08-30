using UnityEngine;
using System.Collections;

public class Master : MonoBehaviour {

    // Use this for initialization
    public Ball_move ct;

    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(ct.transform.position.x,transform.position.y,transform.position.z);
	}
}
