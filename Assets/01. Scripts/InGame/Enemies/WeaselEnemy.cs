using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pathfinding;

public class WeaselEnemy : ResetAbleTrap, IItemAble
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;

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
    private bool isAttacking = false;
    private bool isFutureDead = false;

    private bool canAnything = true;
    public float attackDelay = 1f;
    public float attackDist = 50f;
    public float attackHorizonJumpPower = 5f;
    public float attackVerticalJumpPower = 5f;

    [Header("패스파인딩")]
    public Transform target;
    public float chaseDist = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float nextWayPointDist = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModfier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("행동")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    public Path path;
    private int currentWaupoint = 0;
    //bool isGrounded = false;
    Seeker seeker;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
    }

    void Start()
    {
        StartCoroutine(CheckState());
        StartCoroutine(CantGoCheck());
        StartCoroutine(PlaySFX());
        startPosVec = startPosObj.transform.position;
        endPosVec = endPosObj.transform.position;

        target = GameManager.Instance.player.transform;

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void Update()
    {
        startPosObj.transform.position = startPosVec;
        endPosObj.transform.position = endPosVec;
    }

    private void FixedUpdate()
    {
        //isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.4f), 0.3f, 1 << LayerMask.NameToLayer("Ground"));
        isGround = Physics2D.OverlapBox((Vector2)transform.position + new Vector2(0, -0.4f), new Vector2(2f,0.5f), 0, 1 << LayerMask.NameToLayer("Ground"));
        

        if (!isDie)
        {
            if (!canAnything && !isGround)
            {
                isAttacking = true;
            }
            if (isAttacking)
            {
                if (isGround)
                {
                    rb.velocity = Vector2.zero;
                    isAttacking = false;
                    animator.Play("Enemy_AttackDelay");
                    Invoke("AttackDelay", attackDelay);
                }
            }

            if (TargetInAttackDistance())
            {
                AttackReady();
            }
            else if (TargetInChaseDistance() && followEnabled)
            {
                if (isAttacking || !canAnything) return;
                state = EnemyState.Chase;
                animator.Play("Enemy_Walk");
                PathFollow();
            }
            else if(state == EnemyState.Chase)
            {
                state = EnemyState.Idle;
            }
            if (state == EnemyState.Walk && canAnything)
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

    IEnumerator CantGoCheck()
    {
        while (!isDie)
        {
            if (state == EnemyState.Die) yield break; // 코루틴 종료

            float enemyX = transform.position.x;
            yield return new WaitForFixedUpdate();
            if (state == EnemyState.Walk && transform.position.x == enemyX)
            {
                targetX = Random.Range(startPosObj.transform.position.x, endPosObj.transform.position.x);
            }
        }
    }

    IEnumerator PlaySFX()
    {
        while (!isDie)
        {
            if (state == EnemyState.Die) yield break; // 코루틴 종료

            if (Vector2.Distance(transform.position, GameManager.Instance.player.transform.position) <= 10)
            {
                int soundIdx = Random.Range(0, 2);

                if (soundIdx == 0)
                    ObjectManager.PlaySound(ObjectManager.Instance.Audio_Cat_Meow, 1f, true);
                else if (soundIdx == 1)
                    ObjectManager.PlaySound(ObjectManager.Instance.Audio_Cat_Purring, 1f, true);
            }

            float delay = Random.Range(5, 15);
            yield return new WaitForSeconds(delay);
        }
    }

    void Move()
    {
        spriteRenderer.flipX = (targetX < transform.position.x);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), enemySpeed * Time.deltaTime);

        float dist = Mathf.Abs(targetX - transform.position.x);

        if (dist <= 0.1f)
        {
            state = EnemyState.Idle;
            animator.Play("Enemy_Idle");
            return;
        }

        // 앞에 장애물 감지 후 점프 (구멍은 안댐)
        {
            Vector3 dir = (spriteRenderer.flipX) ? -transform.right : transform.right;

            Debug.DrawRay(transform.position, dir * 2f, Color.red, 0.3f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 2f, 1 << LayerMask.NameToLayer("Ground"));
            if (hit && isGround)
            {
                Jump();
            }

            float holeDetectDistance = (spriteRenderer.flipX) ? -1f : 1f;
            Vector2 detectPos = new Vector2(transform.position.x + holeDetectDistance, transform.position.y);
            Debug.DrawRay(detectPos, Vector2.down * 5f, Color.red, 0.3f);

            RaycastHit2D groundHit = Physics2D.Raycast(detectPos, Vector2.down, 5f, 1 << LayerMask.NameToLayer("Ground"));

            if (!groundHit)
            {
                targetX = Random.Range(startPosObj.transform.position.x, endPosObj.transform.position.x);
            }
        }
    }

    void Jump()
    {
        rb.velocity = Vector2.up * jumpSpeed;
    }

    void AttackReady()
    {
        if (state == EnemyState.Die) return;
        spriteRenderer.flipX = (target.position.x - transform.position.x) < 0;
        //Debug.Log("isGround"+isGround);
        //Debug.Log("isAttacking" + isAttacking);
        if (isGround && canAnything)
        {
            Debug.Log("공격!!!!!!!");
            canAnything = false;
            animator.Play("Enemy_Attack");
            Invoke("Attack", 1f);
        }
    }

    void Attack()
    {
        if (state == EnemyState.Die) return;
        //isAttacking = true;
        //rb.velocity = Vector2.up * jumpSpeed;
        rb.AddForce(Vector2.up * attackHorizonJumpPower * 100);
        Vector2 direct = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        rb.AddForce(direct * attackHorizonJumpPower * 100);
        Invoke("AttackCancel", 0.2f);
    }

    void AttackDelay()
    {
        if (state == EnemyState.Die) return;
        canAnything = true;
        animator.Play("Enemy_Idle");
    }

    void AttackCancel()
    {
        if(!isAttacking && !canAnything&& isGround)
        {
            rb.velocity = Vector2.zero;
            isAttacking = false;
            animator.Play("Enemy_AttackDelay");
            Invoke("AttackDelay", attackDelay);
        }
    }

    public override void Reset()
    {
        if (!isFutureDead) return;

        rb.simulated = true;
        isDie = false;
        state = EnemyState.Idle;
        animator.Play("Enemy_Idle");
        StartCoroutine(CheckState());
        StartCoroutine(CantGoCheck());
        StartCoroutine(PlaySFX());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPosObj.transform.position, 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(endPosObj.transform.position, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + new Vector2(0, -0.4f), new Vector2(2f, 0.5f));
    }

    void OnDrawGizmosSelected() // 시야, 공격 사거리
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDist);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (GameManager.Instance.player.transform.position.y > transform.position.y + 0.2f)
            {
                GameManager.Instance.player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 5);

                if (isGround) rb.simulated = false;
                isDie = true;
                state = EnemyState.Die;
                animator.Play("Enemy_Death");
                ObjectManager.PlaySound(ObjectManager.Instance.Audio_BlockItem, 1f, true);
                ObjectManager.PlaySound(ObjectManager.Instance.Audio_Cat_Die, 1f, true);

                if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].gameObject.activeSelf)
                {
                    if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].isAbilityEnable)
                    {
                        isFutureDead = true;
                    }
                }
            }
            else
            {
                GameManager.Instance.player.GameOver();
            }
        }

        if (collision.collider.CompareTag("Ground"))
        {
            if (isDie)
            {
                rb.simulated = false;
            }
        }
    }

    public void CreateReset(ItemBlockType type)
    {
        rb.simulated = false;
        spriteRenderer.color = new Color(1, 1, 1, 0);
        transform.localScale = Vector2.zero;

        if (type == ItemBlockType.BRICK)
        {
            transform.DOMoveY(0.5f, 0.5f).SetRelative();
        }
        else if (type == ItemBlockType.BLOCK)
        {
            transform.DOMoveY(1, 0.5F).SetRelative();
        }

        transform.DOScale(1, 0.5f);
        spriteRenderer.DOFade(1, 0.5f).OnComplete(() =>
        {
            rb.simulated = true;
            float playerY = transform.position.y;
            SetPointVector(new Vector2(transform.position.x - 7.5f, playerY), new Vector2(transform.position.x + 7.5f, playerY));
        });
    }

    private void SetPointVector(Vector2 start, Vector2 end)
    {
        startPosVec = start;
        endPosVec = end;
        startPosObj.transform.position = start;
        endPosObj.transform.position = end;
    }


    // 패스파인딩
    private void UpdatePath()
    {
        if (followEnabled && TargetInChaseDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaupoint >= path.vectorPath.Count)
        {
            return;
        }

        //isGrounded = Physics2D.OverlapBox((Vector2)transform.position + new Vector2(0, -0.4f), new Vector2(2f, 0.5f), 0, 1 << LayerMask.NameToLayer("Ground"));

        Vector2 direction = ((Vector2)path.vectorPath[currentWaupoint] - rb.position).normalized;
        //Vector2 force = direction * enemySpeed * Time.deltaTime;

        if (jumpEnabled && isGround)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                rb.velocity = Vector2.up * jumpSpeed;
            }
        }
        
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x+direction.x, transform.position.y, transform.position.z), enemySpeed * Time.deltaTime);
        //rb.AddForce(force);

        float dist = Vector2.Distance(rb.position, path.vectorPath[currentWaupoint]);
        if (dist < nextWayPointDist)
        {
            currentWaupoint++;
        }

        if (directionLookEnabled)
        {
            if (direction.x != 0f)
                spriteRenderer.flipX = (direction.x < 0);
        }
    }

    private bool TargetInChaseDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < chaseDist;
    }
    private bool TargetInAttackDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < attackDist;
    }
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaupoint = 0;
        }
    }

    public void EnemySpeedChange(float value)
    {
        enemySpeed = value;
    }

    public void EnemyJumpSpeedChange(float value)
    {
        jumpSpeed = value;
    }
}
