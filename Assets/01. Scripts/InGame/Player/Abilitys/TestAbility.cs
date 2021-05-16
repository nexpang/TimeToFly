using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAbility : Ability, IAbility
{
    [Header("�ɷ� �� ������")]
    [SerializeField] Image clockUI = null;

    public void OnAbility()
    {
        if (abilityCurrentCoolDown > 0) return; // ��Ÿ���� ���� �ȵƴ�.
        abilityCurrentCoolDown = abilityCooldown;
        abilityCurrentCoolDownTime = Time.time;

        Debug.Log("�ɷ� �ѽ�����");
    }
}
