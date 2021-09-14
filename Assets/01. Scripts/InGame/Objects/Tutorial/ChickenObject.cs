using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenObject : InteractionObject
{
    public int chickenId;
    private int tempChickenId;
    public new BoxCollider2D collider2D = null;

    [SerializeField] PlayerController pC;
    [SerializeField] Sprite[] playerSleepSpr;

    private void Awake()
    {
        collider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Ability_FutureCreate ability1 = (Ability_FutureCreate)GameManager.Instance.player.abilitys[(int)Chickens.BROWN];

            Ability_TimeFaster ability2 = (Ability_TimeFaster)GameManager.Instance.player.abilitys[(int)Chickens.BLUE];
            if (ability1.enabled && ability1.isAbilityEnable) // 만약 능력 1이고 자고있는 상태면
            {
                ability1.ResetPlayer();
                return;
            }
            else if(ability2.enabled && ability2.isAbilityEnable)
            {
                ability2.ResetPlayer();
            }
            isAlreadyChange = false;
            GameManager.Instance.player._speed = 0;
            collider2D.enabled = false;
        }
    }

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
