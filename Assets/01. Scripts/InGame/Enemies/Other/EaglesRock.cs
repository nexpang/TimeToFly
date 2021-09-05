using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaglesRock : MonoBehaviour
{
    private Vector3 chatchingPos;
    public Transform parentTrm;
    private Rigidbody2D rigid;

    public EagleEnemy eagleEnemy;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            ParticleManager.CreateParticle<Effect_StoneFrag>(transform.position);
            OnReset();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("FALLINGABLE"))
        {
            OnReset();
        }
    }

    private void OnReset()
    {
        rigid.simulated = false;
        transform.SetParent(parentTrm);
        transform.localPosition = chatchingPos;
        gameObject.SetActive(false);
        eagleEnemy.RockisReset();
    }

    void Start()
    {
        chatchingPos = Vector3.down;
    }
}
