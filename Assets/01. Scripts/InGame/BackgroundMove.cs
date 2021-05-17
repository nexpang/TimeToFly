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
    private float autoMove = 0.1f;

    [Header("Sub�϶��� �ϼ���")]
    [SerializeField]
    private Vector2 clampOffect = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        BGImg = gameObject.GetComponent<Image>();

        if (type == BackgroundType.SUB) offset.x = transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance.playerState == PlayerState.DEAD || PlayerController.Instance.isStun) return;

        if (type == BackgroundType.MAIN)
        {
            offset.x += speed * Time.deltaTime * (PlayerInput.Instance.KeyHorizontal + autoMove);
            BGImg.material.SetTextureOffset("_MainTex", offset);
        }
        else
        {
            Clamp();
        }
    }

    [ContextMenu("�׽�Ʈ")]
    void Clamp()
    {
        offset.x -= speed * Time.deltaTime * PlayerInput.Instance.KeyHorizontal;
        offset.x = Mathf.Clamp(offset.x, clampOffect.x, clampOffect.y);

        transform.localPosition = new Vector3(offset.x, transform.localPosition.y);
    }
}
