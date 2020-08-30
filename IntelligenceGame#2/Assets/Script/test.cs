using UnityEngine;
using System.Collections;

public class test : MonoBehaviour
{
    public GameObject particle;
    Rigidbody rb;
    float speed;
    float alpha;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        MeshRenderer mr = GetComponent<MeshRenderer>();

        mr.material.color = new Color(1.0f, 1.0f, 1.0f, mr.material.color.a);
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        speed = rb.velocity.magnitude;
        if (speed > 6) speed = 6;
        rb.velocity = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal).normalized * speed;

        if (collision.gameObject.tag == "Raket")
        {
            Instantiate(particle, transform.position, transform.rotation);
        }
    }
}
