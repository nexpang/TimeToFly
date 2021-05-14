using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 이 클래스는 왼쪽, 오른쪽 버튼을 제외한 다른 버튼들을 구현합니다.

/// <summary>
/// 이 클래스는 왼쪽, 오른쪽 버튼을 제외한 다른 버튼들을 구현합니다.
/// </summary>
public class PlayerInput : MonoBehaviour
{
    public static bool KeyJump = false;
    public static bool KeyAbility = false;

    private bool joystickKeyJump = false;
    private bool joystickKeyAbility = false;

    private void Update()
    {
        KeyJump = Input.GetButtonDown("Jump");
        KeyAbility = Input.GetKeyDown(KeyCode.K);

        if(joystickKeyJump)
        {
            joystickKeyJump = false;
            KeyJump = true;
        }

        if (joystickKeyAbility)
        {
            joystickKeyAbility = false;
            KeyAbility = true;
        }
    }

    public void JoyStickJump()
    {
        joystickKeyJump = true;
    }

    public void JoyStickAbility()
    {
        joystickKeyAbility = true;
    }
}
