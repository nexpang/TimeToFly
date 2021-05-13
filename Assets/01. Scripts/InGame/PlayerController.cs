using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;

    public bool isGround;
    public bool isStun;

    private Rigidbody2D rb;
    private Animator an;
    private SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        PlayerMove(Movetype.JUMP);
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.4f), 0.3f, 1 << LayerMask.NameToLayer("Ground"));

        AnimParametersSet();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(0, -0.4f), 0.3f);
    }

    void FixedUpdate()
    {
        PlayerMove(Movetype.MOVE);
    }

    void PlayerMove(Movetype type)
    {
        if (isStun) return;

        if(type == Movetype.JUMP)
        {
            // 점프
            if (Input.GetButtonDown("Jump") && isGround)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpSpeed);
                an.SetTrigger("jumpT");
            }
        }
        else if(type == Movetype.MOVE)
        {
            // 이동
            float axis = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime;
            transform.Translate(new Vector2(axis, 0));

            if (axis != 0)
            {
                an.SetBool("walk", true);
                sr.flipX = (axis < 0);
            }
            else an.SetBool("walk", false);
        }
    }

    void AnimParametersSet()
    {
        an.SetBool("jump", !isGround);
        an.SetFloat("ySpeed", rb.velocity.y);
        if (rb.velocity.y < -4f) an.SetBool("falling", true);
        if (rb.velocity.y < -15f) an.SetBool("land", true);
    }
}


enum Movetype
{
    MOVE,
    JUMP
};