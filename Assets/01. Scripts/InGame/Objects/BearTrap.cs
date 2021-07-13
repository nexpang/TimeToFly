using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : ResetAbleTrap
{
    [SerializeField] Sprite defaultSprite = null;
    [SerializeField] Sprite triggeredSprite = null;
    Transform playerRb;
    [Header("È¿°úÀ½")]
    [SerializeField] AudioClip Audio_bearTrap = null;

    bool isTrigger = false;

    private void Awake()
    {
        playerRb = FindObjectOfType<PlayerController>().transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isTrigger) return;

            isTrigger = true;

            GetComponent<SpriteRenderer>().sprite = triggeredSprite;
            PlaySFX.PlaySound(Audio_bearTrap, 0.8f, true);
            GetComponent<SpriteRenderer>().sortingOrder = 18;
            GameManager.Instance.player.GameOver();
            playerRb.position = transform.position;
            playerRb.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public override void Reset()
    {
        isTrigger = false;
        GetComponent<SpriteRenderer>().sortingOrder = 4;
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }
}
