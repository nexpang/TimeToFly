using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ability_FutureCreate : Ability, IAbility
{
    [Header("능력 별 변수들")]
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
    [SerializeField] Animator abilityEffectAnim = null;
    [SerializeField] float abilityDefaultTime = 15;
    public float currentTime = 15;
    [SerializeField] TrailRenderer effect = null;

    [Header("효과음")]
    [SerializeField] AudioSource bgAudioSource = null;
    [SerializeField] AudioSource audioClockSoundSource = null;
    [SerializeField] AudioClip Audio_futureEnter = null;
    [SerializeField] AudioClip Audio_presentEnter = null;
    [SerializeField] AudioClip Audio_tik = null;
    [SerializeField] AudioClip Audio_tok = null;
    [SerializeField] AudioClip Audio_futureBGM = null;
    [SerializeField] AudioClip Audio_futureBGM2 = null;

    [Header("미래 예지 플레이어 기억")]
    public List<Vector2> RecordNumber_XY = new List<Vector2>();
    public List<Sprite> RecordNumber_Sprite = new List<Sprite>();
    public List<bool> RecordNumber_SpriteFlipX = new List<bool>();
    [SerializeField] private Transform sleepPlayer = null;//----------------------------------- TO DO : 이거랑

    private bool isSleep = false;
    public bool IsSleep() => isSleep;

    private bool isRecording = false;
    private float recordTime = 0f;
    private float recordDelay = 0.03f;

    [Header("미래 예지 플레이어 플레이")]
    [SerializeField]
    private bool isFuturePlay = false;

    private float playTime = 0f;
    private float playDelay = 0.03f;
    private int playframe = 0;

    private PlayerController playerRb = null;
    private PlayerAnimation playerAn = null;
    [SerializeField] private Transform recordedPlayer = null;//---------------------------- TO DO : 이거 프리팹으로 만들어서 자동 생성되게 해야한다.

    // 함정 리셋
    ResetAbleTrap[] traps = null;

    new void Start()
    {
        base.Start();
        playerRb = FindObjectOfType<PlayerController>();
        playerAn = FindObjectOfType<PlayerAnimation>();
        currentTime = abilityDefaultTime;
        traps = FindObjectsOfType<ResetAbleTrap>();
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
        abilityCurrentCoolDownTime = Time.time;

        clockUI.SetActive(true);
        DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0.6f, 2f);
        playerAn.GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, 1);
        GlitchEffect.Instance.colorIntensity = 0.100f;
        GlitchEffect.Instance.flipIntensity = 0.194f;
        GlitchEffect.Instance.intensity = 0.194f;
        StartCoroutine(Clock());
        sleepPlayer.position = transform.position;
        sleepPlayer.GetComponent<SpriteRenderer>().flipX = playerAn.GetComponent<SpriteRenderer>().flipX;
        abilityEffectAnim.SetTrigger("BlueT");

        abilityParticle.gameObject.SetActive(true);
        abilityParticle.Play();
        effect.transform.SetParent(this.transform);
        effect.time = 0;
        effect.transform.localPosition = Vector3.zero;
        effect.time = 10;

        GameManager.Instance.SetAudio(audioSource, Audio_futureEnter, 1, false);
        int random = UnityEngine.Random.Range(0, 4);
        if(random == 0)
        {
            bgAudioSource.time = 0;
            GameManager.Instance.SetAudio(bgAudioSource, Audio_futureBGM, 0.8f, true);
        }
        else if (random == 1)
        {
            bgAudioSource.time = 22;
            GameManager.Instance.SetAudio(bgAudioSource, Audio_futureBGM, 0.8f, true);
        }
        else if (random == 2)
        {
            bgAudioSource.time = 0;
            GameManager.Instance.SetAudio(bgAudioSource, Audio_futureBGM2, 0.8f, true);
        }
        else if (random == 3)
        {
            bgAudioSource.time = 22;
            GameManager.Instance.SetAudio(bgAudioSource, Audio_futureBGM2, 0.8f, true);
        }



        isSleep = true;
    }

    new void Update()
    {
        base.Update();
        //clockUIFill.fillAmount = 1 - (currentTime / abilityDefaultTime);

        if (isSleep)
        {
            clockUIClock.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            clockUISandClock.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
            clockUISecondHand.rotation = Quaternion.Euler(0, 0, -360 * (1 - (currentTime / abilityDefaultTime)));
            rectX += Time.deltaTime * stringEffectSpeed;
            rectY += Time.deltaTime * featherEffectSpeed;

            stringRect.Set(-rectX, 0, 1, 1);
            featherRect.Set(0, rectY, 1, 1);

            stringEffect.uvRect = stringRect;
            featherEffect.uvRect = featherRect;

            int seconds = Mathf.FloorToInt(abilityDefaultTime - currentTime);

            if (PlayerController.Instance.playerState != PlayerState.DEAD)
            {
                if (seconds % 2 == 1)
                {
                    GameManager.Instance.SetAudio(audioClockSoundSource, Audio_tik, 1,false);
                }
                else
                {
                    GameManager.Instance.SetAudio(audioClockSoundSource, Audio_tok, 1, false);
                }
            }
        }
        else
        {
            abilityParticle.Stop();
            abilityParticle.gameObject.SetActive(false);
        }

        RecordPlayer();
        PlayPlayer();

        if (PlayerController.Instance.playerState == PlayerState.DEAD)//만약 죽은상태라면
        {
            StopCoroutine(Clock()); // 시계를 정지시킨다.
        }
        
    }

    void RecordPlayer()
    {
        if (isSleep)
        {
            if (!isRecording)
            {
                isRecording = true;
                isFuturePlay = false;
                RecordNumber_XY.Clear();
                RecordNumber_Sprite.Clear();
                RecordNumber_SpriteFlipX.Clear();
            }
        }
        else
        {
            isRecording = false;
        }

        if (isRecording)
        {
            recordTime = Time.time;
            recordTime = (float)Math.Round(recordTime * 100) / 100;
            //Debug.Log("recordTime : " + recordTime + ", recordDelay : " + recordDelay);
            if (recordTime >= recordDelay)
            {
                RecordNumber_XY.Add(new Vector2(playerRb.transform.position.x, playerRb.transform.position.y));
                RecordNumber_Sprite.Add(playerAn.GetComponent<SpriteRenderer>().sprite);
                RecordNumber_SpriteFlipX.Add(playerAn.GetComponent<SpriteRenderer>().flipX);

                recordDelay = recordTime + 0.03f;
                recordTime = 0f;
            }
        }
    }

    void PlayPlayer()
    {
        if (isFuturePlay)
        {
            playTime = Time.time;
            playTime = (float)Math.Round(playTime * 100) / 100;
            if (playTime >= playDelay)
            {
                Vector2 futurePlayerXY = new Vector2(RecordNumber_XY[playframe].x, RecordNumber_XY[playframe].y);
                Sprite futurePlayerSprite = RecordNumber_Sprite[playframe];
                bool futurePlayerSpriteFlipX = RecordNumber_SpriteFlipX[playframe];

                recordedPlayer.position = futurePlayerXY;
                recordedPlayer.GetComponent<SpriteRenderer>().sprite = futurePlayerSprite;
                recordedPlayer.GetComponent<SpriteRenderer>().flipX = futurePlayerSpriteFlipX;

                playDelay = playTime + 0.03f;
                playTime = 0f;
                //Debug.Log(rm.RecordNumber_XY[playScene - 1].XY.Count + " , " + playframe);
                if (RecordNumber_XY.Count - 1 > playframe)
                    playframe++;
                else
                    playframe = 0;
            }
        }
        else
        {
            recordedPlayer.position = new Vector3(-20, 10, 0);
            playTime = 0f;
            playDelay = 0.03f;
            playframe = 0;
        }
    }

    public void ResetPlayer()
    {
        isSleep = false;
        isFuturePlay = true;

        //능력 시간 초기화
        currentTime = abilityDefaultTime;

        // DEAD에서 바꾼다.
        PlayerController.Instance.playerState = PlayerState.NORMAL;

        // 시계 UI 사라지게
        DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0f, 2f).OnComplete(() =>
        {
            clockUI.SetActive(false);
        });

        // 파란색 닭에서 다시 기본 닭으로!
        playerAn.GetComponent<SpriteRenderer>().color = Color.white;

        // 카메라 글리치 효과 초기화
        GlitchEffect.Instance.colorIntensity = 0;
        GlitchEffect.Instance.flipIntensity = 0;
        GlitchEffect.Instance.intensity = 0;

        //trail Effect 남기고
        effect.transform.SetParent(null);
        effect.transform.position = playerRb.transform.position;

        // 자고있는 닭 상태로 다시 돌아간다.
        playerRb.transform.position = sleepPlayer.position;
        playerAn.GetComponent<SpriteRenderer>().flipX = sleepPlayer.GetComponent<SpriteRenderer>().flipX;
        sleepPlayer.GetComponent<SleepingPlayer>().BubbleAwake();
        sleepPlayer.position = new Vector3(-18, 10, 0);
        playerRb.GetComponent<Rigidbody2D>().simulated = true;

        // 현재로 돌아가는 화면 이펙트
        abilityEffectAnim.SetTrigger("OrangeT");

        // 트랩들 리셋
        foreach (ResetAbleTrap trap in traps)
        {
            trap.Reset();
        }

        // 소리 바꾸고
        GameManager.Instance.SetAudio(audioSource, Audio_presentEnter, 1);
        GameManager.Instance.SetAudio(bgAudioSource, GameManager.Instance.defaultBGM, 1, true);
    }

    IEnumerator Clock()
    {
        yield return new WaitForSeconds(1);

        while(currentTime > 0)
        {
            currentTime--;
            yield return new WaitForSeconds(1);
            if(PlayerController.Instance.playerState == PlayerState.DEAD)//만약 죽은상태라면
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
