using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarSpiritEnemy : ResetAbleTrap, IItemAble
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;

    public EnemyState state = EnemyState.Idle;
    public float enemySpeed;

    bool alreadySpawn = false;

    public Transform target;

    public float targetX;

    private bool isDie = false;
    private bool isFutureDead = false;

    public float checkAttackDist = 5f;
    private Vector2 attackDirection = Vector2.zero;
    private bool canAttack = true;
    private bool isAttacking = false;

    public float attackDist = 5f;
    public float attackSpeedForSeconds = 1f;
    public float attackDelay = 1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        target = GameManager.Instance.player.transform;

        //StartCoroutine(CheckState());
        StartCoroutine(PlaySFX());
    }

    private void FixedUpdate()
    {
        if (!isDie)
        {
            if(TargetInAttackDistance() && canAttack)
            {
                StartCoroutine(AttackReady());
            }
            else if (state == EnemyState.Walk)
            {
                //Move();
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
                animator.Play("Enemy_Move");
                //targetX = Random.Range(target.position.x - ( ? 3 : 7), target.position.x + (havingRock ? 3 : 7));
            }

            float delay = Random.Range(2, 3);
            yield return new WaitForSeconds(delay);
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
    IEnumerator Attack()
    {
        if (state == EnemyState.Die) yield break; // 코루틴 종료
        isAttacking = true;
        animator.Play("Enemy_Attack");
        float angel = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        spriteRenderer.flipX = false;
        transform.rotation = Quaternion.AngleAxis(angel, Vector3.forward);
        transform.DOMove((Vector2)transform.position+(attackDirection*attackDist), attackSpeedForSeconds);

        yield return new WaitForSeconds(attackSpeedForSeconds+0.1f);

        isAttacking = false;
        animator.Play("Enemy_Idle");
        transform.rotation = Quaternion.Euler(Vector3.zero);
        spriteRenderer.flipX = attackDirection.x < 0;

        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    IEnumerator AttackReady()
    {
        attackDirection = (target.position - transform.position).normalized;
        spriteRenderer.flipX = attackDirection.x < 0;
        canAttack = false;
        animator.Play("Enemy_AttackReady");
        yield return new WaitForSeconds( 0.4f);
        StartCoroutine(Attack());
    }

    void Move()
    {
        targetX = Random.Range(target.position.x , target.position.x);
        spriteRenderer.flipX = (targetX < transform.position.x);

        float dist = Mathf.Abs(targetX - transform.position.x);

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), dist * enemySpeed * Time.deltaTime);

        if (dist <= 1f)
        {
            state = EnemyState.Idle;

            Attack();
            animator.Play("Enemy_Idle");
            return;
        }
    }
    public override void Reset()
    {
        if (!isFutureDead) return;

        rb.simulated = true;
        isDie = false;
        canAttack = true;
        state = EnemyState.Idle;
        animator.Play("Enemy_Idle");
        StartCoroutine(CheckState());
        StartCoroutine(PlaySFX());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && GameManager.Instance.player.playerState == PlayerState.NORMAL)
        {
            if (GameManager.Instance.player.transform.position.y > transform.position.y + 0.2f && !isAttacking)
            {
                GameManager.Instance.player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 5);

                //if (isGround) rb.simulated = false;
                rb.simulated = false;
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
            //SetPointVector(new Vector2(transform.position.x - 7.5f, playerY), new Vector2(transform.position.x + 7.5f, playerY));
        });
    }

    private bool TargetInAttackDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < checkAttackDist;
    }

    void OnDrawGizmosSelected() // 공격 사거리
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkAttackDist);
    }
}