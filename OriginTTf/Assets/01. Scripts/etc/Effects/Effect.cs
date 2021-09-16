using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float lifeTime = 1f;

    protected WaitForSeconds lifeWait = null;

    protected virtual void Awake()
    {
        lifeWait = new WaitForSeconds(lifeTime);
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(LifeTime());
    }

    protected virtual IEnumerator LifeTime()
    {
        yield return lifeWait;

        gameObject.SetActive(false);
    }
}
