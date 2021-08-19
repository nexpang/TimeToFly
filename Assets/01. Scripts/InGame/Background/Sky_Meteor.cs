using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky_Meteor : MonoBehaviour
{
    public Animator[] Hmeteors = null;
    public Animator[] Mmeteors = null;
    public Animator[] Lmeteors = null;

    private void Start()
    {
        for(int i = 0; i < Hmeteors.Length;i++)
        {
            StartCoroutine(MeteorAnimation(3, Hmeteors[i]));
        }

        for (int i = 0; i < Mmeteors.Length; i++)
        {
            StartCoroutine(MeteorAnimation(2, Mmeteors[i]));
        }

        for (int i = 0; i < Lmeteors.Length; i++)
        {
            StartCoroutine(MeteorAnimation(1, Lmeteors[i]));
        }
    }

    IEnumerator MeteorAnimation(int size, Animator animator)
    {
        while (true)
        {
            if (size == 3)
            {
                float randomSec = Random.Range(10, 12);
                yield return new WaitForSeconds(randomSec);
                animator.Play("HugeStar");
            }
            else if (size == 2)
            {
                float randomSec = Random.Range(6, 9);
                yield return new WaitForSeconds(randomSec);
                animator.Play("MiddleStar");
            }
            else
            {
                float randomSec = Random.Range(4, 6);
                yield return new WaitForSeconds(randomSec);
                animator.Play("SmallStar");
            }
        }
    }
}
