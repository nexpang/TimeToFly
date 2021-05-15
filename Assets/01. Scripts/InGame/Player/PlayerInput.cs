using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// �� Ŭ������ ��� Ű �Է��� ����ƽ ������ �����մϴ�. ����, ������ ��ư�� ������ �ٸ� ��ư���� �����մϴ�.

/// <summary>
/// �� Ŭ������ ��� Ű �Է��� ����ƽ ������ �����մϴ�. ����, ������ ��ư�� ������ �ٸ� ��ư���� �����մϴ�.
/// </summary>
public class PlayerInput : MonoBehaviour
{
    public static bool KeyJump = false;
    public static bool KeyAbility = false;
    public static float KeyHorizontalRaw;
    public static float KeyHorizontal;

    private bool joystickKeyJump = false;
    private bool joystickKeyAbility = false;

    private void Update()
    {
        KeyJump = Input.GetButtonDown("Jump");
        KeyAbility = Input.GetKeyDown(KeyCode.K);
        KeyHorizontalRaw = Input.GetAxisRaw("Horizontal");
        KeyHorizontal = Input.GetAxis("Horizontal");

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
