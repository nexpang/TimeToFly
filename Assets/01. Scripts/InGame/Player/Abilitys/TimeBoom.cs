using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBoom : MonoBehaviour
{
    [SerializeField] GameObject ability = null;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.SetParent(ability.transform);
        Destroy(GetComponent<Rigidbody2D>());
        GetComponent<CircleCollider2D>().enabled = false;
        gameObject.SetActive(false);
        Debug.Log("Æø¹ß");
    }

    void Boom()
    {

    }
}
