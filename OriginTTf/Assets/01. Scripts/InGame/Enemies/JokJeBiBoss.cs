using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JokJeBiBoss : Boss
{
    Animator animator = null;

    public float cameraSpeed = 5f;

    public GameObject warningGround;
    public GameObject warningJumpTxt;
    public GameObject energyGroundObj;
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
            background.backgroundMove.PlayerFollow(false);
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
                print("???? ĵ????");
                nextPatternCancel = false;
                continue;
            }

            switch(currentPattern)
            {
                case 1:
                    int count = 0;
                    while(count < 7)
                    {
                        yield return new WaitForSeconds(0.1f);
                        warningGround.SetActive(true);
                        warningJumpTxt.SetActive(true);
                        yield return new WaitForSeconds(0.1f);
                        warningGround.SetActive(false);
                        warningJumpTxt.SetActive(false);
                        count++;
                    }
                    Pattern1();
                    break;
                case 2:
                    Pattern2();
                    break;
                case 3:
                    yield return new WaitForSeconds(2f);
                    Pattern3();
                    currentPattern = 0;
                    yield return new WaitForSeconds(1f);

                    for (int i = 0; i < weaselSpawnCount; i++)
                    {
                        WeaselEnemy weasel = Instantiate(weaselPrefab, null).GetComponent<WeaselEnemy>();
                        weasel.EnemySpeedChange(6);
                        weasel.EnemyJumpSpeedChange(10);
                        weasel.jumpNodeHeightRequirement = 0.8f;

                        float randomX = Random.Range(Camera.main.transform.position.x - 7, Camera.main.transform.position.x - 5);

                        weasel.transform.position = new Vector2(randomX, 9);
                        ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_Falling, 1, true);
                        yield return new WaitForSeconds(0.6f);
                    }
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

    private void Pattern1() // ?޷????? - ???????? ?????? ?????Ͽ? ???? ????ä???Ѵ?
    {
        animator.Play("JokJeBi_Pattern1");
    }

    private void Pattern2() // ?????? - ???????? ???? ?ļ? ?? ?????? ?????ĸ? ?????Ѵ?
    {
        energyGroundObj.transform.position = new Vector2(GameManager.Instance.player.transform.position.x, energyGroundObj.transform.position.y);
        ParticleManager.CreateWarningPosBox(energyGroundObj.transform.position, new Vector2(920, 226), 1, Color.yellow, Color.red, 0.1f);
        animator.Play("JokJeBi_Pattern2");
    }

    private void Pattern3() // ?????? ?????? - ?ٴ??? ?ļ? ???? ???? ?????????? ????Ʈ????
    {
        animator.Play("JokJeBi_Pattern3");
    }

    private void JokJeBI_HitGroundedPlayer() // ?????? ?????? ??????
    {
        if(GameManager.Instance.player.isGround)
        {
            GameManager.Instance.player.GameOver();
        }
    }

    private void JokJeBI_ShoutingSound() // ?????? ?????? ??????
    {
        ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_BossJokJeBiShout, 1f, true);
    }

    private void JokJeBI_SmallSmashSound() // ?????? ?۰? ???Ž? ??????
    {
        ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_BossJokJeBiSmallSmash, 1f, true);
    }
}
