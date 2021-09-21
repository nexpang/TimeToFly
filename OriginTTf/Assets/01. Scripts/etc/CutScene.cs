using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public enum CutScenes
{
    Farm,
    Forest,
    Mountian,
    Cave
}

public class CutScene : MonoBehaviour
{
    [SerializeField]
    private CutScenes cutScenes;
    [SerializeField]
    private List<TimelineAsset> Timelines;
    [SerializeField]
    private List<GameObject> CutSceneObjs;

    [SerializeField]
    private List<GameObject> dieChickenObjs;

    [SerializeField]
    private Dack[] scene;

    [System.Serializable]
    struct Dack
    {
        public PlayerSprites[] dack;
    }

    void Start()
    {
        cutScenes = (CutScenes)(SecurityPlayerPrefs.GetInt("inGame.saveMapid", 0) / 3 - 1);
        SecurityPlayerPrefs.SetInt("inGame.tempLife", 9);

        Time.timeScale = 1;

        for(int i =0; i < CutSceneObjs.Count; i++)
        {
            if(CutSceneObjs[i].gameObject == null)
            {
                Debug.LogWarning("CutSceneObjs 의" + i + "번째 오브젝트가 null 입니다");
                continue;
            }
            CutSceneObjs[i].gameObject.SetActive(false);
        }
            
        gameObject.GetComponent<PlayableDirector>().playableAsset = Timelines[(int)cutScenes];
        CutSceneObjs[(int)cutScenes].SetActive(true);

        string[] livingChickenIndexs = SecurityPlayerPrefs.GetString("inGame.remainChicken", "0 1 2 3 4").Split(' ');
        List<string> livingChickenList = livingChickenIndexs.ToList();

        PlayerSprites[] playerSprites = scene[(int)cutScenes].dack;

        for (int i = 0; i < playerSprites.Length;i++)
        {
            if(playerSprites[i].gameObject == dieChickenObjs[(int)cutScenes])
            {
                playerSprites[i].targetSheet = SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1);
            }
            else
            {
                int randomIndex = Random.Range(0, livingChickenList.Count);
                playerSprites[i].targetSheet = int.Parse(livingChickenList[randomIndex]);
                livingChickenList.RemoveAt(randomIndex);
            }
        }

        gameObject.GetComponent<PlayableDirector>().Play();
    }
}
