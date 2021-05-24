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

    GameObject boom;
    GameObject boom2;

    private void OnEnable()
    {
        boom = Instantiate(boomObj, transform.position, Quaternion.identity);
        boom2 = Instantiate(boomObj2, transform.position, Quaternion.identity);
        GetComponent<ParticleSystemRenderer>().material.color = Color.cyan;
        transform.SetParent(null);
        transform.DOScale(1.5f, 2).OnComplete(() =>
        {
            transform.DOScaleY(6, 1f);
            GetComponent<ParticleSystemRenderer>().material.DOColor(new Color(0.3f, 0.3f, 1), 4);
            transform.DOScaleX(0.5f, 1f).OnComplete(() =>
            {
                distortion.transform.DOScale(new Vector3(7, 0, 0), 1);
                transform.DOScaleY(0, 1f);
                transform.DOScaleX(7f, 1f).OnComplete(() =>
                {
                    transform.SetParent(originTransform);
                    Destroy(boom);
                    Destroy(boom2);
                    gameObject.SetActive(false);
                });
            });
        });
        distortion.transform.DOScale(5, 1).SetEase(Ease.InExpo);
    }

    private void Update()
    {
        boom.transform.position = transform.position;
        boom2.transform.position = transform.position;
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
        transform.localPosition = Vector3.zero;
        distortion.transform.localScale = Vector3.zero;
        distortion.transform.localPosition = Vector3.zero;
    }
}
