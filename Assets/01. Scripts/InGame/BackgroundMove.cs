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

    private Ability_FutureCreate ability1 = null;

    [Header("Sub일때만 하세요")]
    [SerializeField]
    private Vector2 clampOffect = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        autoMoveCurrent = autoMoveDefault;
        ability1 = FindObjectOfType<Ability_FutureCreate>();
        BGImg = gameObject.GetComponent<Image>();

        if (type == BackgroundType.SUB) offset.x = transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance.playerState == PlayerState.DEAD || PlayerController.Instance.isStun) return;

        if (ability1 != null)
        {
            autoMoveCurrent = ability1.IsSleep() ? 30 : autoMoveDefault;
        }

        if (type == BackgroundType.MAIN)
        {
            offset.x += speed * Time.deltaTime * (PlayerInput.Instance.KeyHorizontal + autoMoveCurrent);
            BGImg.material.SetTextureOffset("_MainTex", offset);
        }
        else
        {
            Clamp();
        }
    }

    void Clamp()
    {
        offset.x -= speed * Time.deltaTime * PlayerInput.Instance.KeyHorizontal;
        offset.x = Mathf.Clamp(offset.x, clampOffect.x, clampOffect.y);

        transform.localPosition = new Vector3(offset.x, transform.localPosition.y);
    }
}
