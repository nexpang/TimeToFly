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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && GameManager.Instance.player.playerState == PlayerState.NORMAL)
        {
            if (GameManager.Instance.player.abilitys[(int)Chickens.BLUE].gameObject.activeSelf)
            {
                if (GameManager.Instance.player._speed != playerSlow)
                {
                    playerDefaultSpeed = GameManager.Instance.player._speed;
                    GameManager.Instance.player._speed = playerSlow;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && GameManager.Instance.player.playerState == PlayerState.NORMAL)
        {
            if(GameManager.Instance.player._speed == playerSlow)
                GameManager.Instance.player._speed = playerDefaultSpeed;
            else
                GameManager.Instance.player._speed = 1f;
        }
    }
}
