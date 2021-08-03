using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaglesRock : MonoBehaviour
{
    private Vector3 chatchingPos;
    public Transform parentTrm;
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            rigid.simulated = false;
            transform.SetParent(parentTrm);
            transform.localPosition = chatchingPos;
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        chatchingPos = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
