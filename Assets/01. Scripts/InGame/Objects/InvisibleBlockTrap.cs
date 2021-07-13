using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InvisibleBlockType
{
    INVISIBLE,
    TRIGGER,
    TRIGGERIFUP
}

public class InvisibleBlockTrap : ResetAbleTrap
{
    [SerializeField] InvisibleBlockType type = InvisibleBlockType.INVISIBLE;

    PlayerController pc;
    SpriteRenderer sr;
    BoxCollider2D bc;
    Vector2 offset; // TRIGGERIFUP 상태일때만 필요함.
    Vector2 bcSize; // TRIGGERIFUP 상태일때만 필요함.

    float alpha;
    bool isTrigger = false;
    bool isRealTrigger = false;

    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        alpha = sr.color.a;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);

        if(type == InvisibleBlockType.TRIGGERIFUP)
        {
            bcSize = bc.size;
            offset = bc.offset;
            bc.isTrigger = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (type != InvisibleBlockType.INVISIBLE)
        {
            if (collision.collider.CompareTag("Player"))
            {
                if (isTrigger) return;
                isTrigger = true;

                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isTrigger) return;

            if (pc.GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                bc.size = Vector2.one;
                bc.offset = Vector2.zero;
                isTrigger = true;
                bc.isTrigger = false;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

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

    public override void Reset()
    {
        if (isRealTrigger) return;

        isTrigger = false;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        if (type == InvisibleBlockType.TRIGGERIFUP)
        {
            bc.size = bcSize;
            bc.offset = offset;
        }
    }
}
