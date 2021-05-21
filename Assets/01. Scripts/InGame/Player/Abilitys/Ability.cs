using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [Header("�⺻��")]
    [SerializeField] protected Image abilityBtn = null;
    [SerializeField] protected Image abilityCooldownCircle = null;
    [SerializeField] Sprite abilityBtnSpr = null;
    [SerializeField] protected float abilityCooldown = 30f;
    protected float abilityCurrentCoolDownTime;
    [SerializeField]  protected float abilityCurrentCoolDown = 0f;
    [SerializeField] protected AudioSource audioSource = null;
    [SerializeField] protected AudioClip Audio_deniedAbility = null;

    protected void Start()
    {
        abilityBtn.sprite = abilityBtnSpr;
    }

    protected void Update()
    {
        abilityCooldownCircle.fillAmount = abilityCurrentCoolDown / abilityCooldown;

        if(abilityCurrentCoolDown > 0)
        {
            abilityCurrentCoolDown = -(Time.time - abilityCurrentCoolDownTime) + abilityCooldown;
        }
    }
}
