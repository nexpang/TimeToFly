using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 해당 클래스는 직접적으로 호출되지 않고 애니메이션 이벤트에 의해서만 호출됩니다.
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Transform playerRigid = null;

    private Animator animator = null;
    private PlayerController playerController = null;

    private void Start()
    {
        if (!playerRigid)
            return;
        animator = GetComponent<Animator>();
        playerController = playerRigid.GetComponent<PlayerController>();
    }

    void ReadyToJump() // 해당 함수는 startJump1에 들어있음. 목적 : 사운드때문에...
    {
        if(GameManager.Instance)
            GameManager.Instance.player.isPressJump = true;
    }

    void InGround() // 해당 함수는 Player_Idle에 들어있음.
    {
        if (GameManager.Instance)
        {
            animator.SetBool("land", false);
            animator.SetBool("falling", false);

            playerController.isAnimationStun = false;
            GameManager.Instance.player.isPressJump = false;
        }
      
    }

    void PlayerStunEventStart() // 해당 함수는 Player_AfterJumpWait에 들어있음.
    {
        if (GameManager.Instance)
        {
            animator.SetBool("land", false);
            animator.SetBool("falling", false);

            playerController.isAnimationStun = true;
        }
    }

    void PlayerStunEventEnd() // 해당 함수는 Player_AfterJumpWait에 들어있음.
    {
        if (GameManager.Instance)
            playerController.isAnimationStun = false;
    }

    public void PlayerDeadAnimEnd() // 해당 함수는 Player_Death, Player_FallenDeath에 들어있음.
    {
        if (GameManager.Instance)
        {
            Ability_FutureCreate ability = (Ability_FutureCreate)GameManager.Instance.player.abilitys[(int)Chickens.BROWN];
            if (ability.enabled && ability.isAbilityEnable) // 만약 능력 1이고 자고있는 상태면
            {
                ability.ResetPlayer(); // 리셋시킨다.
                playerController.isAnimationStun = false;
                animator.Play("Player_Idle");
            }
            else // 아니라면
            {
                if (!GameManager.Instance.IsInfinityLife) GameManager.Instance.tempLife--;
                SecurityPlayerPrefs.SetInt(GameManager.Instance.tempLifekey, GameManager.Instance.tempLife);
                // TO DO : 만약 목숨이 -1이라면, 게임오버 시킨다.
                if(GameManager.Instance.tempLife < 0)
                {
                    GameManager.Instance.LifeOver();
                }
                else
                {
                    GameManager.Instance.SceneReset();
                }
            }
        }
    }
}
