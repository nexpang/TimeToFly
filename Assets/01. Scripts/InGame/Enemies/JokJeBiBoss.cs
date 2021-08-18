using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JokJeBiBoss : Boss
{
    Animator animator = null;
    public float cameraSpeed = 5f;
    public BackgroundMove[] backgroundMoves; // 먼 것부터 집어넣어라.

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void BossStart()
    {
        GameManager.Instance.curStageInfo.virtualCamera.Follow = null;
        backgroundMoves[0].SpeedChange(0.3f);
        backgroundMoves[1].SpeedChange(0.35f);
        backgroundMoves[2].SpeedChange(0.4f);
        backgroundMoves[3].SpeedChange(0.45f);
        backgroundMoves[4].SpeedChange(0.5f);
        cameraStop = false;

        animator.Play("JokJeBi_Walk");
    }

    public override void Update()
    {
        base.Update();
        Move();
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
}
