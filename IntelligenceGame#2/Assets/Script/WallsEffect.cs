using UnityEngine;
using System.Collections;

public class WallsEffect : MonoBehaviour
{
    public bool isActive;
    private float alpha, originAlpha;
    private Color color, originColor;
    private MeshRenderer mr;

    [SerializeField]
    private float startValue;

    [SerializeField]
    private float EndValue;

    private float reff;

    [SerializeField]
    private float Timer;

    [SerializeField]
    private bool Player;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();

        originColor = color = mr.material.color;
        originAlpha = originColor.a;
        alpha = originAlpha;
        isActive = false;
        if(Player)
            mr.material.SetColor("_WireColor", new Color(originColor.r, originColor.g, originColor.b, 1f * 0.5f));
    }

    void Update()
    {
        if (!isActive)
            return;

        alpha = Mathf.SmoothDamp(alpha, EndValue, ref reff, Timer);

        mr.sharedMaterial.SetColor("_Color", new Color(originColor.r, originColor.g, originColor.b, alpha));
    }

    void OnCollisionEnter(Collision other)
    {
        isActive = true;
        alpha = startValue;

        SoundManager.instance.PlaySound("WallBound");
    }
}