using UnityEngine;
using System.Collections;
using System;

public class autoRotate : MonoBehaviour
{
    public float Speed;

    
    void Update()
    {
        this.transform.Rotate(new Vector3(Speed, 0f,0f ));
    }
}