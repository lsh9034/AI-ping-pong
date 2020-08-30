using UnityEngine;
using System.Collections;

public class RaketRotate : MonoBehaviour
{

    // Use this for initialization
    Vector3 MousePos;
    Vector3 PastMousePos;
    Vector3 MyPos;
    Rigidbody rb;
    float a = 0;
    float Pastdx;
    [SerializeField]
    private float speed;


    Transform my;
    Vector3 defaultPos;
    int i;
    void Awake()
    {
        i = 0;
        my = this.transform.GetChild(0).transform;
        defaultPos = my.localPosition;
    }
    void Start()
    {
        PastMousePos = Input.mousePosition;
        rb = GetComponent<Rigidbody>();

    }
    void Update()
    {
        this.setDefault();

    }
    private void setDefault()
    {

        i++;
        if (i == 15)
        {
            my.localPosition = defaultPos; 

            i = 0;
        }
    }
    // Update is called once per frame 참고로 내가 시현이원격입력 막았음
    void FixedUpdate()
    {
        //마우스를 오브젝트가 바라보게 하는 코드    
        //MousePos = Input.mousePosition;
        //MyPos = this.transform.position;
        //MousePos.z = MyPos.z - Camera.main.transform.position.z;

        //Vector3 target = Camera.main.ScreenToWorldPoint(MousePos);

        //float dy = target.y - MyPos.y;
        //float dx = target.x - MyPos.x;

        //float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        //this.transform.rotation = Quaternion.Euler(0f, 0f, rotateDegree);

        float dx = -Input.GetAxis("Mouse X");
        if (dx != 0)
        {
            //this.transform.Rotate(0f, 0f, dx / Mathf.PI);
            float temp = (Pastdx + dx) * Time.fixedDeltaTime * speed;
            temp %= 360;
            rb.MoveRotation(Quaternion.Euler(0f, 0f, temp));
            PastMousePos = MousePos;
            Pastdx += dx;
        }

        //a += speed;
        //Debug.Log(rb.velocity.ToString());
        //rb.MoveRotation(Quaternion.Euler(new Vector3(0f, 0f, a)));
    }
}
