using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour
{
    [SerializeField] private Vector3 rotate;
    private Vector3 rotateCurrent;

    private void Start()
    {
        rotateCurrent = rotate;
    }

    void Update()
    {
        if (PlayerController.Instance.ability1.enabled)
        {
            rotateCurrent = PlayerController.Instance.ability1.IsSleep() ? rotate * 30 : rotate;
        }
        else if(PlayerController.Instance.ability2.enabled)
        {
            rotateCurrent = PlayerController.Instance.ability2.IsTimeFast ? rotate * 0 : rotate;
        }

        transform.Rotate(rotateCurrent * Time.deltaTime);    
    }
}
