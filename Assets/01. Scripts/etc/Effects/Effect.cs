using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float lifeTime = 1f;

    protected WaitForSeconds lifeWait = null;

    protected void Awake()
    {
        lifeWait = new WaitForSeconds(lifeTime);
    }

    protected void OnEnable()
    {
        StartCoroutine(LifeTime());
    }

    protected IEnumerator LifeTime()
    {
        yield return lifeWait;

        gameObject.SetActive(false);
    }
}
