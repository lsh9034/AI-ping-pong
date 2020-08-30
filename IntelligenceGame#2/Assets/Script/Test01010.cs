using UnityEngine;
using System.Collections;

public class Test01010 : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform target2;

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.LookAt((target.position + target2.position)*0.4f);
    }
}