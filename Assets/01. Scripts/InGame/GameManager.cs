using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int defaultTimer;
    private int _timer;
    public int timer
    {
        get { return _timer; }
        set { _timer = value; }
    }

    private void Awake()
    {
        Instance = this; // ½Ì±ÛÅæ
        timer = defaultTimer; // Å¸ÀÌ¸Ó ÃÊ±âÈ­
    }

    private float targetTime = 1;
    private float currentTime = 0;
    private void Update()
    {
        currentTime = Time.time;
        if(currentTime > targetTime)
        {
            Timer();
        }
    }

    private void Timer()
    {
        timer--;
        targetTime = currentTime + 1;
    }
}
