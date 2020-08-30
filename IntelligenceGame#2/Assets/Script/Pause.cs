using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private bool isPush; 

    void Start()
    {
        isPush = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // TODO: 모든 오브젝트 동작들 정지(BGM은 재생)
            isPush = !isPush;
        }
    }

    void OnGUI()
    {
        if(isPush)
        {   
            if(GUI.Button(new Rect(20, 20, 200, 50), "ReStart"))
            {
                SceneManager.LoadScene("Game");
            }
            if(GUI.Button(new Rect(20, 90, 200, 50), "Exit"))
            {
                Application.Quit();
            }
        }
    }
}
