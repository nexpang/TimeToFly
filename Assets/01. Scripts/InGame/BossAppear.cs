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
        StartCoroutine(LateStart());
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

    IEnumerator LateStart()
    {
        yield return null;
        GameManager.Instance.isBossStage = true;
        GameManager.Instance.cameraLimitWall.SetActive(true);
        if (GameManager.Instance.player.abilityNumber == 1)
        {
            GameManager.Instance.player.PlayerBarrierSet();
        }
    }

    IEnumerator BossStart()
    {
        DOTween.To(() => GameManager.Instance.bgAudioSource.volume, value => GameManager.Instance.bgAudioSource.volume = value, 0, 2);
        

        if (bossType == BossType.JOKJEBI)
        {
            GameManager.Instance.player.SetStun(10);
        }
        else if (bossType == BossType.DOKSURI)
        {
            GameManager.Instance.player.SetStun(10);
        }
        else if (bossType == BossType.BAT)
        {
            GameManager.Instance.player.SetStun(10);
            GameManager.Instance.FadeInOut(2.5f, 2f, 1, () =>
             {
                 GameManager.Instance.curStageInfo.virtualCamera.Follow = null;
                 GameManager.Instance.curStageInfo.virtualCamera.transform.position = currentBoss.GetComponent<BatBoss>().cameraTeleportPoint.position;
                 GameManager.Instance.player.transform.position = currentBoss.GetComponent<BatBoss>().playerTeleportPoint.position;
                 currentBoss.GetComponent<BatBoss>().WallLeft.offset = Vector2.zero;
                 currentBoss.GetComponent<BatBoss>().WallRight.offset = Vector2.zero;

                 currentBoss.GetComponent<BatBoss>().beforeBG.SetActive(false);
                 currentBoss.GetComponent<BatBoss>().afterBG.SetActive(true);
                 StartCoroutine(BatBossStart());
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
            yield return new WaitForSeconds(4f);
            GameManager.Instance.playerAnimObj.GetComponent<SpriteRenderer>().flipX = false;
            yield return new WaitForSeconds(1.5f);
        }
        else if(bossType == BossType.DOKSURI)
        {
            currentBoss.GetComponent<Animator>().Play("DokSuRi_Appear");
            ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_BossEagleShout, 1f, true);
            yield return new WaitForSeconds(3f);
            GameManager.Instance.playerAnimObj.GetComponent<SpriteRenderer>().flipX = false;
            yield return new WaitForSeconds(1.5f);
            currentBoss.GetComponent<Animator>().Play("DokSuRi_Idle");
            yield return new WaitForSeconds(2f);
        }

        currentBoss.BossStart();
        GameManager.Instance.bgAudioSource.clip = ObjectManager.Instance.soundData.BGM_Boss;
        GameManager.Instance.bgAudioSource.volume = 1;
        GameManager.Instance.bgAudioSource.Play();
        DOTween.To(() => mobileControllerGroup.alpha, value => mobileControllerGroup.alpha = value, 1, 1f);
        mobileControllerGroup.interactable = true;
        DOTween.To(() => GameManager.Instance.bossBar.alpha, value => GameManager.Instance.bossBar.alpha = value, 1, 1f);
        GameManager.Instance.isBossStart = true;
    }

    IEnumerator BatBossStart()
    {
        yield return new WaitForSeconds(5);

        DOTween.To(() => mobileControllerGroup.alpha, value => mobileControllerGroup.alpha = value, 0, 1f);
        mobileControllerGroup.interactable = false;

        yield return new WaitForSeconds(3);
        GameManager.Instance.CameraImpulse(0.25f, 1f, 0.25f, 2);
        currentBoss.Event_CameraForce();
        currentBoss.transform.position = new Vector2(currentBoss.spawnPoint.position.x, currentBoss.spawnPoint.position.y);

        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.playerAnimObj.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.playerAnimObj.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.playerAnimObj.GetComponent<SpriteRenderer>().flipX = true;

        currentBoss.gameObject.SetActive(true);
        currentBoss.transform.DOMoveY(13, 2).SetRelative();
        yield return new WaitForSeconds(4f);
        GameManager.Instance.playerAnimObj.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(1.5f);

        currentBoss.BossStart();
        GameManager.Instance.bgAudioSource.clip = ObjectManager.Instance.soundData.BGM_Boss;
        GameManager.Instance.bgAudioSource.volume = 1;
        GameManager.Instance.bgAudioSource.Play();
        DOTween.To(() => mobileControllerGroup.alpha, value => mobileControllerGroup.alpha = value, 1, 1f);
        mobileControllerGroup.interactable = true;
        DOTween.To(() => GameManager.Instance.bossBar.alpha, value => GameManager.Instance.bossBar.alpha = value, 1, 1f);
        GameManager.Instance.isBossStart = true;
    }
}
