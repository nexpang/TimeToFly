using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GameManager.Instance.player.isStun = true;
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.player.isStun = false;
    }
}
