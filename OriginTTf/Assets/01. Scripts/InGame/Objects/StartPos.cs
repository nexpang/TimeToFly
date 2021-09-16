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
        if(!SceneController.isSavePointChecked) StartCoroutine(StartPosMove());
    }

    IEnumerator StartPosMove()
    {
        yield return null;
        GameManager.Instance.player.transform.position = transform.position;
    }
}
