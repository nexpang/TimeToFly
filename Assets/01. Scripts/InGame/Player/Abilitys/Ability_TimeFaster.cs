using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ability_TimeFaster : Ability, IAbility
{
    [Header("능력 별 변수들(모래시계)")]
    [SerializeField] GameObject clockUI = null;
    [SerializeField] RectTransform clockUIClock = null;
    [SerializeField] RectTransform clockUISandClock = null;
    [SerializeField] RectTransform clockUISecondHand = null;
    [SerializeField] RawImage stringEffect = null;
    [SerializeField] RawImage featherEffect = null;
    [SerializeField] ParticleSystem abilityParticle = null;

    Rect stringRect = new Rect(0, 0, 1, 1);
    float rectX = 0;
    Rect featherRect = new Rect(0, 0, 1, 1);
    float rectY = 0;

    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float stringEffectSpeed = 1;
    [SerializeField] float featherEffectSpeed = 1;
    [SerializeField] Image sandClockWhite = null;
    [SerializeField] Sprite brokenClock;
    Sprite defaultClock;
    //=================================여기까지 시계 이펙트다.=============

    [Header("능력 별 변수들")]
    [SerializeField] Animator abilityEffectAnim = null;
    [SerializeField] float abilityDefaultTime = 15;
    public float currentTime = 15;

    public float speedUp = 2f;
    [SerializeField] GameObject effect = null;
    [SerializeField] GameObject player = null;

    [Header("효과음")]
    [SerializeField] AudioSource bgAudioSource = null;
    [SerializeField] AudioSource playerAudioSource = null;
    [SerializeField] AudioSource audioClockSoundSource = null;
    [SerializeField] AudioClip Audio_futureEnter = null;
    [SerializeField] AudioClip Audio_presentEnter = null;
    [SerializeField] AudioClip Audio_tik = null;
    [SerializeField] AudioClip Audio_tok = null;
    [SerializeField] AudioClip Audio_glassBroken = null;

    private new void OnEnable() // 필요할때만 켜지게
    {
        base.OnEnable();
        sandClockWhite.gameObject.SetActive(true);
        clockUISandClock.eulerAngles = Vector3.zero;
    }

    private void OnDisable()
    {
        if (sandClockWhite != null)
        {
            sandClockWhite.gameObject.SetActive(false);
        }
    }

    new void Start()
    {
        base.Start();
        currentTime = abilityDefaultTime;
        defaultClock = clockUISandClock.GetComponent<Image>().sprite;
    }

    public void OnAbility()
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
        abilityCurrentCoolDownTime = Time.time; // 쿨타임 돌려주고

        clockUI.SetActive(true); // 시계 UI를 켜준다.
        tween.Kill(); // 트윈 초기화
        // 시계 알파값 닷트윈으로 올려주고
        tween = DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0.75f, 2f);

        // 글리치 이펙트 켜주고
        GlitchEffect.Instance.colorIntensity = 0.100f;
        GlitchEffect.Instance.flipIntensity = 0.194f;
        GlitchEffect.Instance.intensity = 0.194f;

        // 시계 초가 시작된다.
        StartCoroutine(Clock());

        // 미래 이펙트 실행시켜준다.
        abilityEffectAnim.SetTrigger("BlueT");

        // 미래 예지 효과음
        GameManager.Instance.SetAudio(audioSource, Audio_futureEnter, 1, false);
        bgAudioSource.DOPitch(1.5f, 3);
        playerAudioSource.DOPitch(1.5f, 3); // 빠른 애이기때문에 피치를 올려준다.
        DOTween.To(() => bgAudioSource.volume, value => bgAudioSource.volume = value, 0.4f, 2f); // 볼륨은 낮춘다.

        //능력 시작
        isAbilityEnable = true;
        GameManager.Instance.player._speed = speedUp;
        effect.SetActive(true);
        GameManager.Instance.timerScale = 1f / 1.3f;
        //Time.timeScale = 1f / speedUp;
        //player.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;

        int randomRotate = Random.Range(0, 2);

        sandClockWhite.material.DOFade(1, 0.25f).SetDelay(1).OnComplete(() =>
        {
            GameManager.Instance.SetAudio(audioSource, Audio_glassBroken, 1, false);
            abilityParticle.Play();
            sandClockWhite.material.color = new Color(1, 1, 1, 0);
            clockUISandClock.GetComponent<Image>().sprite = brokenClock;
            clockUISandClock.DOAnchorPos(new Vector2(Random.Range(-180,180), -1477), 2).SetEase(Ease.InOutCubic);
            clockUISandClock.DORotate(new Vector3(0, 0, (randomRotate == 0 ) ? 180 : -180), 3);
        });
        DOTween.To(() => rotateSpeed, value => rotateSpeed = value, 350, 5);
        DOTween.To(() => stringEffectSpeed, value => stringEffectSpeed = value, 3, 5).SetEase(Ease.InOutCubic);
        DOTween.To(() => featherEffectSpeed, value => featherEffectSpeed = value, 3, 5);
    }
    new void Update()
    {
        base.Update();

        Using();

        if (GameManager.Instance.player.playerState == PlayerState.DEAD)//만약 죽은상태라면
        {
            StopCoroutine(Clock()); // 시계를 정지시킨다.
        }
    }

    void Using()
    {
        if (!isAbilityEnable) return;

        clockUIClock.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        clockUISecondHand.localRotation = Quaternion.Euler(0, 0, -360 * (1 - (currentTime / abilityDefaultTime)));
        rectX += Time.deltaTime * stringEffectSpeed;
        rectY += Time.deltaTime * featherEffectSpeed;

        stringRect.Set(-rectX, 0, 1, 1);
        featherRect.Set(0, rectY, 1, 1);

        stringEffect.uvRect = stringRect;
        featherEffect.uvRect = featherRect;

        int seconds = Mathf.FloorToInt(abilityDefaultTime - currentTime);

        if (GameManager.Instance.player.playerState != PlayerState.DEAD)
        {
            if (seconds % 2 == 1)
            {
                GameManager.Instance.SetAudio(audioClockSoundSource, Audio_tik, 1, false);
            }
            else
            {
                GameManager.Instance.SetAudio(audioClockSoundSource, Audio_tok, 1, false);
            }
        }
    }

    public void ResetPlayer()
    {
        //능력 중단
        //player.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        //Time.timeScale = 1f;
        GameManager.Instance.timerScale = 1f;
        isAbilityEnable = false;
        effect.SetActive(false);
        GameManager.Instance.player._speed = 1;

        currentTime = abilityDefaultTime;

        DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0f, 2f).OnComplete(() => {
            clockUI.SetActive(false);
            clockUISandClock.anchoredPosition = Vector2.zero;
            clockUISandClock.eulerAngles = Vector3.zero;

            clockUISandClock.GetComponent<Image>().sprite = defaultClock;

            rotateSpeed = 10;
            stringEffectSpeed = 0.5f;
            featherEffectSpeed = 0.1f;
        });

        GlitchEffect.Instance.colorIntensity = 0;
        GlitchEffect.Instance.flipIntensity = 0;
        GlitchEffect.Instance.intensity = 0;

        abilityEffectAnim.SetTrigger("OrangeT");
        bgAudioSource.DOPitch(1, 2);
        playerAudioSource.DOPitch(1, 2);
        DOTween.To(() => bgAudioSource.volume, value => bgAudioSource.volume = value, GameManager.Instance.defaultBGMvolume, 2f);
        GameManager.Instance.SetAudio(audioSource, Audio_presentEnter, 1);
    }

    IEnumerator Clock()
    {
        yield return new WaitForSeconds(1);

        while (currentTime > 0)
        {
            currentTime--;
            yield return new WaitForSeconds(1);
            if (GameManager.Instance.player.playerState == PlayerState.DEAD)//만약 죽은상태라면
            {
                break; // WHILE문 나가기
            }
        }

        if (GameManager.Instance.player.playerState != PlayerState.DEAD)
        {
            ResetPlayer();
        }
    }
}
