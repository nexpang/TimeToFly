using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTrap : MonoBehaviour
{
    private Animator ani = null;

    private bool isPlaying = false;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(GameManager.Instance.player.transform.position.x) - Mathf.Abs(transform.position.x) < 8)
        {
            if (isPlaying)
                return;
            isPlaying = true;
            ani.Play("Lightning_Play");
        }
        else
        {
            ani.Play("Lightning_Idle");
            isPlaying = false;
        }
    }
}
