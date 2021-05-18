using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour
{
    [SerializeField] private Vector3 rotate;
    private Vector3 rotateCurrent;

    private Ability_FutureCreate ability1 = null;
    private Ability_TimeFaster ability2 = null;

    private void Start()
    {
        rotateCurrent = rotate;
        ability1 = FindObjectOfType<Ability_FutureCreate>();
        ability2 = FindObjectOfType<Ability_TimeFaster>();
    }

    void Update()
    {
        if (ability1 != null)
        {
            rotateCurrent = ability1.IsSleep() ? rotate * 30 : rotate;
        }
        else if(ability2 != null)
        {
            rotateCurrent = ability2.IsTimeFast ? rotate * 0 : rotate;
        }

        transform.Rotate(rotateCurrent * Time.deltaTime);    
    }
}
