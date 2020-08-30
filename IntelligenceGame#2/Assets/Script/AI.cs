using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
public class AI : MonoBehaviour
{

    // Use this for initialization

    public Think Basic_NeuronNetwork = null;
    int num_flr = 6; //층 갯수
    int[] flr_num_neuron; //층마다의 뉴런갯수 담는 배열
    int num_neuron = 30;  //층마다의 뉴런갯수 지정 입,출력층 제외
    public int input_num = 4;  //입력층 뉴런갯수
    public int output_num = 1;  //출력층 뉴련갯수
    public List<double[]> learn_data;
    public List<double> output_data;
    public int learn_num = 0;
    Vector3 left = new Vector3(-1.0f, 0, 0);
    Vector3 right = new Vector3(1.0f, 0, 0);
    public Vector3 objectSize;
    public GameManager gm;
    string Path = "Assets/Resources/";

    bool check_L = true;
    bool check_R = true;

    void Start()
    {
        flr_num_neuron = new int[num_flr];
        flr_num_neuron[0] = input_num;
        flr_num_neuron[num_flr - 1] = output_num;
        for (int i = 1; i < num_flr - 1; i++) flr_num_neuron[i] = num_neuron;

        Basic_NeuronNetwork = new Think();
        Basic_NeuronNetwork.Create(num_flr, flr_num_neuron);
        TextAsset txt = Resources.Load("Neuron_weight", typeof(TextAsset)) as TextAsset;

        StringReader reader = new StringReader(txt.text);
        for (int i = 0; i < num_flr - 1; i++)
        {
            for (int j = 0; j < flr_num_neuron[i] + 1; j++)
            {
                string line = reader.ReadLine();
                if (line == null) break;

                string[] arr = line.Split(' ');
                for (int k = 0; k < flr_num_neuron[i + 1]; k++)
                {
                    Basic_NeuronNetwork.weight[i][j][k] = double.Parse(arr[k]);
                    //Debug.Log(Basic_NeuronNetwork.weight[i][j][k].ToString());
                }
            }
        }
        learn_data = new List<double[]>();
        output_data = new List<double>();
        TextAsset txt2 = Resources.Load("LearnData", typeof(TextAsset)) as TextAsset;
        StringReader reader2 = new StringReader(txt2.text);
        string num = reader2.ReadLine();
        int n = int.Parse(num);
        //Debug.Log(n.ToString());
        for (int i = 0; i < n; i++)
        {
            double[] learn_data1 = new double[input_num];
            string line = reader2.ReadLine();
            string[] arr = line.Split(' ');
            for (int j = 0; j < input_num; j++)
            {
                learn_data1[j] = double.Parse(arr[j]);
            }
            learn_data.Add(learn_data1);
            output_data.Add(double.Parse(arr[input_num]));
            learn_num++;
        }
        objectSize = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Merge(List<double[]> Learn_data, int Learn_num, double output)
    {
        for (int i = learn_num; i < learn_num + Learn_num; i++)
        {
            double[] learn_data1 = new double[input_num];
            for (int j = 0; j < input_num; j++)
            {
                learn_data1[j] = Learn_data[i - learn_num][j];
            }
            // for (int j = 0; j < input_num; j++) Debug.Log(learn_data1[j]);
            learn_data.Add(learn_data1);
            output_data.Add(output);
        }
        learn_num += Learn_num;
    }
    public void Learn(List<double[]> Learn_data, List<double> output, int Learn_num)
    {
        bool err_sum = false;
        //int count = 0;
        while (!err_sum)
        {
            err_sum = true;
            for (int i = 0; i < Learn_num; i++)
            {
                double[] o = new double[1];
                o[0] = output[i];
                Basic_NeuronNetwork.learn(Learn_data[i], o);
                if (Basic_NeuronNetwork.err_sum > 0.00001) err_sum = false;
            }
            //count++;
            //if (count == 1000)
            //{
            //   // System.GC.Collect();
            //    count = 0;
            //}
        }
    }
    public void WriteData()
    {
        StreamWriter writer = new StreamWriter(Path + "Neuron_weight.txt");
        for (int i = 0; i < num_flr - 1; i++)
        {
            for (int j = 0; j < flr_num_neuron[i] + 1; j++)
            {
                string arr = "";
                for (int k = 0; k < flr_num_neuron[i + 1]; k++)
                {
                    arr += Basic_NeuronNetwork.weight[i][j][k].ToString();
                    arr += ' ';
                }
                writer.WriteLine(arr);
            }
        }
        writer.Close();
        //FileStream f2 = new FileStream(Path + "LearnData.txt", FileMode.Append, FileAccess.Write);
        writer = new StreamWriter(Path + "LearnData.txt");
        writer.WriteLine(learn_num.ToString());
        for (int i = 0; i < learn_num; i++)
        {
            string arr = "";
            for (int j = 0; j < input_num; j++)
            {
                arr += learn_data[i][j].ToString();
                arr += ' ';
            }
            arr += output_data[i];
            writer.WriteLine(arr);
        }

        writer.Close();
    }
    public void print_LearnData()
    {
        Debug.Log(learn_num.ToString());
        for (int i = 0; i < learn_num; i++)
        {
            for (int j = 0; j < input_num; j++)
            {
                Debug.Log(learn_data[i][j].ToString());
            }
            Debug.Log(output_data[i].ToString());
        }
    }
    //void OnTriggerEnter(Collider collision)  //이 스크립트가 들어가는 오브젝트에 리지드 바디가 없어도되긴하지만 그럴경우 부딪히는 물체에는 리지드바디가 있어야 불림
    //{                                           //collision엔터는 리지드바디가 있어도 is Kinematic이 체크되면 함수가 안불림 개꾸짐
    //    if (collision.tag == "War_L") check_L = false;
    //    if (collision.tag == "War_R") check_R = false;
    //    //Debug.Log(check_L.ToString());
    //}
    //void OnTriggerExit(Collider collision)
    //{
    //    if (collision.tag == "War_L" || collision.tag == "War_R") check_L = check_R = true;
    //   // Debug.Log(check_L.ToString());
    //}
}
