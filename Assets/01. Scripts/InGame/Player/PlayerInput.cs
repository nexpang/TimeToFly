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
    public bool KeyAbilityHold = false;
    public bool KeyInteraction = false;
    public float KeyHorizontalRaw;
    public float KeyHorizontal;

    private bool joystickKeyJump = false;
    private bool joystickKeyAbility = false;
    private bool joystickKeyInteraction = false;
    public bool joystickKeyAbilityHold = false;
    public bool joystickKeyAbilityOn = false;

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
        KeyAbilityHold = Input.GetKey(KeyCode.K);
        KeyHorizontal = Input.GetAxis("Horizontal");
        KeyHorizontalRaw = Input.GetAxisRaw("Horizontal");
        KeyInteraction = Input.GetKeyDown(KeyCode.L);

        if (joystickKeyJump)
        {
            joystickKeyJump = false;
            KeyJump = true;
        }
        if (joystickKeyAbility)
        {
            joystickKeyAbility = false;
            KeyAbility = true;
            KeyAbilityHold = true;
        }

        if(joystickKeyInteraction)
        {
            joystickKeyInteraction = false;
            KeyInteraction = true;
        }

        if(joystickKeyAbilityHold)
        {
            KeyAbilityHold = true;
        }

        if (joystickKeyAbilityOn)
        {
            joystickKeyAbilityHold = false;
            joystickKeyAbilityOn = false;
            KeyAbilityHold = false;
        }

        if(joystickKeyHorizontal != 0)
        {
            KeyHorizontal = joystickKeyHorizontal;
        }

        if (joystickKeyHorizontalRaw != 0)
        {
            KeyHorizontalRaw = joystickKeyHorizontalRaw;
        }

        if(GameManager.Instance.player.reverseKey)
        {
            KeyHorizontalRaw *= -1;
            KeyHorizontal *= -1;
        }
    }

    public void JoyStickJump()
    {
        joystickKeyJump = true;
    }

    public void JoyStickAbility()
    {
        joystickKeyAbility = true;
        joystickKeyAbilityHold = true;
    }

    public void JoyStickInteration()
    {
        joystickKeyInteraction = true;
    }

    public void JoyStickAbilityHold()
    {
        joystickKeyAbilityHold = true;
    }
    public void JoyStickAbilityOn()
    {
        joystickKeyAbilityOn = true;
    }

    public void JoyStickHorizontalRaw(float horizontalRaw)
    {
        joystickKeyHorizontalRaw = horizontalRaw;
    }
}
