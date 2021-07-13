using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterInfo : MonoBehaviour
{
    public List<StageInfo> stageInfos = new List<StageInfo>();
}

[Serializable]
public class StageInfo
{
    public string stageName;
    public int stageLife;
    public int stageTimer;
    public GameObject stage;
    public GameObject background;
}