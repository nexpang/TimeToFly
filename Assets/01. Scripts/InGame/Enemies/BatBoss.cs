using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BatBoss : Boss
{
    Animator animator = null;
    public Animator soundEffectAnimator = null;
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

    [Header("패턴 1")]
    public GameObject batBody;

    [Header("패턴 3")]
    public GameObject stalactiteTrapPrefab;

    [Header("패턴 5")]
    public GameObject[] bats;

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
                    break;
                case 2:
                    StartCoroutine(Pattern2());
                    break;
                case 3:
                    StartCoroutine(Pattern3());
                    break;
                case 4:
                    StartCoroutine(Pattern4());
                    break;
                case 5:
                    StartCoroutine(Pattern5());
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
        yield return new WaitForSeconds(2);
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

    private IEnumerator Pattern3() // 물기 - 박쥐가 플레이어 X로 계속 움직이며 꺠문다
    {
        StartCoroutine(Pattern3_PlayerMove());
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < 5; i++)
        {
            Vector2 targetPos = GameManager.Instance.player.transform.position;
            ParticleManager.CreateWarningPosBox(new Vector2(targetPos.x, targetPos.y), new Vector2(100, 100), 0.3f, Color.yellow, Color.red, 0.08f, 75);
            yield return new WaitForSeconds(0.3f);
            ParticleManager.CreateParticle<Effect_Tooth>(targetPos);
            yield return new WaitForSeconds(0.5f);
        }

        for (int i = 0; i < 5; i++)
        {
            Vector2 targetPos = GameManager.Instance.player.transform.position;
            ParticleManager.CreateWarningPosBox(new Vector2(targetPos.x, targetPos.y), new Vector2(100, 100), 0.1f, Color.yellow, Color.red, 0.08f, 75);
            yield return new WaitForSeconds(0.1f);
            ParticleManager.CreateParticle<Effect_Tooth>(targetPos);
            yield return new WaitForSeconds(0.1f);
        }

        transform.DOMove(new Vector2(spawnPoint.position.x, spawnPoint.position.y + 13), 1);

        patternReady = true;
    }

    private IEnumerator Pattern3_PlayerMove()
    {
        while (true)
        {
            yield return null;

            Vector2 targetVec = new Vector2(GameManager.Instance.player.transform.position.x, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, targetVec, 10 * Time.deltaTime);
            if (patternReady)
            {
                break;
            }
        }
    }

    private IEnumerator Pattern4() // 음파 - 무조건 맞고 맞으면 방향키 변환
    {
        soundEffectAnimator.Play("BatSoundEffect_Trigger");
        GameManager.Instance.player.reverseKey = true;
        GlitchEffect.Instance.colorIntensity = 0.100f;
        GlitchEffect.Instance.flipIntensity = 0.194f;
        GlitchEffect.Instance.intensity = 0.194f;
        patternReady = true;
        yield return new WaitForSeconds(25);
        GameManager.Instance.player.reverseKey = false;
        GlitchEffect.Instance.colorIntensity = 0f;
        GlitchEffect.Instance.flipIntensity = 0f;
        GlitchEffect.Instance.intensity = 0f;
    }

    private IEnumerator Pattern5() // 박치기 - 2개 분신 생성, 그리고 박치기
    {
        transform.DOMoveY(6, 1.5f).SetRelative();
        yield return new WaitForSeconds(2);
        animator.Play("Bat_Pattern5");
        // 박쥐 분신 키고


        for (int i = 0; i < 3; i++)
        {
            List<float> appearPoints = new List<float>() { Camera.main.transform.position.x - 8, Camera.main.transform.position.x, Camera.main.transform.position.x + 8 };
            for (int j = 0; j < 3; j++)
            {
                int randomIndex = Random.Range(0, appearPoints.Count);
                for(int k = 0; k<3;k++)
                {
                    if (j == k)
                    {
                        bats[k].transform.position = new Vector2(appearPoints[randomIndex], transform.position.y);
                    }
                }
                appearPoints.RemoveAt(randomIndex);
            }
            ParticleManager.CreateWarningAnchorBox(new Vector2(-680, -3.8f), new Vector2(560, 1080), 1f, Color.yellow, Color.red, 0.2f, 200);
            ParticleManager.CreateWarningAnchorBox(new Vector2(2, -3.8f), new Vector2(705, 1080), 1f, Color.yellow, Color.red, 0.2f, 200);
            ParticleManager.CreateWarningAnchorBox(new Vector2(675, -3.8f), new Vector2(560, 1080), 1f, Color.yellow, Color.red, 0.2f, 200);
            yield return new WaitForSeconds(2);
            transform.DOMoveY(-6, 1f).SetRelative();
            yield return new WaitForSeconds(2f);
            transform.DOMoveY(-20, 1.5f).SetRelative().OnComplete(() =>
            {
                transform.position = new Vector2(spawnPoint.position.x, spawnPoint.position.y + 19);
            });
            yield return new WaitForSeconds(2.5f);
        }

        animator.Play("Bat_Idle");
        bats[0].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.DOMove(new Vector2(spawnPoint.position.x, spawnPoint.position.y + 13), 1);
        patternReady = true;
    }

    #region ANIMATION_EVENTS

    private void LeftMove() // 패턴 1 이벤트
    {
        batBody.transform.DOLocalMoveX(-260.95f, 0.35f).SetLoops(2,LoopType.Yoyo);
    }

    private void RightMove() // 패턴 1 이벤트
    {
        batBody.transform.DOLocalMoveX(260.95f, 0.35f).SetLoops(2, LoopType.Yoyo);
    }

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

    private void Bat_ShoutingSound() // 박쥐 샤우팅 사운드
    {
        ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_BossBat, 1f, true);
    }

    #endregion
}
