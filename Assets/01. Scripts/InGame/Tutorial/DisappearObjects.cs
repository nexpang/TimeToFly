using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DisappearObjects : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Image Fence;
    [SerializeField] Image Barn1;
    [SerializeField] Image Barn2;
    [SerializeField] Transform[] objTrans = null;
    bool isTrigger = false;
    [SerializeField] float distance = 1;

    void Start()
    {
        Barn1.material.color = Color.white;
        Barn2.material.color = Color.white;
        Fence.color = Color.white;
    }

    void Update()
    {
        foreach( Transform item in objTrans)
        {
            float playerDistance = Mathf.Abs(player.transform.position.x - item.transform.position.x);
            if (playerDistance < distance)
            {
                isTrigger = true;
            }
        }

        if(isTrigger)
        {
            if (Fence.color == new Color(1, 1, 1, 1f)) // 닷트윈 실행중인지 확인하는거 bool로 받아오는게 있을텐데 뭘까 아무튼 알파가 1일때만 작동
            {
                Fence.DOFade(0.1f, 0.75f);
                Barn1.material.DOFade(0.1f, 0.75f);
                Barn2.material.DOFade(0.1f, 0.75f);
            }
        }
        else
        {
            if (Fence.color == new Color(1, 1, 1, 0.1f))
            {
                Fence.DOFade(1f, 0.75f);
                Barn1.material.DOFade(1f, 0.75f);
                Barn2.material.DOFade(1f, 0.75f);
            }
        }

        isTrigger = false;
    }
}
