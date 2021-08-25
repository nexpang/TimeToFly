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
            yield return new WaitForSeconds(5f);

            if (!GameManager.Instance.isBossStart) yield break;

            currentPattern++;

            if (nextPatternCancel)
            {
                print("���� ĵ����");
                nextPatternCancel = false;
                continue;
            }

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

    private void Pattern1() // �� ����ø��� - �������� �÷��̾� �ڿ��� �θ��� ���� �� ���� ������
    {

    }

    private void Pattern2()  // �� �μ��� - �������� �÷��̾� ���� ���� �μ���.
    {

    }

    private void Pattern3() // �ɱ� - �������� ���� �ö��ٰ� �÷��̾� ��ġ�� �ɸ鼭 ��������
    {

    }
}
