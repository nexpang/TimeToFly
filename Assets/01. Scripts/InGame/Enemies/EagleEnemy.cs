using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EagleEnemy : ResetAbleTrap, IItemAble
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;

    public EnemyState state = EnemyState.Idle;
    public float enemySpeed;

    public Transform target;

    public float targetX;

    private bool isDie = false;
    private bool isFutureDead = false;
    private bool havingRock = false;
    private bool firstRock = true;

    public GameObject rockObj = null;

    public float reloadRockDelay = 1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        target = GameManager.Instance.player.transform;

        StartCoroutine(CheckState());
        StartCoroutine(ReroadRockDelay());
    }

    private void FixedUpdate()
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
                if (havingRock)
                    animator.Play("Enemy_Move_R");
                else
                    animator.Play("Enemy_Move");
                targetX = Random.Range(target.position.x- (havingRock ? 3 : 7), target.position.x + (havingRock ? 3 : 7));
            }

            float delay = Random.Range(2, 3);
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator ReroadRockDelay()
    {
        while(!isDie)
        {
            if (state == EnemyState.Die) yield break;

            float delay = firstRock ? 5f : Random.Range(10f, 12f);
            if (firstRock)
                firstRock = false;
            yield return new WaitForSeconds(delay);
            if (!havingRock)
                StartCoroutine(AttackReload());
        }
    }

    void Attack()
    {
        if (state == EnemyState.Die) return; // 코루틴 종료
        rockObj.transform.parent = null;
        rockObj.GetComponent<Rigidbody2D>().simulated = true;
    }

    public void RockisReset()
    {
        havingRock = false;
    }

    IEnumerator AttackReload()
    {
        if (state == EnemyState.Die) yield break; // 코루틴 종료

        state = EnemyState.Chase;
        GetComponent<BoxCollider2D>().enabled = false;
        animator.Play("Enemy_Down");
        ObjectManager.PlaySound(ObjectManager.Instance.Audio_Eagle_Crying, 1f, true);
        transform.DOMove(transform.position + Vector3.down * 7, reloadRockDelay / 2f);
        yield return new WaitForSeconds(reloadRockDelay / 2f+0.1f);
        havingRock = true;
        rockObj.SetActive(true);
        animator.Play("Enemy_Idle_R");
        transform.DOMove(transform.position + Vector3.up * 7, reloadRockDelay / 2f);
        yield return new WaitForSeconds(reloadRockDelay / 2f+0.1f);

        if (state == EnemyState.Die) yield break; // 코루틴 종료

        GetComponent<BoxCollider2D>().enabled = true;
        state = EnemyState.Idle;
    }

    void Move()
    {
        if(transform.position.x<target.position.x-7 || transform.position.x > target.position.x + 7)
            targetX = Random.Range(target.position.x - (havingRock? 3 : 7), target.position.x + (havingRock ? 3 : 7));
        spriteRenderer.flipX = (targetX < transform.position.x);

        float dist = Mathf.Abs(targetX - transform.position.x);

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), (dist > 2f ? dist : 2f) * enemySpeed * Time.deltaTime);

        if (dist <= 0.1f)
        {
            state = EnemyState.Idle;

            if (havingRock)
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
        state = EnemyState.Idle;
        if (havingRock)
            animator.Play("Enemy_Idle_R");
        else
            animator.Play("Enemy_Idle");
        StartCoroutine(CheckState());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && GameManager.Instance.player.playerState == PlayerState.NORMAL)
        {
            if (GameManager.Instance.player.transform.position.y > transform.position.y + 0.2f)
            {
                GameManager.Instance.player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 5);

                //if (isGround) rb.simulated = false;
                rb.simulated = false;
                isDie = true;
                state = EnemyState.Die;
                animator.Play("Enemy_Death");
                ParticleManager.CreateParticle<Effect_EagleFeather>(transform.position);
                ObjectManager.PlaySound(ObjectManager.Instance.Audio_BlockItem, 1f, true);
                //ObjectManager.PlaySound(ObjectManager.Instance.Audio_Eagle_Die, 1f, true);

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
}