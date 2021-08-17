using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public enum BossType
{
    JOKJEBI,
    DOKSURI,
    BAT
}

public class BossAppear : MonoBehaviour
{
    public BossType bossType = BossType.JOKJEBI;
    public CanvasGroup mobileControllerGroup = null;
    public Boss currentBoss;

    private bool isTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!isTrigger)
            {
                isTrigger = true;
                StartCoroutine(BossStart(bossType));
            }
        }
    }

    IEnumerator BossStart(BossType type)
    {
        GameManager.Instance.player.SetStun(8);
        DOTween.To(() => mobileControllerGroup.alpha, value => mobileControllerGroup.alpha = value, 0, 1f);
        mobileControllerGroup.interactable = false;
        yield return new WaitForSeconds(3);
        GameManager.Instance.Impulse(0.25f, 1f, 0.25f,2);
        yield return new WaitForSeconds(1.5f);

        currentBoss.gameObject.SetActive(true);

        if(type == BossType.JOKJEBI)
        {
            currentBoss.GetComponent<Animator>().Play("JokJeBi_Appear");
        }

        yield return new WaitForSeconds(3.5f);
        currentBoss.BossStart();
        DOTween.To(() => mobileControllerGroup.alpha, value => mobileControllerGroup.alpha = value, 1, 1f);
        mobileControllerGroup.interactable = true;
        DOTween.To(() => GameManager.Instance.bossBar.alpha, value => GameManager.Instance.bossBar.alpha = value, 1, 1f);
    }
}
