using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour
{
    [SerializeField] private Vector3 rotate;
    private Vector3 rotateCurrent;

    private Ability_FutureCreate ability1 = null;

    private void Start()
    {
        rotateCurrent = rotate;
        ability1 = FindObjectOfType<Ability_FutureCreate>();
    }

    void Update()
    {
        if (ability1 != null)
        {
            rotateCurrent = ability1.IsSleep() ? rotate * 30 : rotate;
        }

        transform.Rotate(rotateCurrent * Time.deltaTime);    
    }
}
