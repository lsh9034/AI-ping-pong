using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Think  {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

    }
    public double[][][] weight; //가중치 담는 배열
    double[][][] bfr_err; //이전 가중치 보정값 저장하는 배열(가속학습 할 때 필요함)
    double[] err_gradiant;  //오차 기울기 배열
    public double[] value; //신경망이 구한 답을 저장하는 배열
    int num_flr;    //층 갯수
    int[] num_neuron; //층 마다의 뉴런갯수
    double alpha;  //민감도 상수(가중치보정값)
    double betha;  //민감도 상수(전 학습당시 보정값)
    double[] output; //나와야할 결과
    public double err_sum; //오차 제곱합 구할 변수
    public void Create(int num_flr, int[] input) //층 갯수, 층마다의 뉴런갯수
    {
        this.num_flr = num_flr;
        num_neuron = new int[num_flr];
        for (int i = 0; i < num_flr; i++) num_neuron[i] = input[i];  //층마다의 뉴런갯수 입력
        value = new double[num_neuron[num_flr - 1]];
        weight = new double[num_flr - 1][][]; //첫 번째 대괄호는 몇번 째 층인지 두번째는 왼쪽뉴런 세번째는 해당왼쪽뉴런과 오른쪽뉴런을 이어주는 선
        for (int i = 0; i < num_flr - 1; i++) //-1전까지 도는 이유는 왼쪽에서 오른쪽 가중치로 하기 때문에 맨 오른쪽은 그 다음 오른쪽이 없으니까
        {
            weight[i] = new double[num_neuron[i] + 1][];  //왼쪽층의 뉴런의 수 + 임계값 저장(마지막뉴런한개는 임계값을 위한 뉴런인거임)
            for (int j = 0; j < num_neuron[i] + 1; j++)
            {
                weight[i][j] = new double[num_neuron[i + 1]];
            }
        }
        bfr_err = new double[num_flr - 1][][];
        for (int i = 0; i < num_flr - 1; i++)
        {
            bfr_err[i] = new double[num_neuron[i] + 1][];
            for (int j = 0; j < num_neuron[i] + 1; j++)
            {
                bfr_err[i][j] = new double[num_neuron[i + 1]];
                for (int k = 0; k < num_neuron[i + 1]; k++) bfr_err[i][j][k] = 0;
            }
        }
        for (int i = 0; i < num_neuron[num_flr - 1]; i++) value[i] = 0;
        alpha = 1.0;
        betha = 0.95;
        err_sum = 1;
    }

    double sigmoid(double x)
    {
        return (1.0 / (1.0 + Mathf.Exp(((float)-x))));
    }

    public void SetWeight()  //가중치 랜던값 초기화
    {
        //srand(time(NULL));
        for (int i = 0; i < num_flr - 1; i++)
            for (int j = 0; j <= num_neuron[i]; j++)  //등호가 있는 이유는 임계값까지 포함하기 때문
                for (int k = 0; k < num_neuron[i + 1]; k++)
                    weight[i][j][k] = (double)Random.Range(-1.0f, 1.0f);
    }

    public void learn(double[] input, double[] _output)
    {
        output = new double[num_neuron[num_flr - 1]];
        for (int i = 0; i < num_neuron[num_flr - 1]; i++) output[i] = _output[i];
        err_sum = 0;
        work(input, true, 0);

        output = null;
    }
    public void work(double[] input, bool learn, int flr) //안타깝게도 처음 input의 원소갯수는 고정이여야 한다...
    {
        //work_learn함수는 인풋이 들어왔을 때 인풋이 신경망을 통과하고 바뀐 값을 구한다 또한 leran이 true일 경우 학습도 한다.
        //재귀함수로써 input은 왼쪽 층의 뉴런의 값
        if (flr == num_flr - 1)  //출력층에 도착했을 경우
        {
            err_gradiant = new double[num_neuron[num_flr - 1]];
            double[] err = new double[num_neuron[num_flr - 1]];
            for (int i = 0; i < num_neuron[flr]; i++)
            {
                value[i] = input[i]; //출력값 저장
                if (learn)
                {
                    err[i] = output[i] - input[i]; //오차계산
                    err_sum += err[i] * err[i]; //오차 제곱합
                    err_gradiant[i] = err[i] * input[i] * (1.0 - input[i]); //출력층 오차기울기 계산
                }
            }
            err=null;
            return;
        }

        double[] right = new double[num_neuron[flr + 1]];
        for (int i = 0; i < num_neuron[flr + 1]; i++) //i는 오른쪽 j는 왼쪽
        {
            double sum = 0;
            for (int j = 0; j < num_neuron[flr]; j++)
            {
                sum += input[j] * weight[flr][j][i];
            }
            sum -= 1.0f * weight[flr][num_neuron[flr]][i];  //임계값 뉴런에 연결되있는 가중치선
            right[i] = sigmoid(sum);
        }
        work(right, learn, flr + 1); //다음 층으로 넘어갈 때는 오른쪽이 왼쪽이 됨으로 오른쪽을 넘겨주고 층 +1

        right=null; //오른쪽 층 즉 다음층에서 학습 할 때 필요한 건 오차기울기인데 이미 리턴되기전에 구해놨음으로 지워도됨.
        if (!learn) return; //학습이 목표가 아니면 그냥 종료
                            //뒤의 층 부터 가중치 보정 시작

        double[] now_err_gradiant = new double[num_neuron[flr]]; //현재 층의 오차기울기 담을 배열
        for (int i = 0; i < num_neuron[flr]; i++) //i는 현재 j는 다음층
        {
            now_err_gradiant[i] = 0;  //하... 이거 초기화 안해서 학습도 안됬었음... 잘 확인하자....
            for (int j = 0; j < num_neuron[flr + 1]; j++)
            {
                if (flr!=0) now_err_gradiant[i] += weight[flr][i][j] * err_gradiant[j];//현재층 뉴련의 오차기울기 계산 1번
                double deltha_w = input[i] * err_gradiant[j];  //가중치 보정값 계산
                weight[flr][i][j] += alpha * deltha_w + betha * bfr_err[flr][i][j]; //가중치 보정
                bfr_err[flr][i][j] = deltha_w;
            }
            if (flr != 0) now_err_gradiant[i] = now_err_gradiant[i] * input[i] * (1.0 - input[i]);//현재층 뉴런의 오차기울기 계산 2번(완성)
        }

        int threshold = num_neuron[flr]; //임계값 뉴런 번지
        for (int j = 0; j < num_neuron[flr + 1]; j++) //임계값 뉴런과 연결되있는 가중치들을 보정하는 for문임. j는 다음층
        {
            double threshold_w_err = alpha * (-1.0) * err_gradiant[j]; //왜 가중치값을 곱하지 않고 -1을 곱하는지는 잘 모르겠음.
            weight[flr][threshold][j] += threshold_w_err + betha * bfr_err[flr][threshold][j];  //임계값 뉴런과 연결되있는 가중치 보정
            bfr_err[flr][threshold][j] = threshold_w_err;
        }

        err_gradiant=null;
        err_gradiant = new double[num_neuron[flr]];//오차기울기 배열은 다음층 뉴런의 오차기울기를 담고있었기 때문에
        for (int i = 0; i < num_neuron[flr]; i++)  // 이전 뉴런의 가중치를 보정하려면 현재 뉴런 오차기울기를 담아야함.
            err_gradiant[i] = now_err_gradiant[i];
        now_err_gradiant = null;

        if (flr == 0)err_gradiant=null; //입력층까지 왔으면 삭제(메모리 누수 방지)
        return;
    }
}
