using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : ResetAbleTrap
{
    [SerializeField] Sprite defaultSprite = null;
    [SerializeField] Sprite triggeredSprite = null;
    Transform playerRb;

    bool isTrigger = false;

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

            GetComponent<SpriteRenderer>().sprite = triggeredSprite;
            SFXManager.PlaySound(SFXManager.Instance.Audio_bearTrap, 0.8f, true);
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
