using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRepeatTrap : ResetAbleTrap
{
    [SerializeField] Vector2 dir;
    [SerializeField] float speed;
    [SerializeField] Space space = Space.World;

    [Header("이때 리짓바디 컴포넌트가 필요하다.")]
    [Header("만약 떨어뜨릴꺼라면, 리짓바디를 켜준다.")]
    [SerializeField] bool isRigidBody;

    Vector2 originPos;
    Quaternion originRotation;

    Transform child;
    Rigidbody2D childRb;
    bool isTrigger = false;
    bool isRealTrigger = false;

    private void Start()
    {
        child = transform.GetChild(0);
        childRb = child.GetComponent<Rigidbody2D>();
        originPos = child.position;
        originRotation = child.rotation;
    }

    void Update()
    {
        if (isTrigger)
        {
            child.transform.Translate(dir * speed * Time.deltaTime, space);
        }
        if (Mathf.Abs(GameManager.Instance.player.transform.position.x) - Mathf.Abs(transform.position.x) < 8)
            PlayTrap();
    }

    void PlayTrap()
    {
        if (childRb.simulated || isTrigger) return;


        if (isRigidBody)
        {
            childRb.gravityScale = speed;
            childRb.simulated = true;
        }
        else
        {
            isTrigger = true;
        }

        ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_Falling, 1f, true);
    }

    public override void Reset()
    {
        if (isRealTrigger) return;

        isTrigger = false;
        childRb.velocity = Vector2.zero;
        childRb.angularVelocity = 0;
        childRb.simulated = false;
        child.position = originPos;
        child.rotation = originRotation;
        child.tag = "DEADABLE";
        child.gameObject.layer = LayerMask.NameToLayer("DEADABLE");
    }
}
