using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    protected float curTime = 0; //* 현재 시간
    public float maxTime = 45;
    float time = 0;
    public Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if(time >= 1&&maxTime > curTime)
        {
            curTime++;
            slider.value = curTime;
            time = 0;
        }
        else if(maxTime < curTime)
        {
            Player.Instance.Dead();
        }
    }
}
