using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ability_TeleportRemaked : Ability, IAbility
{
    [Header("�ɷ� �� ������(�𷡽ð�)")]
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
    //=================================������� �ð� ����Ʈ��.=============

    [Header("�ɷ� �� ������(�ɷ�)")]
    [SerializeField] Animator abilityEffectAnim = null;
    [SerializeField] float abilityDefaultTime = 15;
    public float currentTime = 15;

    [SerializeField] GameObject player = null;
    [SerializeField] Transform playerPos = null;
    [SerializeField] GameObject teleportPosObj = null; // -------���� �������� ����.
    [SerializeField] GameObject teleportPosFinalPoint = null; // -------^
    [SerializeField] LineRenderer teleportLine = null;
    Vector3[] teleportWayPoints = new Vector3[2];
    [SerializeField] BoxCollider2D teleportRange = null;
    Vector2 teleportRangePos;

    private Transform teleportPosObjTrans = null;

    [SerializeField] float timeSlow = 4f;
    [SerializeField] float minusTimeForS = 10f;

    [SerializeField] private float teleportDist = 5f;
    private Vector2 teleportPos;
    private bool isSkillActive = false;
    private bool isTeleporting = false;
    [SerializeField] SpriteRenderer teleportRangeSpr = null;

    [Header("���� ����Ʈ")]
    [SerializeField] AudioSource bgAudioSource = null;
    [SerializeField] AudioSource playerAudioSource = null;
    [SerializeField] AudioSource audioClockSoundSource = null;
    [SerializeField] AudioClip Audio_teleport = null;
    [SerializeField] AudioClip Audio_futureEnter = null;
    [SerializeField] AudioClip Audio_presentEnter = null;
    [SerializeField] AudioClip Audio_tik = null;
    [SerializeField] AudioClip Audio_tok = null;

    new void Start()
    {
        teleportPosObjTrans = teleportPosObj.transform;

        teleportRange = GameManager.Instance.curStageInfo.teleportAbleRange;
        teleportRangePos = teleportRange.transform.position;
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

        teleportPos = playerPos.position;
        ShowTeleport();

        //�ɷ� ���
        isAbilityEnable = true;
        isSkillActive = true;
        isTeleporting = false;
        teleportPosObj.SetActive(true);
        teleportPosFinalPoint.SetActive(true);
        teleportLine.gameObject.SetActive(true);

        GameManager.Instance.SetAudio(audioSource, Audio_futureEnter, 1, false);
        playerAudioSource.DOPitch(0.3f, 1);
        abilityEffectAnim.SetTrigger("BlueT");

        Time.timeScale = 1f / timeSlow;
        GameManager.Instance.timerScale = 1f / (minusTimeForS * timeSlow);
        GameManager.Instance.player._speed = 0f;
        teleportRangeSpr.DOKill();
        teleportRangeSpr.DOColor(new Color(1, 1, 1, 1), 0.2f);
        GameManager.Instance.Timer();

        clockUI.SetActive(true);

        abilityParticle.Play();

        tween.Kill();
        tween = DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0.75f, 2f).SetUpdate(true);

        GlitchEffect.Instance.colorIntensity = 0.100f;
        GlitchEffect.Instance.flipIntensity = 0.194f;
        GlitchEffect.Instance.intensity = 0.194f;
        StartCoroutine(Clock());
    }
    new void Update()
    {
        //Debug.Log(minMoveBtnPos.position);
        //Debug.Log(maxMoveBtnPos.position);
        base.Update();

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
                    GameManager.Instance.SetAudio(audioClockSoundSource, Audio_tik, 1, false);
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
        }

        Using();

        if (GameManager.Instance.isBossStage)
        {
            teleportRange.transform.position = Camera.main.transform.position;
        }
        else
        {
            teleportRange.transform.position = teleportRangePos;
        }

        if (GameManager.Instance.player.playerState == PlayerState.DEAD && isAbilityEnable)//���� �������¶��
        {
            ResetPlayer();
            StopCoroutine(Clock()); // �ð踦 ������Ų��.
        }
    }

    void Using()
    {
        if (isAbilityEnable)
        {
            if (!isSkillActive)
            {
                ResetPlayer();
                StopCoroutine(Clock());
                playerPos.DOScale(0, 0.25f).OnComplete(()=>
                {
                    playerPos.position = teleportPos;
                    playerPos.DOScale(1, 0.25f).SetEase(Ease.InOutBack);

                    GameManager.Instance.SetAudio(audioSource, Audio_presentEnter, 1);
                    abilityEffectAnim.SetTrigger("OrangeT");
                    bgAudioSource.volume = GameManager.Instance.defaultBGMvolume;
                }).SetEase(Ease.InOutBack);
                GameManager.Instance.SetAudio(audioClockSoundSource, Audio_teleport, 1, false);
                playerAudioSource.DOPitch(1f, 1);
            }
            else if(isSkillActive && isTeleporting)
            {
                SetTeleportPos();
            }
            if(Input.GetMouseButtonDown(0))
            {
                isTeleporting = true;
            }
            if(isTeleporting && Input.GetMouseButtonUp(0))
            {
                isSkillActive = false;
                isTeleporting = false;
                teleportRangeSpr.DOKill();
                teleportRangeSpr.DOColor(new Color(1, 1, 1, 0), 0.2f);
            }
        }
    }


    private void SetTeleportPos()
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPos.z = 0f;

        Vector3 firstJPos = GameManager.Instance.player.transform.position;
        firstJPos.z = 0f;
        Vector3 vec = (touchPos - firstJPos).normalized;
        float dist = Vector2.Distance(touchPos, firstJPos);

        if (dist > teleportDist)
        {
            dist = teleportDist;
        }


        touchPos = firstJPos + (vec * dist);

        RaycastHit2D[] rh2ds = Physics2D.CircleCastAll(touchPos, 0.25f, Vector2.zero);

        if (rh2ds != null)
        {
            for (int i = 0; i < rh2ds.Length; i++)
            {
                if (rh2ds[i].collider.CompareTag("Ground"))
                {
                    ShowTeleport();
                    return;
                }
            }
        }

        teleportPos = touchPos;

        Vector2 teleportRangeOffset = new Vector2(teleportRange.offset.x + teleportRange.transform.position.x,teleportRange.offset.y + teleportRange.transform.position.y);

        teleportPos.x = Mathf.Clamp(teleportPos.x, teleportRangeOffset.x - teleportRange.size.x / 2, teleportRangeOffset.x + teleportRange.size.x / 2);
        teleportPos.y = Mathf.Clamp(teleportPos.y, teleportRangeOffset.y - teleportRange.size.y / 2, teleportRangeOffset.y + teleportRange.size.y / 2);

        ShowTeleport();

        bgAudioSource.volume = 0;

        float width = teleportLine.startWidth;
        teleportLine.material.mainTextureScale = new Vector2(1f / width, 1.0f);
    }

    private void ShowTeleport()
    {
        teleportPosObjTrans.position = teleportPos;
        teleportPosFinalPoint.transform.position = teleportPosObjTrans.position;
        teleportWayPoints[0] = transform.position;
        teleportWayPoints[1] = teleportPos;
        teleportLine.SetPositions(teleportWayPoints);
    }

    public void ResetPlayer()
    {
        abilityCurrentCoolDown = abilityCooldown;
        abilityCurrentCoolDownTime = Time.time;

        playerPos.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //�ɷ� �ߴ�
        player.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        Time.timeScale = 1f;
        GameManager.Instance.timerScale = 1f;
        GameManager.Instance.player._speed = 1;

        currentTime = abilityDefaultTime;

        tween.Kill();
        tween = DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0f, 2f).OnComplete(() => clockUI.SetActive(false));

        GlitchEffect.Instance.colorIntensity = 0;
        GlitchEffect.Instance.flipIntensity = 0;
        GlitchEffect.Instance.intensity = 0;


        isAbilityEnable = false;

        abilityBtn.sprite = abilityBtnSpr;

        teleportPosObjTrans.position = Vector3.zero;
        teleportPosObj.SetActive(false);
        teleportPosFinalPoint.SetActive(false);
        teleportLine.gameObject.SetActive(false);

        StopCoroutine(Clock());
    }

    IEnumerator Clock()
    {
        yield return new WaitForSecondsRealtime(1);

        while (currentTime > 0 && isAbilityEnable)
        {
            currentTime--;
            if (currentTime == 0)
                currentTime = abilityDefaultTime;
            yield return new WaitForSecondsRealtime(1);
            if (GameManager.Instance.player.playerState == PlayerState.DEAD || !isAbilityEnable)//���� �������¶��
            {
                break; // WHILE�� ������
            }
        }
    }
}
