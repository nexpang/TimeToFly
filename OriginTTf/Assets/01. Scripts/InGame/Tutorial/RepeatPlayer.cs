using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatPlayer : MonoBehaviour
{
    [SerializeField] private Sprite[] spriteSheet;

    private int currentSheet = 0;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer playerSR;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerSR = FindObjectOfType<PlayerAnimation>().GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        for (int i = 0; i < spriteSheet.Length; i++)
        {
            if (playerSR.sprite == spriteSheet[i]) // 0은 기본 애니메이션이 들어간 스프라이트들.
            {
                currentSheet = i;
            }
        }

        spriteRenderer.sprite = spriteSheet[currentSheet];
        spriteRenderer.flipX = playerSR.flipX;
    }
}
