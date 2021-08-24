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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void BossStart()
    {
        cameraStop = false;

        animator.Play("DokSuRi_Idle");
        StartCoroutine(BossPattern());
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
                print("패턴 캔슬됨");
                nextPatternCancel = false;
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
                    Pattern2();
                    yield return new WaitForSeconds(2f);
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
    }

    private void PatternReady()
    {
        animator.Play("DokSuRi_PatternReady");
    }

    private void Pattern1() // 땅 끌어올리기 - 독수리가 플레이어 뒤에서 부리로 땅을 파 돌을 던진다
    {

    }

    private void Pattern2()  // 땅 부수기 - 독수리가 플레이어 앞의 땅을 부순다.
    {

    }

    private void Pattern3() // 쪼기 - 독수리가 위로 올랐다가 플레이어 위치를 쪼면서 지나간다
    {

    }
}
