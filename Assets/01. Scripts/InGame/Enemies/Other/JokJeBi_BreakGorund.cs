using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokJeBi_BreakGorund : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            ParticleManager.CreateParticle<Effect_StoneFrag>(transform.position );
            gameObject.SetActive(false);
        }
    }
}
