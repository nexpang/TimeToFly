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
                print("???? ĵ????");
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
                    ParticleManager.CreateWarningAnchorBox(new Vector2(-615,115), new Vector2(690, 850), 1, Color.yellow, Color.red, 0.1f);
                    Event_CameraStop();
                    yield return new WaitForSeconds(1f);
                    Event_CameraResume();
                    Pattern1();

                    yield return new WaitForSeconds(2f);
                    break;
                case 2:
                    ParticleManager.CreateWarningAnchorBox(new Vector2(0, 270), new Vector2(1920, 540), 3, Color.yellow, Color.red, 0.1f);
                    ParticleManager.CreateWarningAnchorBox(new Vector2(255, -154), new Vector2(893, 309), 3, Color.yellow, Color.red, 0.1f);
                    Event_CameraStop();
                    yield return new WaitForSeconds(4f);
                    Pattern3();
                    currentPattern = 0;

                    yield return new WaitForSeconds(2f);
                    ParticleManager.CreateWarningAnchorBox(new Vector2(-615, 115), new Vector2(690, 850), 1, Color.yellow, Color.red, 0.1f);
                    yield return new WaitForSeconds(1f);
                    break;
            }
        }
    }

    private void Move()
    {
        if (cameraStop) return;
        if (GameManager.Instance.player.playerState == PlayerState.DEAD) return;

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

    private void Pattern1() // ?? ?????ø??? - ???????? ?÷??̾? ?ڿ??? ?θ??? ???? ?? ???? ??????
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
        //print("?Ÿ? : " + dist);
        if(dist <= 0.1f)
        {
            //print("2 ???? ????");
            return true;
        }
        else if(dist <= 10f)
        {
            //print("?ٸ? ???? ĵ??");
            isPattern2 = true;
        }
        return false;
    }

    private IEnumerator Pattern2()  // ?? ?μ??? - ???????? ?÷??̾? ???? ???? ?μ???.
    {
        isPattern2Playing = true;

        Event_CameraStop();
        PatternReady();
        ParticleManager.CreateWarningAnchorBox(new Vector2(615, 115), new Vector2(690, 850), 1, Color.yellow, Color.red, 0.1f);
        yield return new WaitForSeconds(1f);

        animator.Play("DokSuRi_Pattern2");

        yield return new WaitForSeconds(2f);
        ParticleManager.CreateWarningAnchorBox(new Vector2(-615, 115), new Vector2(690, 850), 1, Color.yellow, Color.red, 0.1f);
        yield return new WaitForSeconds(1f);
        isPattern2 = false;
        isPattern2Playing = false;
    }

    public void BreakGround()
    {
        ParticleManager.CreateParticle<Effect_StoneFrag>(new Vector2(pattern2Poss[pattern2Idx].position.x+3f, 0f));
        pattern2Poss[pattern2Idx].gameObject.SetActive(false);
        pattern2Idx++;
    }

    private void Pattern3() // ?ɱ? - ???????? ???? ?ö??ٰ? ?÷??̾? ??ġ?? ?ɸ鼭 ????????
    {
        animator.Play("DokSuRi_Pattern3");
    }


    private void Eagle_AmbeintSound() // ?????? ???? ??????
    {
        ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_BossEagleAmbient, 1f, true);
    }

    private void Eagle_ShoutSound() // ?????? ?????? ??????
    {
        ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_BossEagleShout, 1f, true);
    }
}