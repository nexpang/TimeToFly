using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEagleRock : MonoBehaviour
{
    private bool isground = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DistroyThis());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            isground = true;

            print("³ª ´êÀ½");
        }
    }

    IEnumerator DistroyThis()
    {
        while(!isground)
        {
            yield return new WaitForSeconds(1f);
        }
        isground = false;
        gameObject.SetActive(false);
    }
}
