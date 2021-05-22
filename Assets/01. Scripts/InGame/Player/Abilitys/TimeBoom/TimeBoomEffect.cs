using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeBoomEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem distortion = null;
    [SerializeField] Transform originTransform = null;

    private void OnEnable()
    {
        transform.SetParent(null);
        transform.DOScale(1.5f, 1).OnComplete(() =>
        {
            transform.DOScaleY(6, 0.5f);
            transform.DOScaleX(0.5f, 0.5f).OnComplete(() =>
            {
                distortion.transform.DOScale(0, 0);
                transform.DOScaleY(0, 0.5f);
                transform.DOScaleX(7f, 0.5f).OnComplete(() =>
                {
                    transform.SetParent(originTransform);
                    gameObject.SetActive(false);
                });
            });
        });
        distortion.transform.DOScale(5, 1).SetEase(Ease.InExpo);
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
        transform.localPosition = Vector3.zero;
        distortion.transform.localScale = Vector3.zero;
        distortion.transform.localPosition = Vector3.zero;
    }
}
