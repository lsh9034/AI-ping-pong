using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour
{
    private int score;

    [SerializeField]
    private TextMesh tm;

    [SerializeField]
    private string scorerName;

    void Start()
    {
        score = 0;
    }

    void OnCollisionEnter(Collision _collision)
    {
        if(_collision.gameObject.tag == "Ball")
        {
            ++score;

            tm.text = scorerName + "\n" + score;
        }
    }
}
