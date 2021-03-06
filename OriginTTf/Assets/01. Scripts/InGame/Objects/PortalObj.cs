using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalObj : MonoBehaviour
{
    public PortalObj targetPotal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance.player.isTeleportAble)
            {
                GameManager.Instance.player.PlayerActCoolTimeSet(2, () =>
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    GameManager.Instance.player.transform.position = targetPotal.transform.position;

                    ParticleManager.CreateParticle<Effect_PortalDirt>(transform.position);
                    ParticleManager.CreateParticle<Effect_PortalDirt>(targetPotal.transform.position);
                    ParticleManager.CreateParticle<Effect_PortalMoving>(targetPotal.transform.position);
                    GameManager.Instance.player.isTeleportAble = false;
                    GetComponent<BoxCollider2D>().enabled = true;
                });
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.player.isTeleportAble = true;
            GameManager.Instance.player.PlayerActCoolTimeStop();
        }
    }
}
