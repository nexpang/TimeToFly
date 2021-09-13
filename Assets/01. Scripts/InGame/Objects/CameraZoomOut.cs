using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    public float zoomValue = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.zoomOuts.Add(this);
            GameManager.Instance.CameraZoomSet();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.zoomOuts.Remove(this);
            GameManager.Instance.CameraZoomSet();
        }
    }
}