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

    public EnemyState state = EnemyState.Idle;
    public float enemySpeed;

    public GameObject startPos;
    public GameObject endPos;

    public float targetX;

    private bool isDie = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(CheckState());
    }

    private void Update()
    {
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
                targetX = Random.Range(startPos.transform.position.x, endPos.transform.position.x);
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
        }

        // 앞에 장애물 감지 후 점프 (구멍은 안댐)
        {
            Vector3 dir = (spriteRenderer.flipX) ? -transform.right : transform.right;

            Debug.DrawRay(transform.position, dir * 2, Color.red, 0.3f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 2f, 1 << LayerMask.NameToLayer("Ground"));

            if (hit)
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        Debug.Log("점프");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPos.transform.position, 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(endPos.transform.position, 0.5f);
    }
}
