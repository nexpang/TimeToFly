using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BatBoss : Boss
{
    Animator animator = null;
    private int currentPattern;
    public Transform playerTeleportPoint;
    public Transform cameraTeleportPoint;
    public BoxCollider2D WallLeft;
    public BoxCollider2D WallRight;
    public GameObject beforeBG;
    public GameObject afterBG;

    public float defaultTimer = 120;
    float currentTimer = 0;
    bool patternReady = true;

    public GameObject stalactiteTrapPrefab;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void BossStart()
    {
        cameraStop = false;

        StartCoroutine(BossPattern());
    }

    public override void Update()
    {
        if(GameManager.Instance.isBossStart)
        {
            if (currentTimer < defaultTimer)
            {
                currentTimer += Time.deltaTime;
            }
            else
            {
                // CLEAR
            }
        }

        float barScale = currentTimer / defaultTimer;

        GameManager.Instance.bossBarFill.transform.localScale = new Vector2(barScale, 1);

        GameManager.Instance.bossBarChicken.anchoredPosition = new Vector2(
            (bossBarRectStartAndEnd.x + ((bossBarRectStartAndEnd.y - bossBarRectStartAndEnd.x) * barScale)
            - GameManager.Instance.bossBar.GetComponent<RectTransform>().sizeDelta.x / 2)
            , GameManager.Instance.bossBarChicken.anchoredPosition.y);
    }

    IEnumerator BossPattern()
    {
        while (true)
        {
            yield return new WaitUntil(() => patternReady);
            patternReady = false;

            yield return new WaitForSeconds(5f);

            if (!GameManager.Instance.isBossStart) yield break;

            currentPattern++;

            if (nextPatternCancel)
            {
                print("패턴 캔슬됨");
                nextPatternCancel = false;
                continue;
            }


            switch (currentPattern)
            {
                case 1:
                    transform.DOMoveY(-2, 0.5f).SetRelative();
                    ParticleManager.CreateWarningAnchorBox(new Vector2(-476, -5), new Vector2(952, 790), 2f, Color.yellow, Color.red, 0.2f, 200);
                    yield return new WaitForSeconds(2f);
                    Pattern1();
                    animator.SetInteger("SwingCount", 4);
                    yield return new WaitForSeconds(2f); 
                    break;
                case 2:
                    StartCoroutine(Pattern2());
                    break;
                case 3:
                    Pattern3();
                    currentPattern = 0;
                    break;
            }
        }
    }

    private void Pattern1() // 날개 치기 - 큰 날개로 한 부분을 타격합니다
    {
        animator.Play("Bat_Pattern1");
    }

    private IEnumerator Pattern2() // 종유석 떨구기 - 하늘 위로 사라지면서 카메라 쉐이킹을 줍니다. 그 후 종유석을 다량 떨어트린 후, 다시 내려옵니다. 
    {
        transform.DOMoveY(6, 1.5f).SetRelative();
        yield return new WaitForSeconds(4);
        Event_CameraBigForce();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                StalactiteTrap trap = Instantiate(stalactiteTrapPrefab, null).GetComponent<StalactiteTrap>();
                float randomSpeed = Random.Range(1, 3);
                float randomX = Random.Range(Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7);

                trap.transform.position = new Vector2(randomX, Camera.main.transform.position.y + 5);
                ParticleManager.CreateWarningPosBox(new Vector2(randomX, 4), new Vector2(100, 900), 0.5f, Color.yellow, Color.red, 0.2f, 100);
                trap.Init(randomSpeed, true);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(2);
        transform.DOMoveY(-6, 1.5f).SetRelative();

        patternReady = true;
    }

    private void Pattern3() // 쪼기 - 독수리가 위로 올랐다가 플레이어 위치를 쪼면서 지나간다
    {
        ParticleManager.CreateParticle<Effect_Tooth>(GameManager.Instance.player.transform.position);
        patternReady = true;
    }

    #region ANIMATION_EVENTS

    private void RemoveSwingCount() // 패턴 1 이벤트
    {
        int count = animator.GetInteger("SwingCount");
        animator.SetInteger("SwingCount", count - 1);
    }

    private void Warning_Default_Event() // 패턴 1 이벤트
    {
        int count = animator.GetInteger("SwingCount");

        if (count > 0)
        {
            ParticleManager.CreateWarningAnchorBox(new Vector2(-476, -5), new Vector2(952, 790), 0.5f, Color.yellow, Color.red, 0.2f, 200);
        }
        else
        {
            transform.DOMoveY(2, 0.5f).SetRelative();
            patternReady = true;
        }
    }

    private void Warning_Reverse_Event() // 패턴 1 이벤트
    {
        int count = animator.GetInteger("SwingCount");

        if (count > 0)
        {
            ParticleManager.CreateWarningAnchorBox(new Vector2(476, -5), new Vector2(952, 790), 0.5f, Color.yellow, Color.red, 0.2f, 200);
        }
        else
        {
            transform.DOMoveY(2, 0.5f).SetRelative();
            patternReady = true;
        }
    }

    #endregion
}
