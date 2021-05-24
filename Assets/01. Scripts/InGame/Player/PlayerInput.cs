using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// �� Ŭ������ ��� Ű �Է��� ������ �����մϴ�. ����, ������ ��ư�� ������ �ٸ� ��ư���� �����մϴ�.

/// <summary>
/// �� Ŭ������ ��� Ű �Է��� ������ �����մϴ�. ����, ������ ��ư�� ������ �ٸ� ��ư���� �����մϴ�.
/// </summary>
public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance;

    public bool KeyJump = false;
    public bool KeyAbility = false;
    public bool KeyAbilityHold = false;
    public float KeyHorizontalRaw;
    public float KeyHorizontal;

    private bool joystickKeyJump = false;
    private bool joystickKeyAbility = false;
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

        if(joystickKeyJump)
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
