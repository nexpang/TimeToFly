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

    void Awake()
    {
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
        CutSceneObjs[(int)cutScenes].gameObject.SetActive(true);
        gameObject.GetComponent<PlayableDirector>().Play();
    }
}
