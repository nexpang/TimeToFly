using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct Sheet
{
    public Sprite[] sprites;
};

public class PlayerSprites : MonoBehaviour
{
    [SerializeField] private Sheet[] spriteSheet;
    public int targetSheet = 0;
    private int currentSheet = 0;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        for (int i = 0; i < spriteSheet[0].sprites.Length; i++)
        {
            if (spriteRenderer.sprite == spriteSheet[0].sprites[i]) // 0은 기본 애니메이션이 들어간 스프라이트들.
            {
                currentSheet = i;
            }
        }

        spriteRenderer.sprite = spriteSheet[targetSheet].sprites[currentSheet];
    }
}