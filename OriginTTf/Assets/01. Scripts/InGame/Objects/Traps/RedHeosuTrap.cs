using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHeosuTrap : ResetAbleTrap
{
    Transform playerRb;
    bool isTrigger = false;
    Animator animator = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerRb = GameManager.Instance.player.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isTrigger) return;

            isTrigger = true;
            animator.Play("RedHeosu_Trigger");
            ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_HeosuFall, 1f, true);
            GameManager.Instance.player.GameOver();
            playerRb.position = transform.position;
            playerRb.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public override void Reset()
    {
        isTrigger = false;
        animator.Play("RedHeosu_Idle");
    }
}
