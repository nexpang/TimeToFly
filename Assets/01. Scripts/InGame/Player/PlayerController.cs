using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Movetype
{
    MOVE,
    JUMP
};

public enum PlayerState
{
    NORMAL,
    DEAD
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;

    public bool isGround;
    public bool isStun;

    public PlayerState playerState = PlayerState.NORMAL;

    private Rigidbody2D rb;
    private Animator an;
    private SpriteRenderer sr;
    public Ability_FutureCreate ability1;

    [SerializeField] private Transform playerAnim = null;
    private Transform playerAbility = null;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        an = playerAnim.GetComponent<Animator>();
        sr = playerAnim.GetComponent<SpriteRenderer>();
        playerAbility = GameObject.FindGameObjectWithTag("Ability").transform;
        playerState = PlayerState.NORMAL;
    }

    private void Start()
    {
        ability1 = FindObjectOfType<Ability_FutureCreate>();
    }

    private void Update()
    {
        PlayerMove(Movetype.JUMP);
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.4f), 0.3f, 1 << LayerMask.NameToLayer("Ground"));

        AnimParametersSet();
        AbilityKey();
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
            if (PlayerInput.Instance.KeyJump && isGround)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpSpeed);
                an.SetTrigger("jumpT");
            }
        }
        else if(type == Movetype.MOVE)
        {
            // 이동
            float axis = PlayerInput.Instance.KeyHorizontalRaw * moveSpeed * Time.fixedDeltaTime;
            float simpleAxis = Mathf.Round(axis * 1000) / 1000;
            transform.Translate(new Vector2(simpleAxis, 0));

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

        an.SetBool("isDead", playerState == PlayerState.DEAD);
    }

    void AbilityKey()
    {
        if (playerState == PlayerState.DEAD) return;

        if(PlayerInput.Instance.KeyAbility)
        {
            IAbility ability = playerAbility.GetComponent<IAbility>();

            ability.OnAbility();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(0, -0.4f), 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("DEADABLE"))
        {
            GameOver();
        }
        else if(collision.CompareTag("FALLINGABLE"))
        {
            FallGameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("DEADABLE"))
        {
            GameOver();
        }
    }

    [ContextMenu("게임오버")]
    public void GameOver()
    {
        if (playerState != PlayerState.DEAD)
        {
            playerState = PlayerState.DEAD;
            rb.simulated = false;
            an.SetTrigger("dead");
        }
    }

    [ContextMenu("떨어져서 게임오버")]
    public void FallGameOver()
    {
        if (playerState != PlayerState.DEAD)
        {
            playerState = PlayerState.DEAD;
            rb.simulated = false;
            an.SetTrigger("falldead");
        }
    }
}