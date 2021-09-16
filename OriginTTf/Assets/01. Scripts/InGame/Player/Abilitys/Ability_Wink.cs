using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ability_Wink : Ability, IAbility
{
    [Header("능력 별 변수들")]
    [SerializeField] ParticleSystem effect = null;
    [SerializeField] AudioClip Audio_playerWink = null;
    [SerializeField] AudioClip Audio_playerWink2 = null;
    Animator playerAnimation = null;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();
        playerAnimation = FindObjectOfType<PlayerAnimation>().GetComponent<Animator>();
    }


    void IAbility.OnAbility()
    {
        if (abilityCurrentCoolDown > 0)
        {
            GameManager.Instance.SetAudio(audioSource, Audio_deniedAbility, 0.5f, false);
            abilityCooldownCircle.DOComplete();
            abilityCooldownCircle.color = Color.red;
            abilityCooldownCircle.DOColor(new Color(0, 0, 0, 0.75f), 0.5f);
            return;
        }// 쿨타임이 아직 안됐다.
        abilityCurrentCoolDown = abilityCooldown;
        abilityCurrentCoolDownTime = Time.time;
        playerAnimation.SetTrigger("WinkT");
        GameManager.Instance.SetAudio(audioSource, Audio_playerWink, 0.6f);
        GameManager.Instance.SetAudioImmediate(audioSource, Audio_playerWink2, 0.6f);
        effect.Play();
    }
}