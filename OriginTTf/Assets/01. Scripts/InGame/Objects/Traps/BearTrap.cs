using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : ResetAbleTrap
{
    Transform playerRb;

    bool isTrigger = false;

    private void Start()
    {
        if(GameManager.Instance != null)
            playerRb = GameManager.Instance.player.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isTrigger) return;

            isTrigger = true;

            GetComponent<SpriteRenderer>().sprite = ObjectManager.Instance.Spr_bearTrapTriggered;
            ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_bearTrap, 0.8f, true);
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
        GetComponent<SpriteRenderer>().sprite = ObjectManager.Instance.Spr_bearTrapDefault;
    }
}
