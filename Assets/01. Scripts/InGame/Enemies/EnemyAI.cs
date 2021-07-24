using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("패스파인딩")]
    public Transform target;
    public float activateDist = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWayPointDist = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModfier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("행동")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private SpriteRenderer spriteRenderer;
    public Path path;
    private int currentWaupoint = 0;
    bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void Start()
    {
        target = GameManager.Instance.player.transform;
    }

    private void FixedUpdate()
    {
        if(TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if(followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if(path == null)
        {
            return;
        }

        if(currentWaupoint >= path.vectorPath.Count)
        {
            return;
        }

        isGrounded = Physics2D.OverlapBox((Vector2)transform.position + new Vector2(0, -0.4f), new Vector2(2f, 0.5f), 0, 1 << LayerMask.NameToLayer("Ground"));

        Vector2 direction = ((Vector2)path.vectorPath[currentWaupoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if(jumpEnabled && isGrounded)
        {
            if(direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * speed * jumpModfier);
            }
        }

        rb.AddForce(force);

        float dist = Vector2.Distance(rb.position, path.vectorPath[currentWaupoint]);
        if(dist < nextWayPointDist)
        {
            currentWaupoint++;
        }

        if(directionLookEnabled)
        {
            if(Mathf.Abs(rb.velocity.x) > 0.05f)
            {
                spriteRenderer.flipX = (rb.velocity.x < 0f);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDist;
    }

    private void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaupoint = 0;
        }
    }

    void OnDrawGizmosSelected() // 시야, 공격 사거리
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activateDist);
    }
}
