using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float width = lineRenderer.startWidth;
        lineRenderer.material.mainTextureScale = new Vector2(1f / width, 1.0f);
    }
}
