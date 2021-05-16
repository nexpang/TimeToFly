using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ControllerType
{
    LEFT,
    RIGHT,
    JUMP,
    ABILITY
}

public class MobileController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] ControllerType controllerType;
    private Image controllerBtn = null;
    private bool isPressed = false;

    private void Start()
    {
        controllerBtn = GetComponent<Image>();
    }

    private void Update()
    {
        isPressed = controllerBtn.color == Color.gray;

        if(isPressed)
        {
            switch (controllerType)
            {
                case ControllerType.LEFT:
                    PlayerInput.Instance.joystickKeyHorizontal = Mathf.Lerp(PlayerInput.Instance.joystickKeyHorizontal, -1, Time.deltaTime * 5);
                    break;
                case ControllerType.RIGHT:
                    PlayerInput.Instance.joystickKeyHorizontal = Mathf.Lerp(PlayerInput.Instance.joystickKeyHorizontal, 1, Time.deltaTime * 5);
                    break;
            }
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        controllerBtn.color = Color.gray;

        switch (controllerType)
        {
            case ControllerType.JUMP:
                PlayerInput.Instance.JoyStickJump();
                break;
            case ControllerType.ABILITY:
                PlayerInput.Instance.JoyStickAbility();
                break;
            case ControllerType.LEFT:
                PlayerInput.Instance.JoyStickHorizontalRaw(-1);
                break;
            case ControllerType.RIGHT:
                PlayerInput.Instance.JoyStickHorizontalRaw(1);
                break;
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        controllerBtn.color = Color.white;

        switch (controllerType)
        {
            case ControllerType.LEFT:
            case ControllerType.RIGHT:
                PlayerInput.Instance.JoyStickHorizontalRaw(0);
                PlayerInput.Instance.joystickKeyHorizontal = 0;
                break;
        }
    }
    
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            controllerBtn.color = Color.gray;

            switch (controllerType)
            {
                case ControllerType.LEFT:
                    PlayerInput.Instance.JoyStickHorizontalRaw(-1);
                    break;
                case ControllerType.RIGHT:
                    PlayerInput.Instance.JoyStickHorizontalRaw(1);
                    break;
            }
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        controllerBtn.color = Color.white;

        switch (controllerType)
        {
            case ControllerType.LEFT:
            case ControllerType.RIGHT:
                PlayerInput.Instance.JoyStickHorizontalRaw(0);
                PlayerInput.Instance.joystickKeyHorizontal = 0;
                break;
        }
    }
}
