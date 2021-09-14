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
