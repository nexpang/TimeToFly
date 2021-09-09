using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapType
{
    NongGiGu,
    Stalactite,
    RepeatTrap
}

public class TrapObj : MonoBehaviour
{
    public TrapType trapType = TrapType.NongGiGu;

    private MoveRepeatTrap moveTrap;
    private bool isAlreadyReset;

    private void Awake()
    {
        if(trapType == TrapType.RepeatTrap)
            moveTrap = transform.parent.GetComponent<MoveRepeatTrap>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground") || collision.collider.CompareTag("DEADABLE"))
        {
            if (trapType == TrapType.NongGiGu)
            {
                gameObject.tag = "Object";
                gameObject.layer = LayerMask.NameToLayer("Object");
            }
            else if(trapType == TrapType.RepeatTrap)
            {
                if (isAlreadyReset)
                    return;
                gameObject.tag = "Object";
                gameObject.layer = LayerMask.NameToLayer("Object");
                StartCoroutine(returnCool(1.5f));
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

    private IEnumerator returnCool(float time)
    {
        isAlreadyReset = true;
        yield return new WaitForSeconds(time);
        print("¸®¼Â");
        moveTrap.Reset();
        isAlreadyReset = false;
    }
}
