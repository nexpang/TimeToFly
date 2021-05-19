using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int defaultTimer;
    [SerializeField] bool isNeedTimer = true;
    private int _timer;
    public int timer
    {
        get { return _timer; }
        set { _timer = value; }
    }

    private void Awake()
    {
        Time.timeScale = 1;
        Instance = this; // �̱���
        timer = defaultTimer; // Ÿ�̸� �ʱ�ȭ
    }

    private float targetTime = 1;
    private float currentTime = 0;
    private void Update()
    {
        if (isNeedTimer)
        {
            currentTime = Time.time;
            if (currentTime > targetTime)
            {
                Timer();
            }
        }
    }

    private void Timer()
    {
        timer--;
        targetTime = currentTime + 1;
    }
}
