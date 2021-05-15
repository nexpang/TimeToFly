using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMove : MonoBehaviour
{
    private void Update()
    {
       gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, new Vector2(64, 35), 1f * Time.deltaTime);
    }
}
