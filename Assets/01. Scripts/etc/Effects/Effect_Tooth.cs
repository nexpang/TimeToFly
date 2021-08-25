using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Tooth : Effect
{
    private SpriteRenderer sr;
    private Animator an;

    protected override void Awake()
    {
        base.Awake();

        sr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        StartCoroutine(LifeTime());
        an.Play("Tooth");
    }

    protected override IEnumerator LifeTime()
    {
        yield return lifeWait;

        an.Play("Tooth_Wait");
        gameObject.SetActive(false);
        sr.color = Color.white;
    }
}
