using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 이 클래스는 모든 키 입력의 변수를 선언합니다. 왼쪽, 오른쪽 버튼을 제외한 다른 버튼들을 구현합니다.

/// <summary>
/// 이 클래스는 모든 키 입력의 변수를 선언합니다. 왼쪽, 오른쪽 버튼을 제외한 다른 버튼들을 구현합니다.
/// </summary>
public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance;

    public bool KeyJump = false;
    public bool KeyAbility = false;
    public float KeyHorizontalRaw;
    public float KeyHorizontal;

    private bool joystickKeyJump = false;
    private bool joystickKeyAbility = false;

    public float joystickKeyHorizontal = 0;
    [SerializeField]  private float joystickKeyHorizontalRaw = 0;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        KeyJump = Input.GetButtonDown("Jump");
        KeyAbility = Input.GetKeyDown(KeyCode.K);
        KeyHorizontal = Input.GetAxis("Horizontal");
        KeyHorizontalRaw = Input.GetAxisRaw("Horizontal");

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

        if(joystickKeyHorizontal != 0)
        {
            KeyHorizontal = joystickKeyHorizontal;
        }

        if (joystickKeyHorizontalRaw != 0)
        {
            KeyHorizontalRaw = joystickKeyHorizontalRaw;
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

    public void JoyStickHorizontalRaw(float horizontalRaw)
    {
        joystickKeyHorizontalRaw = horizontalRaw;
    }
}
