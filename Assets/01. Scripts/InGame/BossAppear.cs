using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum BossType
{
    JOKJEBI,
    DOKSURI,
    BAT
}

public class BossAppear : MonoBehaviour
{
    public BossType bossType = BossType.JOKJEBI;

    private bool isTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!isTrigger)
            {
                isTrigger = true;
                StartCoroutine(BossStart());
            }
        }
    }

    IEnumerator BossStart()
    {
        GameManager.Instance.player.SetStun(5);
        yield return new WaitForSeconds(3);
        GameManager.Instance.Impulse(0.25f, 1f, 0.25f,2);
    }
}
