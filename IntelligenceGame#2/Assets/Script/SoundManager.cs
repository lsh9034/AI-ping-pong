using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    private AudioSource myAudio;

    [SerializeField]
    private AudioClip boundSound;

    [SerializeField]
    public static SoundManager instance;


    void Awake()
    {
        if(!SoundManager.instance)
            SoundManager.instance = this;
    }

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundName)
    {
        if(soundName.CompareTo("WallBound") == 0)
        {
            myAudio.PlayOneShot(boundSound);
        }
    }
}
