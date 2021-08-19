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
        if(GameManager.Instance != null)
        {
            if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].gameObject.activeSelf)
            {
                rotateCurrent = GameManager.Instance.player.abilitys[(int)Chickens.BROWN].isAbilityEnable ? rotate * -30 : rotate;
            }
            else if (GameManager.Instance.player.abilitys[(int)Chickens.BLUE].gameObject.activeSelf)
            {
                rotateCurrent = GameManager.Instance.player.abilitys[(int)Chickens.BLUE].isAbilityEnable ? rotate * 100 : rotate;
            }
        }

        

        transform.Rotate(rotateCurrent * Time.deltaTime);    
    }
}
