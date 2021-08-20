using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JokJeBiBoss : Boss
{
    Animator animator = null;

    public float cameraSpeed = 5f;

    public GameObject weaselPrefab;
    public int weaselSpawnCount = 3;

    private int currentPattern;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void BossStart()
    {
        cameraStop = false;

        animator.Play("JokJeBi_Walk");
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
        while(true)
        {
            yield return new WaitForSeconds(5f);

            if (!GameManager.Instance.isBossStart) yield break;

            currentPattern++;

            if (nextPatternCancel)
            {
                print("패턴 캔슬됨");
                nextPatternCancel = false;
                continue;
            }

            switch(currentPattern)
            {
                case 1:
                    Pattern1();
                    break;
                case 2:
                    Pattern2();
                    break;
                case 3:
                    Pattern3();
                    currentPattern = 0;
                    yield return new WaitForSeconds(1f);

                    for (int i = 0; i < weaselSpawnCount; i++)
                    {
                        GameObject weasel = Instantiate(weaselPrefab, null);

                        float randomX = Random.Range(Camera.main.transform.position.x - 5, Camera.main.transform.position.x);

                        weasel.transform.position = new Vector2(randomX, 7);
                        yield return new WaitForSeconds(0.3f);
                    }
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

    private void Pattern1() // 달려들기 - 족제비가 앞으로 돌진하여 닭을 잡아채려한다
    {
        animator.Play("JokJeBi_Pattern1");
    }

    private void Pattern2() // 충격파 - 족제비가 땅을 쳐서 맵 반절의 충격파를 생성한다
    {
        animator.Play("JokJeBi_Pattern2");
    }

    private void Pattern3() // 족제비 떨구기 - 바닥을 쳐서 나무 위에 족제비들을 떨어트린다
    {
        animator.Play("JokJeBi_Pattern3");
    }
}
