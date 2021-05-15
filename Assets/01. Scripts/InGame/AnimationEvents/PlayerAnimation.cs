using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Transform playerRigid = null;

    private Animator animator = null;
    private PlayerController playerController = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = playerRigid.GetComponent<PlayerController>();
    }

    void InGround() // 해당 함수는 Player_Idle에 들어있음.
    {
        animator.SetBool("land", false);
        animator.SetBool("falling", false);
    }

    void PlayerStunEventStart() // 해당 함수는 Player_AfterJumpWait에 들어있음.
    {
        animator.SetBool("land", false);
        animator.SetBool("falling", false);
        playerController.isStun = true;
    }

    void PlayerStunEventEnd() // 해당 함수는 Player_AfterJumpWait에 들어있음.
    {
        playerController.isStun = false;
    }

    void PlayerDeadAnimEnd()
    {
        // TO DO : 녹화중이었다면 다시 과거로, 아니라면 게임 오버
    }
}
