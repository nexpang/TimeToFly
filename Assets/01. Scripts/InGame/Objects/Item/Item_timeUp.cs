using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item_timeUp : MonoBehaviour, IItemAble
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] int plusTime = 10;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.Instance.player.playerState == PlayerState.NORMAL)
        {

            if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].gameObject.activeSelf)
            {
                if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].isAbilityEnable)
                {
                    return;
                }
            }

            ObjectManager.PlaySound(GameManager.Instance.player.Audio_playerHeartEat, 1, true);
            ObjectManager.PlaySound(GameManager.Instance.player.Audio_playerTimeEat, 1, true);
            GameManager.Instance.player.EffectShow_TimerUp(plusTime);
            GameManager.Instance.timer += plusTime;
            gameObject.SetActive(false);
        }
    }

    public void CreateReset(ItemBlockType type)
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        transform.localScale = Vector2.zero;

        if (type == ItemBlockType.BRICK)
        {
            transform.DOMoveY(0.5f, 0.5f).SetRelative();
        }
        else if (type == ItemBlockType.BLOCK)
        {
            transform.DOMoveY(1, 0.5F).SetRelative();
        }

        transform.DOScale(1, 0.5f);
        spriteRenderer.DOFade(1, 0.5f);
    }
}
