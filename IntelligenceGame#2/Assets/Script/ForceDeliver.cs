using UnityEngine;
using System.Collections;

public class ForceDeliver : MonoBehaviour {

    // Use this for initialization
    public RaketRotate Raket;
    public Rigidbody rb;
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        
    }
}
