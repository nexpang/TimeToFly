using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherParticleSet : MonoBehaviour
{
    [SerializeField]
    private GameObject clone1;
    [SerializeField]
    private List<Texture> featerSprites;
    [SerializeField]
    private bool canPlayEffcet = false;

    private void Start()
    {
        gameObject.GetComponent<ParticleSystemRenderer>().material.mainTexture =
            featerSprites[clone1.GetComponent<PlayerSprites>().targetSheet];
    }

    private void Update()
    {
        if (canPlayEffcet)
        {
            gameObject.GetComponent<ParticleSystem>().Play();
            canPlayEffcet = false;
        }
        else
            return;
    }
}
