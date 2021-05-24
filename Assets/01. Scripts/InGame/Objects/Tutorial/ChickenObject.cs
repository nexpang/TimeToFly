using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenObject : InteractionObject
{
    public int chickenId;
    private int tempChickenId;

    [SerializeField] PlayerController pC;
    [SerializeField] Sprite[] playerSleepSpr;

    public override void OnInteraction()
    {
        tempChickenId = PlayerController.Instance.abilityNumber;
        GetComponent<SpriteRenderer>().sprite = playerSleepSpr[tempChickenId];
        PlayerController.Instance.PlayerAbilitySet(chickenId);
        chickenId = tempChickenId;
    }
}
