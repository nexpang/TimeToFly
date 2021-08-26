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
        Move();
    }

    IEnumerator BossPattern()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (!GameManager.Instance.isBossStart) yield break;

            currentPattern++;

            if (nextPatternCancel)
            {
                print("���� ĵ����");
                nextPatternCancel = false;
                continue;
            }

            PatternReady();
            yield return new WaitForSeconds(1f);

            switch (currentPattern)
            {
                case 1:
                    currentPattern++;
                    Pattern1();


                    yield return new WaitForSeconds(2f);
                    break;
                case 2:
                    Pattern2();
                    break;
                case 3:
                    Pattern3();
                    currentPattern = 0;

                    yield return new WaitForSeconds(2f);
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

    private void Pattern1() // �� ����ø��� - �������� �÷��̾� �ڿ��� �θ��� ���� �� ���� ������
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

    private void Pattern2()  // �� �μ��� - �������� �÷��̾� ���� ���� �μ���.
    {

    }

    private void Pattern3() // �ɱ� - �������� ���� �ö��ٰ� �÷��̾� ��ġ�� �ɸ鼭 ��������
    {
        animator.Play("DokSuRi_Pattern3");
    }
}
