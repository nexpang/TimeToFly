using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ablility_Teleport : Ability, IAbility
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
    //=================================여기까지 시계 이펙트다.=============

    [Header("능력 별 변수들(능력)")]
    [SerializeField] Animator abilityEffectAnim = null;
    [SerializeField] float abilityDefaultTime = 15;
    public float currentTime = 15;

    [SerializeField] Transform defaultParent = null;
    [SerializeField] GameObject joystick = null;
    [SerializeField] GameObject joystickBack = null;
    private RectTransform joystickRect = null;
    private bool isUsing = false;
    [SerializeField] GameObject player = null;
    [SerializeField] Transform playerPos = null;
    [SerializeField] GameObject teleportPosObj = null;
    private Transform teleportPosObjTrans = null;
    [SerializeField] Sprite joystickSpr = null;

    [SerializeField] float timeSlow = 4f;
    [SerializeField] float minusTimeForS = 10f;

    private float fSqr = 0f;
    [SerializeField]
    private float radius = 1f;

    [SerializeField, Range(1f, 10f)]
    private float teleportPower = 10f;
    private Vector2 teleportPos;

    [Header("사운드 이펙트")]
    [SerializeField] AudioSource audioClockSoundSource = null;
    [SerializeField] AudioClip Audio_futureEnter = null;
    [SerializeField] AudioClip Audio_presentEnter = null;
    [SerializeField] AudioClip Audio_tik = null;
    [SerializeField] AudioClip Audio_tok = null;

    new void Start()
    {
        base.Start();
        joystickRect = joystick.GetComponent<RectTransform>();
        teleportPosObjTrans = teleportPosObj.GetComponent<Transform>();
    }

    public void OnAbility()
    {
        if (abilityCurrentCoolDown > 0)
        {
            abilityCooldownCircle.DOComplete();
            abilityCooldownCircle.color = Color.red;
            abilityCooldownCircle.DOColor(new Color(0, 0, 0, 0.75f), 0.5f);
            return;
        }// 쿨타임이 아직 안됐다.

        //능력 사용
        isUsing = true;
        joystickBack.SetActive(true);
        teleportPosObj.SetActive(true);
        joystick.transform.SetParent(joystickBack.transform);
        joystickRect.transform.localPosition = Vector2.zero;
        abilityBtn.sprite = joystickSpr;

        GameManager.Instance.SetAudio(audioSource, Audio_futureEnter, 1, false);
        abilityEffectAnim.SetTrigger("BlueT");

        Time.timeScale = 1f / timeSlow;
        GameManager.Instance.TimerScale = 1f / (minusTimeForS * timeSlow);
        PlayerController.Instance._speed = 0f;

        clockUI.SetActive(true);
        DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0.75f, 2f).SetUpdate(true);
        GlitchEffect.Instance.colorIntensity = 0.100f;
        GlitchEffect.Instance.flipIntensity = 0.194f;
        GlitchEffect.Instance.intensity = 0.194f;
        StartCoroutine(Clock());
    }
    new void Update()
    {
        base.Update();

        if (isUsing)
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
            abilityParticle.gameObject.SetActive(false);
        }

        Using();

        if (PlayerController.Instance.playerState == PlayerState.DEAD && isUsing)//만약 죽은상태라면
        {
            ResetPlayer();
            StopCoroutine(Clock()); // 시계를 정지시킨다.
        }
    }

    void Using()
    {

        if (isUsing)
        {
            if (!PlayerInput.Instance.KeyAbilityHold)
            {
                Debug.Log("능력 뿌슝빠슝");
                ResetPlayer();
                StopCoroutine(Clock());
                playerPos.position = teleportPos;
            }
            else
            {
                SetTeleportPos();
            }
        }
    }

    private void SetTeleportPos()
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPos.z = 0f;
        Vector3 firstJPos = joystickBack.transform.position;
        firstJPos.z = 0f;
        Vector3 vec = (touchPos - firstJPos).normalized;
        float dist = Vector2.Distance(touchPos, joystickBack.transform.position);

        if (dist < radius)
        {
            fSqr = dist;
        }
        else
        {
            fSqr = radius;
        }

        joystick.transform.position = joystickBack.transform.position + vec * fSqr;

        teleportPos = playerPos.position + vec * (fSqr * teleportPower);

        teleportPosObjTrans.position = teleportPos;

    }

    public void ResetPlayer()
    {
        playerPos.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //능력 중단
        player.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        Time.timeScale = 1f;
        GameManager.Instance.TimerScale = 1f;
        PlayerController.Instance._speed = 1;

        currentTime = abilityDefaultTime;

        DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0f, 2f).OnComplete(() => clockUI.SetActive(false));

        GlitchEffect.Instance.colorIntensity = 0;
        GlitchEffect.Instance.flipIntensity = 0;
        GlitchEffect.Instance.intensity = 0;

        GameManager.Instance.SetAudio(audioSource, Audio_presentEnter, 1);
        abilityEffectAnim.SetTrigger("OrangeT");


        isUsing = false;

        joystickRect.localPosition = Vector2.zero;
        abilityBtn.sprite = abilityBtnSpr;

        joystick.transform.SetParent(defaultParent);
        joystickBack.SetActive(false);

        teleportPosObjTrans.position = Vector3.zero;
        teleportPosObj.SetActive(false);
        //StopCoroutine(Clock());
    }

    IEnumerator Clock()
    {
        yield return new WaitForSecondsRealtime(1);

        while (currentTime > 0)
        {
            currentTime--;
            yield return new WaitForSecondsRealtime(1);
            if (PlayerController.Instance.playerState == PlayerState.DEAD || !isUsing)//만약 죽은상태라면
            {
                break; // WHILE문 나가기
            }
        }
    }
}
