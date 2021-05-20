using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Wink : Ability, IAbility
{
    [Header("�ɷ� �� ������")]
    [SerializeField] ParticleSystem effect = null;
    Animator playerAnimation = null;

    new void Start()
    {
        base.Start();
        playerAnimation = FindObjectOfType<PlayerAnimation>().GetComponent<Animator>();
    }


    void IAbility.OnAbility()
    {
        if (abilityCurrentCoolDown > 0) return; // ��Ÿ���� ���� �ȵƴ�.
        abilityCurrentCoolDown = abilityCooldown;
        abilityCurrentCoolDownTime = Time.time;
        playerAnimation.SetTrigger("WinkT");
        effect.Play();
    }
}