using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RepeatBtn : MonoBehaviour
{
    private Image image = null;
    [SerializeField] Image repeatBtnImage = null;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        image.sprite = repeatBtnImage.sprite;
    }
}
