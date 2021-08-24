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

    private void Start()
    {
        GameManager.Instance.isBossStage = true;
    }

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
        if (bossType == BossType.JOKJEBI)
        {
            GameManager.Instance.player.SetStun(10);
        }
        else if (bossType == BossType.DOKSURI)
        {
            GameManager.Instance.player.SetStun(8);
        }
        else if (bossType == BossType.BAT)
        {
            GameManager.Instance.FadeInOut(2.5f, 2f, 1, () =>
             {
                 Debug.Log("¤¾¤·");
             });
            yield break;
        }


        DOTween.To(() => mobileControllerGroup.alpha, value => mobileControllerGroup.alpha = value, 0, 1f);
        mobileControllerGroup.interactable = false;

        DOTween.To(() => GameManager.Instance.curStageInfo.virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
            value => GameManager.Instance.curStageInfo.virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y = value,
            2.5f, 0.5f);

        yield return new WaitForSeconds(3);
        GameManager.Instance.curStageInfo.virtualCamera.Follow = null;
        GameManager.Instance.CameraImpulse(0.25f, 1f, 0.25f,2);
        currentBoss.Event_CameraForce();
        currentBoss.transform.position = new Vector2(currentBoss.transform.position.x, currentBoss.spawnPoint.position.y);

        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.playerAnimObj.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.playerAnimObj.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.playerAnimObj.GetComponent<SpriteRenderer>().flipX = true;

        currentBoss.gameObject.SetActive(true);

        if(bossType == BossType.JOKJEBI)
        {
            currentBoss.GetComponent<Animator>().Play("JokJeBi_Appear");
            yield return new WaitForSeconds(2f);
        }
        else if(bossType == BossType.DOKSURI)
        {
            currentBoss.GetComponent<Animator>().Play("DokSuRi_Appear");
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(2f);
        GameManager.Instance.playerAnimObj.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(1.5f);
        currentBoss.BossStart();
        DOTween.To(() => mobileControllerGroup.alpha, value => mobileControllerGroup.alpha = value, 1, 1f);
        mobileControllerGroup.interactable = true;
        DOTween.To(() => GameManager.Instance.bossBar.alpha, value => GameManager.Instance.bossBar.alpha = value, 1, 1f);
        GameManager.Instance.isBossStart = true;
    }
}
