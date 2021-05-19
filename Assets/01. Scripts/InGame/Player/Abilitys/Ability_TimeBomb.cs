using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ability_TimeBomb : Ability, IAbility
{
    [Header("�ɷ� �� ������")]
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
            Debug.Log("�ִϸ��̼� ������");
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
            if (abilityCurrentCoolDown > 0) return; // ��Ÿ���� ���� �ȵƴ�.
            Using();

            abilityEffectAnim.SetTrigger("BlueT");
        }

        Debug.Log("�ɷ� �ѽ�����");

        //�ɷ� ����
        Debug.Log("�ɷ� �ӵ���");
    }
    new void Update()
    {
        base.Update();

        //effect.GetComponent<ParticleSystemRenderer>().material.mainTexture = player.sprite.texture; �ȵ�
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
        //�ɷ� �ߴ�


        currentTime = abilityDefaultTime;

        DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0f, 2f).OnComplete(() => clockUI.SetActive(false));

        GlitchEffect.Instance.colorIntensity = 0;
        GlitchEffect.Instance.flipIntensity = 0;
        GlitchEffect.Instance.intensity = 0;

        abilityEffectAnim.SetTrigger("OrangeT");
    }
}
