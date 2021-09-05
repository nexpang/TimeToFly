using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ability_FutureCreate : Ability, IAbility
{
    [Header("�ɷ� �� ������")]
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

    [Header("ȿ����")]
    [SerializeField] AudioSource bgAudioSource = null;
    [SerializeField] AudioSource audioClockSoundSource = null;
    [SerializeField] AudioClip Audio_futureEnter = null;
    [SerializeField] AudioClip Audio_presentEnter = null;
    [SerializeField] AudioClip Audio_tik = null;
    [SerializeField] AudioClip Audio_tok = null;
    [SerializeField] AudioClip Audio_futureBGM = null;
    [SerializeField] AudioClip Audio_futureBGM2 = null;

    [Header("�̷� ���� �÷��̾� ���")]
    public List<Vector2> RecordNumber_XY = new List<Vector2>();
    public List<Sprite> RecordNumber_Sprite = new List<Sprite>();
    public List<bool> RecordNumber_SpriteFlipX = new List<bool>();
    [SerializeField] private GameObject sleepPlayer = null;

    private bool isRecording = false;
    private float recordTime = 0f;
    private float recordDelay = 0.03f;

    [Header("�̷� ���� �÷��̾� �÷���")]
    [SerializeField]
    private bool isFuturePlay = false;

    private float playTime = 0f;
    private float playDelay = 0.03f;
    private int playframe = 0;

    private PlayerController playerRb = null;
    private PlayerAnimation playerAn = null;
    [SerializeField] private GameObject recordedPlayer = null;

    // ���� ����
    ResetAbleTrap[] traps = null;


    public TimerMinusEffect timerMinusEffect;

    new void Start()
    {
        base.Start();
        playerRb = FindObjectOfType<PlayerController>();
        playerAn = FindObjectOfType<PlayerAnimation>();
        currentTime = abilityDefaultTime;
        traps = FindObjectsOfType<ResetAbleTrap>();

        sleepPlayer = Instantiate(sleepPlayer, new Vector2(-18, 10), Quaternion.identity);
        recordedPlayer = Instantiate(recordedPlayer, new Vector2(-20, 10), Quaternion.identity);
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
        }// ��Ÿ���� ���� �ȵƴ�.

        abilityCurrentCoolDown = abilityCooldown;
        abilityCurrentCoolDownTime = Time.time; // ��Ÿ�� �����ְ�

        clockUI.SetActive(true); // �ð� UI�� ���ش�.
        tween.Kill(); // Ʈ�� �ʱ�ȭ
        // �ð� ���İ� ��Ʈ������ �÷��ְ�
        tween = DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0.6f, 2f);
        playerAn.GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, 1); // �÷��̾ �Ķ�������

        // �۸�ġ ����Ʈ ���ְ�
        GlitchEffect.Instance.colorIntensity = 0.100f;
        GlitchEffect.Instance.flipIntensity = 0.194f;
        GlitchEffect.Instance.intensity = 0.194f;

        // Ÿ�̸� ������
        GameManager.Instance.timerScale = 1f / 1.5f;
        StartCoroutine(timerMinusEffect.OnEffect(10));
        GameManager.Instance.timer -= 10;

        // �ð� �ʰ� ���۵ȴ�.
        StartCoroutine(Clock());

        // ��� �÷��̾ ������ش�.
        sleepPlayer.transform.position = transform.position;
        sleepPlayer.GetComponent<SpriteRenderer>().flipX = playerAn.GetComponent<SpriteRenderer>().flipX;

        // �̷� ����Ʈ ��������ش�.
        abilityEffectAnim.SetTrigger("BlueT");

        // �̷����� Trail
        abilityParticle.gameObject.SetActive(true);
        abilityParticle.Play();
        effect.transform.SetParent(this.transform);
        effect.time = 0;
        effect.transform.localPosition = Vector3.zero;
        effect.time = 10;

        // �̷� ���� ȿ����
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

        isAbilityEnable = true;
    }

    new void Update()
    {
        base.Update();
        //clockUIFill.fillAmount = 1 - (currentTime / abilityDefaultTime);

        if (isAbilityEnable)
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

            if (GameManager.Instance.player.playerState != PlayerState.DEAD)
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

        if (GameManager.Instance.player.playerState == PlayerState.DEAD)//���� �������¶��
        {
            StopCoroutine(Clock()); // �ð踦 ������Ų��.
        }
        
    }

    void RecordPlayer()
    {
        if (isAbilityEnable)
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

                recordedPlayer.transform.position = futurePlayerXY;
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
            recordedPlayer.transform.position = new Vector3(-20, 10, 0);
            playTime = 0f;
            playDelay = 0.03f;
            playframe = 0;
        }
    }

    public void ResetPlayer()
    {
        isAbilityEnable = false;
        isFuturePlay = true;

        //�ɷ� �ð� �ʱ�ȭ
        currentTime = abilityDefaultTime;

        // DEAD���� �ٲ۴�.
        GameManager.Instance.player.playerState = PlayerState.NORMAL;

        // �ð� UI �������
        DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0f, 2f).OnComplete(() =>
        {
            clockUI.SetActive(false);
        });

        // �Ķ��� �߿��� �ٽ� �⺻ ������!
        playerAn.GetComponent<SpriteRenderer>().color = Color.white;

        // ī�޶� �۸�ġ ȿ�� �ʱ�ȭ
        GlitchEffect.Instance.colorIntensity = 0;
        GlitchEffect.Instance.flipIntensity = 0;
        GlitchEffect.Instance.intensity = 0;

        //trail Effect �����
        effect.transform.SetParent(null);
        effect.transform.position = playerRb.transform.position;

        // �ڰ��ִ� �� ���·� �ٽ� ���ư���.
        playerRb.transform.position = sleepPlayer.transform.position;
        playerAn.GetComponent<SpriteRenderer>().flipX = sleepPlayer.GetComponent<SpriteRenderer>().flipX;
        sleepPlayer.GetComponent<SleepingPlayer>().BubbleAwake();
        sleepPlayer.transform.position = new Vector3(-18, 10, 0);
        playerRb.GetComponent<Rigidbody2D>().simulated = true;

        // ����� ���ư��� ȭ�� ����Ʈ
        abilityEffectAnim.SetTrigger("OrangeT");

        // Ʈ���� ����
        foreach (ResetAbleTrap trap in traps)
        {
            trap.Reset();
        }

        // �Ҹ� �ٲٰ�
        GameManager.Instance.SetAudio(audioSource, Audio_presentEnter, 1);

        if (!GameManager.Instance.isBossStart)
        {
            GameManager.Instance.SetAudio(bgAudioSource, GameManager.Instance.curChapterInfo.chapterBGM, GameManager.Instance.defaultBGMvolume, true);
        }
        else
        {
            GameManager.Instance.SetAudio(bgAudioSource, ObjectManager.Instance.soundData.BGM_Boss, GameManager.Instance.defaultBGMvolume, true);
        }
    }

    IEnumerator Clock()
    {
        yield return new WaitForSeconds(1);

        while(currentTime > 0)
        {
            currentTime--;
            yield return new WaitForSeconds(1);
            if(GameManager.Instance.player.playerState == PlayerState.DEAD)//���� �������¶��
            {
                break; // WHILE�� ������
            }
        }

        if (GameManager.Instance.player.playerState != PlayerState.DEAD)
        {
            ResetPlayer();
        }
    }

    private void OnDisable() // Ʃ�丮��� �ڵ�
    {
        base.OnEnable();
        if (recordedPlayer)
        {
            recordedPlayer.transform.position = new Vector3(-20, 10, 0);
        }
    }
}
