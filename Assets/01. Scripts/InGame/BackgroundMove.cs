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

    [Header("메인이 clamp가 필요한가요?")]
    [SerializeField]
    bool isNeedClamp = true;
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
        if (GameManager.Instance.player.playerState == PlayerState.DEAD || GameManager.Instance.player.isStun) return;

        if (GameManager.Instance.player.abilitys[(int)Chickens.BROWN].gameObject.activeSelf)
        {
            autoMoveCurrent = GameManager.Instance.player.abilitys[(int)Chickens.BROWN].isAbilityEnable ? -30 : autoMoveDefault;
        }
        else if (GameManager.Instance.player.abilitys[(int)Chickens.BLUE].gameObject.activeSelf)
        {
            autoMoveCurrent = GameManager.Instance.player.abilitys[(int)Chickens.BLUE].isAbilityEnable ? 100 : autoMoveDefault;
        }


        if (type == BackgroundType.MAIN)
        {
            offset.x += speed * Time.deltaTime * PlayerInput.Instance.KeyHorizontal * GameManager.Instance.player.currentMoveS * 7;
            offset.x += speed * autoMoveCurrent * Time.deltaTime;
            if(isNeedClamp) offset.x = Mathf.Clamp(offset.x, 0, 10);
            BGImg.material.SetTextureOffset("_MainTex", offset);
        }
        else
        {
            Clamp();
        }
    }

    void Clamp()
    {
        offset.x -= speed * Time.deltaTime * PlayerInput.Instance.KeyHorizontal * GameManager.Instance.player.currentMoveS * 7;
        offset.x = Mathf.Clamp(offset.x, clampOffect.x, clampOffect.y);

        transform.localPosition = new Vector3(offset.x, transform.localPosition.y);
    }
}
