using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsSetDebug : MonoBehaviour
{
    public bool targetMapIdChange = false;
    public int inGame_tempLife = 9;
    public string inGame_remainChicken = "0 1 2 3 4";
    public int inGame_saveMapid = 0;
    public bool newbie = true;
    public int inGame_saveCurrentChickenIndex = -1;
    public bool inGame_ending = false;
    public int inGame_bakSukEndingCount = 0;
    public int inGame_otherEndingCount = 0;

    private void Awake()
    {
        if(targetMapIdChange)
        SceneController.targetMapId = SecurityPlayerPrefs.GetInt("inGame.saveMapid", 0);
    }

    [ContextMenu("디버그 / 플레이어 프랩스 표시")]
    public void ShowPlayerPrefs()
    {
        Debug.Log($"inGame.tempLife : {SecurityPlayerPrefs.GetInt("inGame.tempLife", 9)} \n" +
            $"inGame.remainChicken : {SecurityPlayerPrefs.GetString("inGame.remainChicken", "0 1 2 3 4")} \n" +
            $"inGame.saveMapid : {SecurityPlayerPrefs.GetInt("inGame.saveMapid", 0)} \n" +
            $"newbie : {SecurityPlayerPrefs.GetBool("newbie", true)} \n" +
            $"inGame.saveCurrentChickenIndex : {SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1)}");
    }

    [ContextMenu("디버그 / 플레이어 프랩스 설정")]
    public void SetPlayerPrefs()
    {
        SecurityPlayerPrefs.SetInt("inGame.tempLife", inGame_tempLife);
        SecurityPlayerPrefs.SetString("inGame.remainChicken", inGame_remainChicken);
        SecurityPlayerPrefs.SetInt("inGame.saveMapid", inGame_saveMapid);
        SecurityPlayerPrefs.SetBool("newbie", newbie);
        SecurityPlayerPrefs.SetInt("inGame.saveCurrentChickenIndex", inGame_saveCurrentChickenIndex);
        SecurityPlayerPrefs.SetBool("inGame.ending", inGame_ending);
        SecurityPlayerPrefs.SetInt("inGame.bakSukEndingCount", inGame_bakSukEndingCount);
        SecurityPlayerPrefs.SetInt("inGame.otherEndingCount", inGame_otherEndingCount);
        Debug.Log("완료");
    }
}
