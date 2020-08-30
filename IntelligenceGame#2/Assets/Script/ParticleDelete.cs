using UnityEngine;
using System.Collections;

public class ParticleDelete : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 0.4f);
    }
}
