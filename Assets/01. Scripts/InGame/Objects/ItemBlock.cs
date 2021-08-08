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
    public BlockParticleType particleType = BlockParticleType.BOX;
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
            GameManager.Instance.player.collisionBlock = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(GameManager.Instance.player.collisionBlock == this)
                GameManager.Instance.player.collisionBlock = null;
        }
    }

    public void JumpDetect()
    {
        if (isTrigger) return;

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

    void BreakEvent()
    {
        pc.GetComponent<Rigidbody2D>().velocity = new Vector2(pc.GetComponent<Rigidbody2D>().velocity.x, pc.GetComponent<Rigidbody2D>().velocity.y * -0.6f);

        if (blockType == ItemBlockType.BRICK)
        {
            if(item != null)
            {
                GameObject itemObj = Instantiate(item, GameManager.Instance.prefabContainer);
                itemObj.transform.position = transform.position;

                if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].gameObject.activeSelf)
                {
                    if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].isAbilityEnable)
                    {
                        ObjectManager.Instance.createdObjectWhileFutureList.Add(itemObj);
                    }
                }

                ObjectManager.PlaySound(ObjectManager.Instance.Audio_BlockItem, 1, true);

                IItemAble createItem = itemObj.GetComponent<IItemAble>();
                if(createItem != null)
                {
                    createItem.CreateReset(ItemBlockType.BRICK);
                }
            }

            ParticleManager.CreateBlockParticle(particleType, transform.position, 1.5f);
            ObjectManager.PlaySound(ObjectManager.Instance.Audio_BrickBreak, 1, true);

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

                if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].gameObject.activeSelf)
                {
                    if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].isAbilityEnable)
                    {
                        ObjectManager.Instance.createdObjectWhileFutureList.Add(itemObj);
                    }
                }

                ObjectManager.PlaySound(ObjectManager.Instance.Audio_BlockItem, 1, true);

                IItemAble createItem = itemObj.GetComponent<IItemAble>();
                if (createItem != null)
                {
                    createItem.CreateReset(ItemBlockType.BLOCK);
                }
            }

            // 스프라이트 바꿔주기
            GetComponent<SpriteRenderer>().sprite = ObjectManager.Instance.Spr_questionBlockTriggered;

            ObjectManager.PlaySound(ObjectManager.Instance.Audio_BlockHit, 1, true);
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
            GetComponent<SpriteRenderer>().sprite = ObjectManager.Instance.Spr_questionBlockDefault;
        }

        for(int i = 0; i< ObjectManager.Instance.createdObjectWhileFutureList.Count;i++)
        {
            Destroy(ObjectManager.Instance.createdObjectWhileFutureList[i]);
            ObjectManager.Instance.createdObjectWhileFutureList.RemoveAt(i);
        }
    }
}
