using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DisappearGround : ResetAbleTrap
{
    private bool isActive = false;
    [SerializeField] private float disappearTime = 1f;
    [SerializeField] private float waitDisappear = 1f;
    private SpriteRenderer spriteRenderer;
    private new PolygonCollider2D collider2D;
    private bool isFutureDead = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<PolygonCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && GameManager.Instance.player.playerState == PlayerState.NORMAL)
        {
            if (isActive)
                return;
            if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].gameObject.activeSelf)
            {
                if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].isAbilityEnable)
                {
                    isFutureDead = true;
                }
            }
            Invoke("Disappear", waitDisappear);
        }

    }

    void Disappear()
    {
        spriteRenderer.DOColor(new Color(0f, 0f, 0f, 0f), disappearTime).OnComplete(() =>
        {
            collider2D.enabled = false;
        });
    }

    public override void Reset()
    {
        if (!isFutureDead) return;
        spriteRenderer.color = Color.white;
        collider2D.enabled = true;
    }
}
