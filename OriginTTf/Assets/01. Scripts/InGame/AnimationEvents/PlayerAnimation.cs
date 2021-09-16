using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �ش� Ŭ������ ���������� ȣ����� �ʰ� �ִϸ��̼� �̺�Ʈ�� ���ؼ��� ȣ��˴ϴ�.
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

    void ReadyToJump() // �ش� �Լ��� startJump1�� �������. ���� : ���嶧����...
    {
        if(GameManager.Instance)
            GameManager.Instance.player.isPressJump = true;
    }

    void InGround() // �ش� �Լ��� Player_Idle�� �������.
    {
        if (GameManager.Instance)
        {
            animator.SetBool("land", false);
            animator.SetBool("falling", false);

            playerController.isAnimationStun = false;
            GameManager.Instance.player.isPressJump = false;
        }
      
    }

    void PlayerStunEventStart() // �ش� �Լ��� Player_AfterJumpWait�� �������.
    {
        if (GameManager.Instance)
        {
            animator.SetBool("land", false);
            animator.SetBool("falling", false);

            playerController.isAnimationStun = true;
        }
    }

    void PlayerStunEventEnd() // �ش� �Լ��� Player_AfterJumpWait�� �������.
    {
        if (GameManager.Instance)
            playerController.isAnimationStun = false;
    }

    public void PlayerDeadAnimEnd() // �ش� �Լ��� Player_Death, Player_FallenDeath�� �������.
    {
        if (GameManager.Instance)
        {
            Ability_FutureCreate ability = (Ability_FutureCreate)GameManager.Instance.player.abilitys[(int)Chickens.BROWN];
            if (ability.enabled && ability.isAbilityEnable) // ���� �ɷ� 1�̰� �ڰ��ִ� ���¸�
            {
                ability.ResetPlayer(); // ���½�Ų��.
                playerController.isAnimationStun = false;
                animator.Play("Player_Idle");
            }
            else // �ƴ϶��
            {
                if (!GameManager.Instance.IsInfinityLife) GameManager.Instance.tempLife--;
                SecurityPlayerPrefs.SetInt(GameManager.Instance.tempLifekey, GameManager.Instance.tempLife);
                // TO DO : ���� ����� -1�̶��, ���ӿ��� ��Ų��.
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
