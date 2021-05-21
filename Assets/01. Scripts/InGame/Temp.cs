using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public static Temp Instance;


    [SerializeField] private int currentStage = 0;
    public int CurrentStage { get { return currentStage; } set { currentStage = value; } }

    [SerializeField] private int tempLife = 0;
    public int TempLife { get { return tempLife; } set { tempLife = value; } }

    [SerializeField] private List<Stage> stages = new List<Stage>();

    public List<Stage> GetStage() => stages;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

[Serializable]
public class Stage
{
    public int stageLife;
    public int stageTimer;
}