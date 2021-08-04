using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimerMinusEffect : MonoBehaviour
{
    private Text text;
    public float radius = 0.5f;
    public RectTransform parentRectTrm;

    private void Awake()
    {
        text = GetComponent<Text>();
    }
    public IEnumerator OnEffect(int usingTime)
    {
        text.text = "-"+usingTime;
        transform.position = transform.position + Vector3.up;
        //transform.localPosition = new Vector3(-parentRectTrm.rect.width/2, -parentRectTrm.rect.width / 2, 0f)+(new Vector3(Random.Range(-1,1), Random.Range(-1, 1), 0).normalized * 50);
        transform.DOMoveY(transform.position.y - 1, 1f);
        text.DOColor(new Color(1f, 0f, 0f, 1f), 0.3f);
        yield return new WaitForSeconds(0.6f);
        text.DOColor(new Color(1f, 0f, 0f, 0f), 0.8f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
