using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator = null;
    private PlayerController playerController = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void InGround() // �ش� �Լ��� Player_Idle�� �������.
    {
        animator.SetBool("land", false);
        animator.SetBool("falling", false);
    }

    void PlayerStunEventStart() // �ش� �Լ��� Player_AfterJumpWait�� �������.
    {
        animator.SetBool("land", false);
        animator.SetBool("falling", false);
        playerController.isStun = true;
    }

    void PlayerStunEventEnd() // �ش� �Լ��� Player_AfterJumpWait�� �������.
    {
        playerController.isStun = false;
    }
}
