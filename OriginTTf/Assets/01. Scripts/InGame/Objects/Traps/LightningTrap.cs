using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTrap : MonoBehaviour
{
    public float waitTime;

    private Animator ani = null;

    private bool isPlaying = false;
    private WaitForSeconds ws;
    private IEnumerator myIenumerator;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        ws = new WaitForSeconds(waitTime);
        myIenumerator = Lightning();
    }
    private void Start()
    {
        //StartCoroutine(Lighting());
    }
    private void Update()
    {
        if (Mathf.Abs(GameManager.Instance.player.transform.position.x - transform.position.x) < 8)
        {
            if (isPlaying)
                return;
            isPlaying = true;
            StartCoroutine(myIenumerator);
        }
        else
        {
            StopCoroutine(myIenumerator);
            ani.Play("Lightning_Idle");
            isPlaying = false;
        }
    }

    private IEnumerator Lightning()
    {
        while(true)
        {
            yield return ws;
            ani.Play("Lightning_Play");
        }
    }

    private void Event_Thundering()
    {
        ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_Thunder, 1f, true);
    }
}
