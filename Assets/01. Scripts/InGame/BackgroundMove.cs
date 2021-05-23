using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BackgroundType
{
    MAIN,
    SUB
}

public class BackgroundMove : MonoBehaviour
{
    [SerializeField] BackgroundType type = BackgroundType.MAIN;
    private Image BGImg = null;
    private Vector2 offset = Vector2.zero; // == new Vector2(0f, 0f)

    [SerializeField]
    private float speed = 0.1f;
    [SerializeField]
    private float autoMoveDefault = 0.5f;
    private float autoMoveCurrent = 0.5f;

    [Header("Sub일때만 하세요")]
    [SerializeField]
    private Vector2 clampOffect = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        autoMoveCurrent = autoMoveDefault;
        BGImg = gameObject.GetComponent<Image>();

        if (type == BackgroundType.SUB) offset.x = transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance.playerState == PlayerState.DEAD || PlayerController.Instance.isStun) return;

        if (PlayerController.Instance.ability1.enabled)
        {
            autoMoveCurrent = PlayerController.Instance.ability1.IsSleep() ? 30 : autoMoveDefault;
        }
        else if (PlayerController.Instance.ability2.enabled)
        {
            autoMoveCurrent = PlayerController.Instance.ability2.IsTimeFast ? 0 : autoMoveDefault;
        }


        if (type == BackgroundType.MAIN)
        {
            offset.x += speed * Time.deltaTime * PlayerInput.Instance.KeyHorizontal * PlayerController.Instance.currentMoveS * 7;
            offset.x += autoMoveCurrent * Time.deltaTime;
            offset.x = Mathf.Clamp(offset.x, 0, 10);
            BGImg.material.SetTextureOffset("_MainTex", offset);
        }
        else
        {
            Clamp();
        }
    }

    void Clamp()
    {
        offset.x -= speed * Time.deltaTime * PlayerInput.Instance.KeyHorizontal * PlayerController.Instance.currentMoveS * 7;
        offset.x = Mathf.Clamp(offset.x, clampOffect.x, clampOffect.y);

        transform.localPosition = new Vector3(offset.x, transform.localPosition.y);
    }
}
