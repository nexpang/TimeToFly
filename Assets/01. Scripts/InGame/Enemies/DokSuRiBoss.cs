using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DokSuRiBoss : Boss
{
    Animator animator = null;

    public float cameraSpeed = 5f;

    private int currentPattern;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void BossStart()
    {
        cameraStop = false;

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

            switch (currentPattern)
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

    private void Pattern1() // 땅 부수기 - 독수리가 플레이어 앞의 땅을 부순다.
    {
        animator.Play("JokJeBi_Pattern1");
    }

    private void Pattern2() // 땅 끌어올리기 - 독수리가 플레이어 뒤에서 부리로 땅을 파 돌을 던진다
    {
        animator.Play("JokJeBi_Pattern2");
    }

    private void Pattern3() // 쪼기 - 독수리가 위로 올랐다가 플레이어 위치를 쪼면서 지나간다
    {
        animator.Play("JokJeBi_Pattern3");
    }
}
