using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAbility : Ability, IAbility
{
    [Header("능력 별 변수들")]
    [SerializeField] Image clockUI = null;

    public void OnAbility()
    {
        if (abilityCurrentCoolDown > 0) return; // 쿨타임이 아직 안됐다.
        abilityCurrentCoolDown = abilityCooldown;
        abilityCurrentCoolDownTime = Time.time;

        Debug.Log("능력 뿌슝빠슝");
    }
}
