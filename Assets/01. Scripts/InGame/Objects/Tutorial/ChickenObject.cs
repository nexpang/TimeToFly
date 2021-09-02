using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenObject : InteractionObject
{
    public int chickenId;
    private int tempChickenId;

    [SerializeField] PlayerController pC;
    [SerializeField] Sprite[] playerSleepSpr;

    public override bool OnInteraction()
    {
        if (pC.abilitys[pC.abilityNumber].isAbilityEnable) return false;

        tempChickenId = GameManager.Instance.player.abilityNumber;
        GetComponent<SpriteRenderer>().sprite = playerSleepSpr[tempChickenId];
        GameManager.Instance.player.PlayerAbilitySet(chickenId);
        chickenId = tempChickenId;

        return true;
    }
}
