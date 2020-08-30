using UnityEngine;
using System.Collections;

public class TempAIMove : MonoBehaviour
{
    [SerializeField]
    private Transform ballPosiiton;

    void Update()
    {
        this.transform.position = new Vector3(ballPosiiton.position.x, this.transform.position.y, this.transform.position.z);
    }
}
