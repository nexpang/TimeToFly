using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEagleRock : MonoBehaviour
{

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            Invoke("ReturnCool", 3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FALLINGABLE"))
        {
            gameObject.SetActive(false);
        }
    }

    private void ReturnCool()
    {
        ParticleManager.CreateParticle<Effect_StoneFrag>(transform.position);
        gameObject.SetActive(false);
    }
}
