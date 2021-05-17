using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TestAbility2 : Ability, IAbility
{
    [Header("능력 별 변수들")]
    [SerializeField] GameObject clockUI = null;
    [SerializeField] Image clockUIFill = null;
    [SerializeField] Animator abilityEffectAnim = null;
    [SerializeField] RectTransform clockUINeedle = null;
    [SerializeField] float abilityDefaultTime = 15;
    public float currentTime = 15;

    public float speedUp = 2f;
    [SerializeField] GameObject effect = null;
    [SerializeField] SpriteRenderer player = null;

    new void Start()
    {
        base.Start();
        currentTime = abilityDefaultTime;
    }

    public void OnAbility()
    {
        if (abilityCurrentCoolDown > 0) return; // 쿨타임이 아직 안됐다.
        abilityCurrentCoolDown = abilityCooldown;
        abilityCurrentCoolDownTime = Time.time;

        Debug.Log("능력 뿌슝빠슝");
        clockUI.SetActive(true);
        DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0.75f, 2f);
        GlitchEffect.Instance.colorIntensity = 0.100f;
        GlitchEffect.Instance.flipIntensity = 0.194f;
        GlitchEffect.Instance.intensity = 0.194f;
        StartCoroutine(Clock());
        abilityEffectAnim.SetTrigger("BlueT");

        //능력 시작
        PlayerController.Instance._speed = speedUp;
        effect.SetActive(true);
        Debug.Log("능력 속도업");
    }
    new void Update()
    {
        base.Update();
        clockUIFill.fillAmount = 1 - (currentTime / abilityDefaultTime);
        clockUINeedle.rotation = Quaternion.Euler(0, 0, -360 * (1 - (currentTime / abilityDefaultTime)));

        //effect.GetComponent<ParticleSystemRenderer>().material.mainTexture = player.sprite.texture; 안됨

        if (PlayerController.Instance.playerState == PlayerState.DEAD)//만약 죽은상태라면
        {
            StopCoroutine(Clock()); // 시계를 정지시킨다.
        }
    }

    public void ResetPlayer()
    {
        //능력 중단
        effect.SetActive(false);
        PlayerController.Instance._speed = 1;

        currentTime = abilityDefaultTime;

        DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0f, 2f).OnComplete(() => clockUI.SetActive(false));

        GlitchEffect.Instance.colorIntensity = 0;
        GlitchEffect.Instance.flipIntensity = 0;
        GlitchEffect.Instance.intensity = 0;

        abilityEffectAnim.SetTrigger("OrangeT");
    }

    IEnumerator Clock()
    {
        yield return new WaitForSeconds(1);

        while (currentTime > 0)
        {
            currentTime--;
            yield return new WaitForSeconds(1);
            if (PlayerController.Instance.playerState == PlayerState.DEAD)//만약 죽은상태라면
            {
                break; // WHILE문 나가기
            }
        }

        if (PlayerController.Instance.playerState != PlayerState.DEAD)
        {
            ResetPlayer();
        }
    }
}
