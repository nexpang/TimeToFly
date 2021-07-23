using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishFlag : MonoBehaviour
{
    public GameObject[] chickens;
    public Sprite[] chickensIdle;

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
            // TO DO : Å¬¸®¾î
        }
    }
}
