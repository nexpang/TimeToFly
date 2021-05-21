using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ability_TimeFaster : Ability, IAbility
{
    [Header("�ɷ� �� ������")]
    [SerializeField] GameObject clockUI = null;
    [SerializeField] Image clockUIFill = null;
    [SerializeField] Animator abilityEffectAnim = null;
    [SerializeField] RectTransform clockUINeedle = null;
    [SerializeField] float abilityDefaultTime = 15;
    public float currentTime = 15;

    private bool isTimeFast = false;
    public bool IsTimeFast { get { return isTimeFast; } set { isTimeFast = value; } }

    public float speedUp = 2f;
    [SerializeField] GameObject effect = null;
    [SerializeField] GameObject player = null;

    new void Start()
    {
        base.Start();
        currentTime = abilityDefaultTime;
    }

    public void OnAbility()
    {
        if (abilityCurrentCoolDown > 0)
        {
            GameManager.SetAudio(audioSource, Audio_deniedAbility, false);
            abilityCooldownCircle.DOComplete();
            abilityCooldownCircle.color = Color.red;
            abilityCooldownCircle.DOColor(Color.black, 0.5f);
            return;
        }// ��Ÿ���� ���� �ȵƴ�.

        abilityCurrentCoolDown = abilityCooldown;
        abilityCurrentCoolDownTime = Time.time;

        Debug.Log("�ɷ� �ѽ�����");
        clockUI.SetActive(true);
        DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0.75f, 2f);
        GlitchEffect.Instance.colorIntensity = 0.100f;
        GlitchEffect.Instance.flipIntensity = 0.194f;
        GlitchEffect.Instance.intensity = 0.194f;
        StartCoroutine(Clock());
        abilityEffectAnim.SetTrigger("BlueT");

        //�ɷ� ����
        IsTimeFast = true;
        PlayerController.Instance._speed = speedUp;
        effect.SetActive(true);
        Time.timeScale = 1f / speedUp;
        player.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        Debug.Log("�ɷ� �ӵ���");
    }
    new void Update()
    {
        base.Update();
        clockUIFill.fillAmount = 1 - (currentTime / abilityDefaultTime);
        clockUINeedle.rotation = Quaternion.Euler(0, 0, -360 * (1 - (currentTime / abilityDefaultTime)));

        Using();
        //effect.GetComponent<ParticleSystemRenderer>().material.mainTexture = player.sprite.texture; �ȵ�

        if (PlayerController.Instance.playerState == PlayerState.DEAD)//���� �������¶��
        {
            StopCoroutine(Clock()); // �ð踦 ������Ų��.
        }
    }

    void Using()
    {
        if (!IsTimeFast) return;

    }

    public void ResetPlayer()
    {
        //�ɷ� �ߴ�
        player.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        Time.timeScale = 1f;
        IsTimeFast = false;
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
            if (PlayerController.Instance.playerState == PlayerState.DEAD)//���� �������¶��
            {
                break; // WHILE�� ������
            }
        }

        if (PlayerController.Instance.playerState != PlayerState.DEAD)
        {
            ResetPlayer();
        }
    }
}
