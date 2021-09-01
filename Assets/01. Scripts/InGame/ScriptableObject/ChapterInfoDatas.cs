using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ChapterInfoDataScriptableObject", order = 1)]
public class ChapterInfoDatas : ScriptableObject
{
    public GlobalDataInfo[] infos;
}

[System.Serializable]
public class GlobalDataInfo
{
    public string mapName;
    public Sprite mapSprite;
}