using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pathfinding;

public class BatEnemy : ResetAbleTrap, IItemAble
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;

    public EnemyState state = EnemyState.Idle;
    public float enemySpeed;
    //public float jumpSpeed;

    //public GameObject startPosObj;
    //public GameObject endPosObj;

    private Vector2 startPosVec;
    private Vector2 endPosVec;

    //public float targetX;

    private bool isDie = false;
    private bool isGround = false;
    private bool isFutureDead = false;

    [Header("패스파인딩")]
    public Transform target;
    public Vector2 waitPosition;
    public float chaseDist = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float nextWayPointDist = 3f;

    [Header("행동")]
    public bool followEnabled = true;
    //public bool jumpEnabled = true;
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
        //StartCoroutine(CheckState());
        //StartCoroutine(CantGoCheck());
        StartCoroutine(PlaySFX());
        //startPosVec = startPosObj.transform.position;
        //endPosVec = endPosObj.transform.position;

        target = GameManager.Instance.player.transform;
        waitPosition = transform.position;

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void Update()
    {
        //startPosObj.transform.position = startPosVec;
        //endPosObj.transform.position = endPosVec;
    }

    private void FixedUpdate()
    {
        //isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.4f), 0.3f, 1 << LayerMask.NameToLayer("Ground"));
        isGround = Physics2D.OverlapBox((Vector2)transform.position + new Vector2(0, -0.4f), new Vector2(1f,0.5f), 0, 1 << LayerMask.NameToLayer("Ground"));
        

        if (!isDie)
        {
            if (TargetInChaseDistance() && followEnabled)
            {
                state = EnemyState.Chase;
                animator.Play("Enemy_Follow");
                PathFollow();
            }
            else if(state == EnemyState.Chase)
            {
                state = EnemyState.Walk;
                seeker.StartPath(rb.position, waitPosition, OnPathComplete);
            }
            else if (state == EnemyState.Walk)
            {
                if (Vector2.Distance(transform.position, waitPosition) < 0.1)
                {
                    state = EnemyState.Idle;
                    animator.Play("Enemy_Idle");
                }
                PathFollow();
            }
            else
            {
                // Idle

            }
        }
    }

    //IEnumerator CheckState()
    //{
    //    while (!isDie)
    //    {
    //        if (state == EnemyState.Die) yield break; // 코루틴 종료

    //        if (state == EnemyState.Idle)
    //        {
    //            state = EnemyState.Walk;
    //            animator.Play("Enemy_Walk");
    //            targetX = Random.Range(startPosObj.transform.position.x, endPosObj.transform.position.x);
    //        }

    //        float delay = Random.Range(1, 3);
    //        yield return new WaitForSeconds(delay);
    //    }
    //}

    //IEnumerator CantGoCheck()
    //{
    //    while (!isDie)
    //    {
    //        if (state == EnemyState.Die) yield break; // 코루틴 종료

    //        float enemyX = transform.position.x;
    //        yield return new WaitForFixedUpdate();
    //        if (state == EnemyState.Walk && transform.position.x == enemyX)
    //        {
    //            targetX = Random.Range(startPosObj.transform.position.x, endPosObj.transform.position.x);
    //        }
    //    }
    //}

    IEnumerator PlaySFX()
    {
        while (!isDie)
        {
            if (state == EnemyState.Die) yield break; // 코루틴 종료

            if (Vector2.Distance(transform.position, GameManager.Instance.player.transform.position) <= 10)
            {
                int soundIdx = Random.Range(0, 2);

                if (soundIdx == 0)
                    ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_Cat_Meow, 1f, true);
                else if (soundIdx == 1)
                    ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_Cat_Purring, 1f, true);
            }

            float delay = Random.Range(5, 15);
            yield return new WaitForSeconds(delay);
        }
    }

    public override void Reset()
    {
        if (!isFutureDead) return;

        rb.simulated = true;
        isDie = false;
        state = EnemyState.Idle;
        animator.Play("Enemy_Idle");
        //StartCoroutine(CheckState());
        //StartCoroutine(CantGoCheck());
        StartCoroutine(PlaySFX());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + new Vector2(0, -0.4f), new Vector2(1f, 0.5f));
    }

    void OnDrawGizmosSelected() // 시야, 공격 사거리
    {
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
                rb.velocity = new Vector2(0, -5f);
                ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_BlockItem, 1f, true);
                ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_Bat_Die, 1f, true);

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
                animator.Play("Enemy_DeathGround");
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
        });
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
        float moveX = 0f;

        if (Mathf.Abs(direction.x) > 0.1f)
            moveX = direction.x < 0 ? -1f : 1f;


        Debug.Log((Vector2)path.vectorPath[currentWaupoint]);
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)path.vectorPath[currentWaupoint], enemySpeed * Time.deltaTime);
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

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaupoint = 0;
        }
    }
}