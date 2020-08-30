using UnityEngine;
using System.Collections;

public class Bar_move_AI : MonoBehaviour
{
    Vector3 left = new Vector3(-1.0f, 0, 0);
    Vector3 right = new Vector3(1.0f, 0, 0);
    Vector3 up = new Vector3(0, 1.0f, 0);
    Vector3 down = new Vector3(0, -1.0f, 0);

    bool check_L = true;
    bool check_R = true;
    bool check_T = true;
    bool check_B = true;
    bool check_M = true;

    float top = 1.54f;
    float bottom = -1.54f;

    Vector3 objectSize;

    public AI ai;
    public GameManager gm;
    public float speed = 1;
    public Ball_move ball;
    Rigidbody rb;
    void Start()
    {
        objectSize = this.transform.localScale;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        double x = rb.position.x;
        double fin = ai.Basic_NeuronNetwork.value[0] * gm.answer_ratio;
        double destination = 3;
        if (fin >= 3) destination = fin - 0.8;
        if (fin < 3) destination = fin + 0.8;
        float z = rb.rotation.eulerAngles.z;
        if (ball.check == false)
        {
            if (x < destination) rb.MovePosition(rb.position + right * gm.Game_Speed * Time.fixedDeltaTime * speed);
            else if (x > destination) rb.MovePosition(rb.position + left * gm.Game_Speed * Time.fixedDeltaTime * speed);
        }
        else if (x < destination && check_R == true)
        {
            rb.MovePosition(rb.position + right * gm.Game_Speed * Time.fixedDeltaTime * speed);
        }
        else if (x > destination && check_L == true) rb.MovePosition(rb.position + left * gm.Game_Speed * Time.fixedDeltaTime * speed);

        if (fin >= 3)
        {
            if (z <= 180 && z > 2) rb.MoveRotation(Quaternion.Euler(new Vector3(0, 0, z - 1.0f)));
            else if (180 < z && z < 357) rb.MoveRotation(Quaternion.Euler(new Vector3(0, 0, z + 1.0f)));
        }
        if (fin < 3)
        {
            if (z <= 178 && z >= 0) rb.MoveRotation(Quaternion.Euler(new Vector3(0, 0, z + 1.0f)));
            else if (z > 183) rb.MoveRotation(Quaternion.Euler(new Vector3(0, 0, z - 1.0f)));
        }
    }
    void OnTriggerEnter(Collider collision)  //이 스크립트가 들어가는 오브젝트에 리지드 바디가 없어도되긴하지만 그럴경우 부딪히는 물체에는 리지드바디가 있어야 불림
    {                                           //collision엔터는 리지드바디가 있어도 is Kinematic이 체크되면 함수가 안불림 개꾸짐
        if (collision.tag == "War_L") check_L = false;
        if (collision.tag == "War_R") check_R = false;
        if (collision.tag == "War_T") check_T = false;
        if (collision.tag == "War_B") check_B = false;
        if (collision.tag == "MiddleLine") check_M = false;
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "War_L" ||
            collision.tag == "War_R" ||
            collision.tag == "War_T" ||
            collision.tag == "War_B" ||
            collision.tag == "MiddleLine")
        {
            check_L = check_R = check_T = check_B = check_M = true;
        }
    }
}
