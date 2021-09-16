using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item_1up : MonoBehaviour, IItemAble
{
    private SpriteRenderer spriteRenderer;
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
            ParticleManager.CreateParticle<Effect_Heart>(transform.position);
            GameManager.Instance.player.EffectShow_1up();
            SecurityPlayerPrefs.SetInt(GameManager.Instance.tempLifekey, GameManager.Instance.tempLife++);
            gameObject.SetActive(false);
            //GameManager.Instance.tempLife = SecurityPlayerPrefs.GetInt(GameManager.Instance.tempLifekey, GameManager.Instance.tempLife);
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
