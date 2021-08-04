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
    ABILITY,
    INTERACTION
}

public class MobileController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] ControllerType controllerType;
    private Image controllerBtn = null;
    private bool isPressed = false;
    [SerializeField] private Sprite defaultSpr = null;
    [SerializeField] private Sprite pressedSpr = null;

    public RectTransform abilityImgRectTrm;

    private void Start()
    {
        controllerBtn = GetComponent<Image>();
    }

    private void Update()
    {
        if(!Input.GetMouseButton(0))
        {
            isPressed = false;

            switch (controllerType)
            {
                case ControllerType.LEFT:
                case ControllerType.RIGHT:
                    PlayerInput.Instance.JoyStickHorizontalRaw(0);
                    PlayerInput.Instance.joystickKeyHorizontal = 0;
                    break;
            }
        }

        if(isPressed)
        {
            if (controllerType != ControllerType.ABILITY) controllerBtn.sprite = pressedSpr;

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
        else
        {
            if(controllerType != ControllerType.ABILITY) controllerBtn.sprite = defaultSpr;
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;

        switch (controllerType)
        {
            case ControllerType.JUMP:
                PlayerInput.Instance.JoyStickJump();
                break;
            case ControllerType.ABILITY:
                abilityImgRectTrm.localPosition = abilityImgRectTrm.localPosition + (Vector3.down*15);
                PlayerInput.Instance.JoyStickAbility();
                break;
            case ControllerType.INTERACTION:
                PlayerInput.Instance.JoyStickInteration();
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
        isPressed = false;

        switch (controllerType)
        {
            case ControllerType.LEFT:
            case ControllerType.RIGHT:
                PlayerInput.Instance.JoyStickHorizontalRaw(0);
                PlayerInput.Instance.joystickKeyHorizontal = 0;
                break;
            case ControllerType.ABILITY:
                abilityImgRectTrm.localPosition = abilityImgRectTrm.localPosition + (Vector3.up*15);
                PlayerInput.Instance.JoyStickAbilityOn();
                break;
        }
    }
    
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            isPressed = true;

            switch (controllerType)
            {
                case ControllerType.LEFT:
                    PlayerInput.Instance.JoyStickHorizontalRaw(-1);
                    break;
                case ControllerType.RIGHT:
                    PlayerInput.Instance.JoyStickHorizontalRaw(1);
                    break;
                case ControllerType.ABILITY:
                    PlayerInput.Instance.JoyStickAbilityHold();
                    break;
            }
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isPressed = false;

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
