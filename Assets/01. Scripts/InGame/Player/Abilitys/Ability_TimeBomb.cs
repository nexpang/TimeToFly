using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ability_TimeBomb : Ability, IAbility
{
    [Header("능력 별 변수들")]
    [SerializeField] GameObject clockUI = null;
    [SerializeField] Animator abilityEffectAnim = null;
    [SerializeField] float abilityDefaultTime = 15;
    public float currentTime = 15;

    bool isAnimationPlaying = false;
    private bool hasTimeBoom = false;
    public bool _hasTimeBoom { get { return hasTimeBoom; } set { hasTimeBoom = value; } }

    [SerializeField] GameObject player = null;
    [SerializeField] GameObject timeBoom = null;
    GameObject myBoom = null;

    new void Start()
    {
        base.Start();
        currentTime = abilityDefaultTime;
    }

    public void OnAbility()
    {
        if (isAnimationPlaying)
        {
            Debug.Log("애니메이션 실행중");
            return;
        }
        if (hasTimeBoom)
        {
            hasTimeBoom = false;

            Destroy(myBoom);


            abilityCurrentCoolDown = abilityCooldown;
            abilityCurrentCoolDownTime = Time.time;
        }
        else
        {
            if (abilityCurrentCoolDown > 0) return; // 쿨타임이 아직 안됐다.
            Using();

            abilityEffectAnim.SetTrigger("BlueT");
        }

        Debug.Log("능력 뿌슝빠슝");

        //능력 시작
        Debug.Log("능력 속도업");
    }
    new void Update()
    {
        base.Update();

        //effect.GetComponent<ParticleSystemRenderer>().material.mainTexture = player.sprite.texture; 안됨
    }

    void Using()
    {
        isAnimationPlaying = true;
        Invoke("EndAnimation", 1.5f);
        hasTimeBoom = true;
        myBoom = Instantiate(timeBoom, new Vector3(transform.position.x, transform.position.y +0.8f, 1), Quaternion.identity, transform);
    }

    void EndAnimation()
    {
        isAnimationPlaying = false;
    }

    public void ResetPlayer()
    {
        //능력 중단


        currentTime = abilityDefaultTime;

        DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0f, 2f).OnComplete(() => clockUI.SetActive(false));

        GlitchEffect.Instance.colorIntensity = 0;
        GlitchEffect.Instance.flipIntensity = 0;
        GlitchEffect.Instance.intensity = 0;

        abilityEffectAnim.SetTrigger("OrangeT");
    }
}
