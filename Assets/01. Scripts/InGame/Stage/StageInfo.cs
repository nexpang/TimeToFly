using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[Serializable]
public class StageInfo
{
    public string stageName;
    public int stageId;
    public int stageLife;
    public int stageTimer;
    public GameObject stage;
    public GameObject background;
    public CinemachineVirtualCamera virtualCamera;
    public Vector3 cameraStartPos;
    public Vector3 virtualCameraStartPos;
    public BoxCollider2D teleportAbleRange;
}