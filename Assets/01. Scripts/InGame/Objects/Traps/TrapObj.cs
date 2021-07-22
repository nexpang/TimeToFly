using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObj : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground") || collision.collider.CompareTag("DEADABLE"))
        {
            gameObject.tag = "Object";
            gameObject.layer = LayerMask.NameToLayer("Object");
        }
    }
}
