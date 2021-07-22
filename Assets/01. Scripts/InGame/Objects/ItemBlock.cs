using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemBlockType
{
    BRICK,
    BLOCK
}

public class ItemBlock : ResetAbleTrap
{
    public ItemBlockType blockType = ItemBlockType.BRICK;
    public ParticleType particleType = ParticleType.BOX;
    public GameObject item;

    bool isTrigger = false;
    bool isRealTrigger = false;

    PlayerController pc;
    SpriteRenderer sr;
    Rigidbody2D rb;

    private void Start()
    {
        pc = GameManager.Instance.player;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isTrigger) return;

            if (pc.GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                isTrigger = true;

                BreakEvent();
                // TO DO :소리가 있다면 실행

                if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].gameObject.activeSelf)
                {
                    if (!GameManager.Instance.player.abilitys[(int)Chickens.BROWN].isAbilityEnable)
                    {
                        isRealTrigger = true;
                    }
                }
            }
        }
    }

    void BreakEvent()
    {
        pc.GetComponent<Rigidbody2D>().velocity = new Vector2(pc.GetComponent<Rigidbody2D>().velocity.x, pc.GetComponent<Rigidbody2D>().velocity.y * -0.6f);

        if (blockType == ItemBlockType.BRICK)
        {
            if(item != null)
            {
                GameObject itemObj = Instantiate(item, GameManager.Instance.prefabContainer);
                itemObj.transform.position = transform.position;
                SFXManager.PlaySound(SFXManager.Instance.Audio_BlockItem, 1, true);

                IItemAble createItem = itemObj.GetComponent<IItemAble>();
                if(createItem != null)
                {
                    createItem.CreateReset(ItemBlockType.BRICK);
                }
            }

            ParticleManager.CreateParticle(particleType, transform.position, 1.5f);
            SFXManager.PlaySound(SFXManager.Instance.Audio_BrickBreak, 1, true);

            if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].gameObject.activeSelf)
            {
                sr.color = new Color(1, 1, 1, 0);
                rb.simulated = false;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else if (blockType == ItemBlockType.BLOCK)
        {
            if (item != null)
            {
                GameObject itemObj = Instantiate(item, GameManager.Instance.prefabContainer);
                itemObj.transform.position = transform.position;
                SFXManager.PlaySound(SFXManager.Instance.Audio_BlockItem, 1, true);

                IItemAble createItem = itemObj.GetComponent<IItemAble>();
                if (createItem != null)
                {
                    createItem.CreateReset(ItemBlockType.BLOCK);
                }
            }

            // 스프라이트 바꿔주기

            SFXManager.PlaySound(SFXManager.Instance.Audio_BlockHit, 1, true);
        }
    }

    public override void Reset()
    {
        if (isRealTrigger) return;

        isTrigger = false;

        if (blockType == ItemBlockType.BRICK)
        {
            sr.color = new Color(1, 1, 1, 1);
            rb.simulated = true;
        }
        else if (blockType == ItemBlockType.BLOCK)
        {
            // 스프라이트 원래대로
        }
    }
}
