using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMove : MonoBehaviour
{
    private void Update()
    {
       this.gameObject.transform.position = Vector2.Lerp(this.gameObject.transform.position, new Vector2(64, 35), 1f * Time.deltaTime);
    }
}
