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
        animator = GetComponent<Animator>();
        playerController = playerRigid.GetComponent<PlayerController>();
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

    public void PlayerDeadAnimEnd() // �ش� �Լ��� Player_Death, Player_FallenDeath�� �������.
    {
        Ability_FutureCreate ability = FindObjectOfType<Ability_FutureCreate>();
        if (ability != null && ability.IsSleep()) // ���� �ɷ� 1�̰� �ڰ��ִ� ���¸�
        {
            ability.ResetPlayer(); // ���½�Ų��.
            playerController.isStun = false;
            animator.Play("Player_Idle");
        }
        else // �ƴ϶��
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // TO DO : ��ȭ���̾��ٸ� �ٽ� ���ŷ�, �ƴ϶�� ���� ����
        }
    }
}