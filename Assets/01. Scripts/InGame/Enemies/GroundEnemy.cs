using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Walk,
    Chase,
    Die
}

public class GroundEnemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rigidbody;

    public EnemyState state = EnemyState.Idle;
    public float enemySpeed;
    public float jumpSpeed;

    public GameObject startPosObj;
    public GameObject endPosObj;

    private Vector2 startPosVec;
    private Vector2 endPosVec;

    public float targetX;

    private bool isDie = false;
    private bool isGround = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartCoroutine(CheckState());
        startPosVec = startPosObj.transform.position;
        endPosVec = endPosObj.transform.position;
    }

    private void Update()
    {
        startPosObj.transform.position = startPosVec;
        endPosObj.transform.position = endPosVec;
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.4f), 0.3f, 1 << LayerMask.NameToLayer("Ground"));

        if (!isDie)
        {
            if (state == EnemyState.Walk)
            {
                Move();
            }
            else
            {
                // Idle
            }
        }
    }

    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == EnemyState.Die) yield break; // 코루틴 종료

            if (state == EnemyState.Idle)
            {
                state = EnemyState.Walk;
                animator.Play("Enemy_Walk");
                targetX = Random.Range(startPosObj.transform.position.x, endPosObj.transform.position.x);
            }

            float delay = Random.Range(1, 3);
            yield return new WaitForSeconds(delay);
        }
    }

    void Move()
    {
        spriteRenderer.flipX = (targetX < transform.position.x);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), enemySpeed * Time.deltaTime);

        float dist = Mathf.Abs(targetX - transform.position.x);

        if (dist <= 0.25f)
        {
            state = EnemyState.Idle;
            animator.Play("Enemy_Idle");
        }

        // 앞에 장애물 감지 후 점프 (구멍은 안댐)
        {
            Vector3 dir = (spriteRenderer.flipX) ? -transform.right : transform.right;

            Debug.DrawRay(transform.position, dir * 2, Color.red, 0.3f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 2f, 1 << LayerMask.NameToLayer("Ground"));

            if (hit && isGround)
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        rigidbody.AddForce(Vector2.up * jumpSpeed);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPosObj.transform.position, 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(endPosObj.transform.position, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            if(GameManager.Instance.player.isGround)
            {
                GameManager.Instance.player.GameOver();
            }
            else
            {
                GameManager.Instance.player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 5);
                rigidbody.simulated = false;
                isDie = true;
                animator.Play("Enemy_Death");
            }
        }
    }
}
