using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeBoomEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem distortion = null;
    [SerializeField] Transform originTransform = null;
    [SerializeField] GameObject boomObj = null;
    [SerializeField] GameObject boomObj2 = null;

    private void OnEnable()
    {
        boomObj = Instantiate(boomObj, transform.position, Quaternion.identity);
        boomObj2 = Instantiate(boomObj2, transform.position, Quaternion.identity);
        transform.SetParent(null);
        transform.DOScale(1.5f, 1).OnComplete(() =>
        {
            transform.DOScaleY(6, 0.5f);
            GetComponent<ParticleSystemRenderer>().material.DOColor(Color.blue,1);
            transform.DOScaleX(0.5f, 0.5f).OnComplete(() =>
            {
                distortion.transform.DOScale(0, 0);
                transform.DOScaleY(0, 0.5f);
                transform.DOScaleX(7f, 0.5f).OnComplete(() =>
                {
                    transform.SetParent(originTransform);
                    Destroy(boomObj);
                    Destroy(boomObj2);
                    gameObject.SetActive(false);
                });
            });
        });
        distortion.transform.DOScale(5, 1).SetEase(Ease.InExpo);
    }

    private void Update()
    {
        boomObj.transform.position = transform.position;
        boomObj2.transform.position = transform.position;
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
        transform.localPosition = Vector3.zero;
        distortion.transform.localScale = Vector3.zero;
        distortion.transform.localPosition = Vector3.zero;
    }
}
