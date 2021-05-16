using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SleepingPlayer : MonoBehaviour
{
    private Animator animator = null;
    [SerializeField] Animator bubbleAnimator = null;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("DEADABLE"))
        {
            animator.SetTrigger("DeadT");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("DEADABLE"))
        {
            animator.SetTrigger("DeadT");
        }
    }

    public void PlayerDeadAnimEnd() // �ش� �Լ��� Sleeping_Player_Death�� �������.
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BubbleAwake()
    {
        bubbleAnimator.SetTrigger("Awake");
    }

}
