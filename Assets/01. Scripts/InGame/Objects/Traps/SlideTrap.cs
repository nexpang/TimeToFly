using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideTrap : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && GameManager.Instance.player.playerState == PlayerState.NORMAL)
        {
            //playerDefaultSpeed = GameManager.Instance.player._speed;
            //GameManager.Instance.player._speed = playerSlow;
            StartCoroutine(GameManager.Instance.player.MoveSlide());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && GameManager.Instance.player.playerState == PlayerState.NORMAL)
        {
            GameManager.Instance.player.MoveSlideStop();
            //GameManager.Instance.player._speed = playerDefaultSpeed;
        }
    }
}
