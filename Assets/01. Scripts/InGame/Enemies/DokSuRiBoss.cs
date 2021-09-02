using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DokSuRiBoss : Boss
{
    Animator animator = null;

    public float cameraSpeed = 5f;

    public GameObject body, rock;

    private int currentPattern;

    public Transform[] pattern2Poss;
    private int pattern2Idx = 0;

    private bool isPattern2 = false;
    private bool isPattern2Playing = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void BossStart()
    {
        cameraStop = false;

        body.GetComponent<BoxCollider2D>().enabled = true;
        animator.Play("DokSuRi_Idle");
        StartCoroutine(BossPattern());
        foreach (BackgroundSpeed background in backgroundMoves)
        {
            background.backgroundMove.SpeedChange(background.speed);
        }
    }

    public override void Update()
    {
        base.Update();
        if(pattern2Idx<3)
        {
            if (!isPattern2Playing && CheckPattern2())
            {
                StartCoroutine(Pattern2());
            }
        }
        Move();
    }

    IEnumerator BossPattern()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (isPattern2) continue;
            yield return new WaitForSeconds(4.9f);

            if (!GameManager.Instance.isBossStart) yield break;

            currentPattern++;

            if (nextPatternCancel)
            {
                print("패턴 캔슬됨");
                nextPatternCancel = false;
                continue;
            }

            if (isPattern2)
            {
                currentPattern--;
                continue;
            }

            PatternReady();
            yield return new WaitForSeconds(1f);

            switch (currentPattern)
            {
                case 1:
                    Pattern1();

                    yield return new WaitForSeconds(2f);
                    break;
                case 2:
                    Pattern3();
                    currentPattern = 0;

                    yield return new WaitForSeconds(3f);
                    break;
            }
        }
    }

    private void Move()
    {
        if (cameraStop) return;

        if (GameManager.Instance.curStageInfo.virtualCamera.transform.position.x > startAndEnd.y)
        {
            return;
        }

        GameManager.Instance.curStageInfo.virtualCamera.transform.Translate(Vector2.right * cameraSpeed * Time.deltaTime);
    }

    private void PatternReady()
    {
        animator.Play("DokSuRi_PatternReady");
    }

    private void Pattern1() // 땅 끌어올리기 - 독수리가 플레이어 뒤에서 부리로 땅을 파 돌을 던진다
    {
        if(rock.activeSelf)
        {
            Pattern3();
        }
        else
        {
            animator.Play("DokSuRi_Pattern1");
        }
    }

    public void Pattern1Rock()
    {
        float dist = Mathf.Abs(transform.position.x - GameManager.Instance.player.transform.position.x);
        float power = dist*0.8f;

        rock.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350f, -100f);
        rock.SetActive(true);

        rock.GetComponent<Rigidbody2D>().AddForce(Vector2.up*10f, ForceMode2D.Impulse);
        rock.GetComponent<Rigidbody2D>().AddForce(Vector2.right * (power > 1.2f ? power : 1.2f), ForceMode2D.Impulse);
    }

    private bool CheckPattern2()
    {
        if (isPattern2Playing) return false;
        float dist = Mathf.Abs(pattern2Poss[pattern2Idx].position.x - Camera.main.transform.position.x);
        //print("거리 : " + dist);
        if(dist <= 0.1f)
        {
            print("2 패턴 시작");
            return true;
        }
        else if(dist <= 10f)
        {
            print("다른 패턴 캔슬");
            isPattern2 = true;
        }
        return false;
    }

    private IEnumerator Pattern2()  // 땅 부수기 - 독수리가 플레이어 앞의 땅을 부순다.
    {
        isPattern2Playing = true;

        Event_CameraStop();
        PatternReady();

        yield return new WaitForSeconds(1f);

        animator.Play("DokSuRi_Pattern2");

        yield return new WaitForSeconds(3f);

        isPattern2 = false;
        isPattern2Playing = false;
    }

    public void BreakGround()
    {
        ParticleManager.CreateParticle<Effect_StoneFrag>(new Vector2(pattern2Poss[pattern2Idx].position.x+3f, 0f));
        pattern2Poss[pattern2Idx].gameObject.SetActive(false);
        pattern2Idx++;
    }

    private void Pattern3() // 쪼기 - 독수리가 위로 올랐다가 플레이어 위치를 쪼면서 지나간다
    {
        animator.Play("DokSuRi_Pattern3");
    }


    private void Eagle_AmbeintSound() // 독수리 날개 사운드
    {
        ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_BossEagleAmbient, 1f, true);
    }

    private void Eagle_ShoutSound() // 독수리 샤우팅 사운드
    {
        ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_BossEagleShout, 1f, true);
    }
}