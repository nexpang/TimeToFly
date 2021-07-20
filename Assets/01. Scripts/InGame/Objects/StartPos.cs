using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    void Start()
    {
        GameManager.Instance.player.transform.position = transform.position;
    }
}
