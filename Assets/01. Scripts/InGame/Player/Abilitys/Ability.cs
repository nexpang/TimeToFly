using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [Header("±âº»°ª")]
    [SerializeField] protected Image abilityBtn = null;
    [SerializeField] protected Image abilityCooldownCircle = null;
    [SerializeField] Sprite abilityBtnSpr = null;
    [SerializeField] protected float abilityCooldown = 30f;
    protected float abilityCurrentCoolDownTime;
    protected float abilityCurrentCoolDown = 0f;

    private void Start()
    {
        abilityBtn.sprite = abilityBtnSpr;
    }

    private void Update()
    {
        abilityCooldownCircle.fillAmount = abilityCurrentCoolDown / abilityCooldown;

        if(abilityCurrentCoolDown > 0)
        {
            abilityCurrentCoolDown = -(Time.time - abilityCurrentCoolDownTime) + abilityCooldown;
        }
    }
}
