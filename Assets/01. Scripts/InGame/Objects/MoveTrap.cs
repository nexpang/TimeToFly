using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrap : ResetAbleTrap
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

    private void Awake()
    {
        child = transform.GetChild(0);
        childRb = child.GetComponent<Rigidbody2D>();
        originPos = child.position;
        originRotation = child.rotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (isRigidBody)
            {
                childRb.simulated = true;
            }
            else
            {
                isTrigger = true;
            }


            if (PlayerController.Instance.ability1 != null)
            {
                if (!PlayerController.Instance.ability1.IsSleep())
                {
                    isRealTrigger = true;
                }
            }
        }
    }

    void Update()
    {
        if(isTrigger)
        {
            child.transform.Translate(dir * speed * Time.deltaTime, space);
        }
    }

    public override void Reset()
    {
        if (isRealTrigger) return;

        isTrigger = false;
        childRb.simulated = false;
        child.position = originPos;
        child.rotation = originRotation;
        child.tag = "DEADABLE";
    }
}
