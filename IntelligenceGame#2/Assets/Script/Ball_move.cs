using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Ball_move : MonoBehaviour
{

    // Use this for initialization
    Vector3 ShootPos;
    Vector3 move;
    Vector2 CrashPos;
    public float Speed = 2;
    public GameManager gm;
    public AI ai;
    List<double[]> learn_data;
    List<double> output;
    int learn_num = 0;
    public bool check = false;  //마지막의 닿았던 바가 아래바인지 위바인지 알기위한 변수
    double bar_width_y;
    double answer = -10;
    bool gameover = false;
    bool touch = false; //공이 충돌후 충돌범위에서 벗어낫는지
    public bool learn_check = false;
    Rigidbody rb;
    public float Pos_w_l, Pos_w_r;
    public float Pos_w_t, Pos_w_b;
    public GameObject ra;
    Vector3 ra_object_size;
    public int learn_turm; //몇번 졌을때 학습할 것이냐
    int turm = 0;
    public float PosY; //현재 AI Y좌표
    void Start()
    {
        ShootPos = this.gameObject.transform.position;
        rb = GetComponent<Rigidbody>();
        {//블록해놓은곳 주석하면 중간에서 자동으로 안날라가고 내가 쳐야 됨
            float x = (float)Random.Range(-0.8f, 0.8f);
            float y = (float)Random.Range(-0.05f, -1.0f);
            rb.velocity = new Vector3(x, y, 0);
            rb.velocity = rb.velocity.normalized * Speed;
        }
        learn_data = new List<double[]>();
        output = new List<double>();
        ra_object_size = ra.transform.localScale;
        bar_width_y = ra_object_size.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        float y = this.transform.position.y;
        float x = this.transform.position.x;
        if (y >= PosY - 0.15 && answer == -10)
        {
            answer = this.transform.position.x;
            if (answer < 0.38) answer = 0.38;
            else if (answer > 5.62) answer = 5.62;
            //check = false;
            Debug.Log(answer.ToString());
            Debug.Log((ai.Basic_NeuronNetwork.value[0] * 6.0f).ToString());
        }
        if ((y >= Pos_w_t + 1) && learn_check == true && check == true) //게임 오버
        {
            turm++;
            for (int i = 0; i < learn_num; i++) output.Add(answer / gm.answer_ratio);
            // ai.Learn(learn_data, output, learn_num);
            //Debug.Log(learn_num.ToString());
            //for (int i = 0; i < learn_num; i++) Debug.Log(learn_data[i][1].ToString());
            ai.Merge(learn_data, learn_num, answer / gm.answer_ratio);
            if (turm == learn_turm)
            {
                ai.Learn(ai.learn_data, ai.output_data, ai.learn_num);
                ai.WriteData();
                turm = 0;
            }
            if (gm.loop == 1)
            {
                Restart_Setting();
                //ai.print_LearnData();
            }
        }
        if (y <= Pos_w_b - 1 || y >= Pos_w_t + 1)
        {
            if (gm.loop == 1)
            {
                Restart_Setting();
            }
        }
        if (x <= Pos_w_l - 1 || x >= Pos_w_r + 1)
        {
            if (gm.loop == 1) Restart_Setting();
        }
    }
    void Restart_Setting()
    {
        transform.position = new Vector3(3.75f, -3.35f, 3);
        ShootPos = this.gameObject.transform.position;
        {//블록해놓은곳 주석하면 중간에서 자동으로 안날라가고 내가 쳐야 됨
            float x1 = (float)Random.Range(-0.8f, 0.8f);
            float y1 = (float)Random.Range(-0.05f, -1.0f);
            rb.velocity = new Vector3(x1, y1, 0);
            rb.velocity = rb.velocity.normalized * Speed;
        }
        learn_data.Clear();
        output.Clear();
        learn_num = 0;
        answer = -10;
        check = false;
        touch = false;
    }
    public void OnCollisionEnter(Collision collision)
    {
        //this.transform.position = PastPosition;
        if (touch == false)
        {
            touch = true;
            // Debug.Log(touch.ToString());
            // CrashPos = collision.contacts[0].point;

            // 입사벡터를 알아본다. (충돌할때 충돌한 물체의 입사 벡터 노말값)
            //Vector3 incomingVector = move;
            //incomingVector = incomingVector.normalized;
            // 충돌한 면의 법선 벡터를 구해낸다.
            // Vector3 normalVector = collision.contacts[0].normal;
            // 법선 벡터와 입사벡터을 이용하여 반사벡터를 알아낸다.
            //Vector3 reflectVector = Vector3.Reflect(incomingVector, normalVector); //반사각
            //reflectVector = reflectVector.normalized;
            //move = reflectVector;
            //Debug.Log(move.ToString());
            //Vector3 force = new Vector3(move.x * collision.relativeVelocity.x, move.y * collision.relativeVelocity.y,move.z*collision.relativeVelocity.z);
            //rb.AddForce();
            move = rb.velocity.normalized;
            CrashPos = collision.contacts[0].point;

            double[] input = new double[4];
            input[0] = CrashPos.x;
            input[1] = CrashPos.y;
            input[2] = move.x;
            input[3] = move.y;
            ai.Basic_NeuronNetwork.work(input, false, 0);

            if (collision.transform.tag == "Under_bar")
            {
                check = true;
                learn_data.Clear();
                learn_data = new List<double[]>();
                learn_num = 0;
                answer = -10;
            }
            else if (collision.transform.tag == "Top_bar")
            {
                check = false;
                //Debug.Log(this.transform.position.y.ToString());
            }
            if (check == true && answer == -10)
            {
                double[] learn_data1 = new double[4];
                learn_data1[0] = CrashPos.x;
                learn_data1[1] = CrashPos.y;
                learn_data1[2] = move.x;
                learn_data1[3] = move.y;
                learn_data.Add(learn_data1);
                // for (int i = 0; i < 4; i++) Debug.Log(learn_data[0][i].ToString());
                learn_num++;
            }
        }
    }
    public void OnCollisionExit(Collision col)
    {
        touch = false;
        // Debug.Log(touch.ToString());
    }
}
