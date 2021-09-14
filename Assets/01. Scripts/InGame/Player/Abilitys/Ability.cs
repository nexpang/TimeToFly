using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ability : MonoBehaviour
{
    [Header("기본값")]
    [SerializeField] protected Image abilityBtn = null;
    [SerializeField] protected Image abilityCooldownCircle = null;
    [SerializeField] protected Sprite abilityBtnSpr = null;
    [SerializeField] protected float abilityCooldown = 30f;
    protected float abilityCurrentCoolDownTime;
    [SerializeField]  protected float abilityCurrentCoolDown = 0f;
    [SerializeField] protected AudioSource audioSource = null;
    [SerializeField] protected AudioClip Audio_deniedAbility = null;
    [HideInInspector] public bool isReady;// 쿨타임이 끝나고 준비가 되었나? (튜토리얼 용)
    [HideInInspector] protected Tween tween; // 시계 없어지는거 방지용 트윈
    [HideInInspector] public bool isAbilityEnable;

    protected virtual void Start()
    {
        if(SceneController.targetMapId == 0)
        {
            abilityCooldown = 1;
            abilityCurrentCoolDown = abilityCooldown;
        }
    }

    protected virtual void OnEnable()
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

        isReady = abilityCurrentCoolDown <= 0;
    }
}
