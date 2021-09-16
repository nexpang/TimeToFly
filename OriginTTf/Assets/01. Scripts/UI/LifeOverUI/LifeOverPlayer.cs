using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeOverPlayer : MonoBehaviour
{
    private Animator ani = null;
    private bool aniStart = false;
    
    void Start()
    {
        ani = GetComponent<Animator>();
        aniStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayAwakeAni()
    {
        if (aniStart) return;
        aniStart = true;
        ani.Play("LifeOverPlayer_Awake&Run");
    }

    public void ChapterAgain()
    {
        GameManager.Instance.ChapterAgain();
    }
}
