using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapType
{
    NongGiGu,
    Stalactite
}

public class TrapObj : MonoBehaviour
{
    public TrapType trapType = TrapType.NongGiGu;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground") || collision.collider.CompareTag("DEADABLE"))
        {
            if (trapType == TrapType.NongGiGu)
            {
                gameObject.tag = "Object";
                gameObject.layer = LayerMask.NameToLayer("Object");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("DEADABLE"))
        {
            if (trapType == TrapType.Stalactite)
            {
                ParticleManager.CreateParticle<Effect_StoneFrag>(transform.position);
                gameObject.SetActive(false);
            }
        }
    }
}
