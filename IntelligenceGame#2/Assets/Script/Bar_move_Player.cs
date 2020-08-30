using UnityEngine;
using System.Collections;

public class Bar_move_Player : MonoBehaviour
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

    public GameManager gm;
    public float speed = 1;

    Rigidbody rb;
    void Start()
    {
        objectSize = this.transform.localScale;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 tempVec3 = new Vector3(0.0f, 0.0f, 0.0f);

        if (Input.GetKey(KeyCode.A) && check_L == true) tempVec3 += left; //rb.MovePosition(rb.position + left * Time.fixedDeltaTime * gm.Game_Speed * speed);//여기 싹다 velocity로 고치세요                                                                                                                                     
        if (Input.GetKey(KeyCode.D) && check_R == true) tempVec3 += right; //rb.MovePosition(rb.position + right * Time.fixedDeltaTime * gm.Game_Speed * speed);//포지션값 때려박으면 저렇게 뚫고지나간다고 했습니까 안했습니까
        if (Input.GetKey(KeyCode.W) &&
            (check_T == true && check_M == true)) tempVec3 += up; //rb.MovePosition(rb.position + up * Time.fixedDeltaTime * gm.Game_Speed * speed);
        if (Input.GetKey(KeyCode.S) && check_B == true) tempVec3 += down; //rb.MovePosition(rb.position + down * Time.fixedDeltaTime * gm.Game_Speed * speed);
        
        rb.MovePosition(rb.position + tempVec3 * Time.fixedDeltaTime * gm.Game_Speed * speed);
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
