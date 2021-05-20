using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    [SerializeField] private int currentStage = 0;
    public int CurrentStage { get { return currentStage; } set { currentStage = value; } }

    [SerializeField] private int tempLife = 0;
    public int TempLife { get { return tempLife; } set { tempLife = value; } }

    [SerializeField] private List<Stage> stages = new List<Stage>();

    public List<Stage> GetStage() => stages;
}

[Serializable]
public class Stage
{
    public int stageLife;
    public int stageTimer;
}