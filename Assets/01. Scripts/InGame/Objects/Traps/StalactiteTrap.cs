using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteTrap : ResetAbleTrap
{
    Rigidbody2D childRb;
    Vector2 originPos;
    Quaternion originRotation;
    Transform child;

    public float gravitySpeed = 3;
    public bool triggerImmediately = false;
    bool isTrigger = false;
    bool isRealTrigger = false;

    private void Awake()
    {
        child = transform.GetChild(0);
        childRb = child.GetComponent<Rigidbody2D>();
    }

    public void Init(float gravitySpeed, bool immediate)
    {
        this.gravitySpeed = gravitySpeed;
        this.triggerImmediately = immediate;

        if (triggerImmediately)
        {
            childRb.gravityScale = gravitySpeed;
            childRb.simulated = true;
            isTrigger = true;

            ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_Falling, 1f, true);
            Destroy(gameObject, 5);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (childRb.simulated || isTrigger) return;

            childRb.gravityScale = gravitySpeed;
            childRb.simulated = true;

            ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_Falling, 1f, true);

            if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].gameObject.activeSelf)
            {
                if (!GameManager.Instance.player.abilitys[(int)Chickens.BROWN].isAbilityEnable)
                {
                    isRealTrigger = true;
                }
            }
        }
    }

    public override void Reset()
    {
        if (isRealTrigger) return;

        child.gameObject.SetActive(true);
        isTrigger = false;
        childRb.simulated = false;
        child.position = originPos;
        child.rotation = originRotation;
    }
}
