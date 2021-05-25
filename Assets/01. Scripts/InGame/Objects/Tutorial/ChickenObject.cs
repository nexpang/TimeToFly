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
        if(pC.abilityNumber == 1)
        {
            if (pC.abilitys[1].GetComponent<Ability_FutureCreate>().IsSleep()) return false;

        }else if (pC.abilityNumber == 2)
        {
            if (pC.abilitys[2].GetComponent<Ability_TimeFaster>().IsTimeFast) return false;
        }

        tempChickenId = PlayerController.Instance.abilityNumber;
        GetComponent<SpriteRenderer>().sprite = playerSleepSpr[tempChickenId];
        PlayerController.Instance.PlayerAbilitySet(chickenId);
        chickenId = tempChickenId;

        for (int i = 0; i < playerAbilityLore.Length; i++)
        {
            playerAbilityLore[i].SetActive(pC.abilityNumber == i);
        }

        return true;
    }
}
