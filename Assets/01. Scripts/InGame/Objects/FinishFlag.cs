using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishFlag : MonoBehaviour
{
    public GameObject[] chickens;
    public Sprite[] chickensIdle;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        List<int> chickenList = GameManager.Instance.remainChickenIndex;
        chickenList.Remove(GameManager.Instance.player.abilityNumber);

        for(int i = 0; i<chickens.Length;i++)
        {
            chickens[i].SetActive(false);

            for (int j = 0; j<chickenList.Count;j++)
            {
                if(i == j)
                {
                    chickens[i].SetActive(true);
                    chickens[i].GetComponent<SpriteRenderer>().sprite = chickensIdle[chickenList[j]];
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Ability_FutureCreate ability = (Ability_FutureCreate)GameManager.Instance.player.abilitys[(int)Chickens.BROWN];
            if (ability.enabled && ability.isAbilityEnable) // 만약 능력 1이고 자고있는 상태면
            {
                return;
            }

            boxCollider.enabled = false;
            GameManager.Instance.player.GameClear();
        }
    }
}
