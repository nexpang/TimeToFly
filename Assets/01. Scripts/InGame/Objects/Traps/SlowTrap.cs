using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    private float playerDefaultSpeed;
    [SerializeField] float playerSlow = 0.75f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && GameManager.Instance.player.playerState == PlayerState.NORMAL)
        {
            playerDefaultSpeed = GameManager.Instance.player._speed;
            GameManager.Instance.player._speed = playerSlow;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && GameManager.Instance.player.playerState == PlayerState.NORMAL)
        {
            GameManager.Instance.player._speed = playerDefaultSpeed;
        }
    }
}
