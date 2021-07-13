using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenObject : InteractionObject
{
    public int chickenId;
    private int tempChickenId;

    [SerializeField] PlayerController pC;
    [SerializeField] Sprite[] playerSleepSpr;
    [SerializeField] GameObject[] playerAbilityLore;

    public override bool OnInteraction()
    {
        if (pC.abilitys[pC.abilityNumber].isAbilityEnable) return false;

        tempChickenId = GameManager.Instance.player.abilityNumber;
        GetComponent<SpriteRenderer>().sprite = playerSleepSpr[tempChickenId];
        GameManager.Instance.player.PlayerAbilitySet(chickenId);
        chickenId = tempChickenId;

        for (int i = 0; i < playerAbilityLore.Length; i++)
        {
            playerAbilityLore[i].SetActive(pC.abilityNumber == i);
        }

        return true;
    }
}
