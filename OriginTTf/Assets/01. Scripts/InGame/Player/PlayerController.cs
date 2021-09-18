using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Events;

enum Movetype
{
    MOVE,
    JUMP
};

public enum PlayerState
{
    NORMAL,
    DEAD
}

public enum Chickens
{
    WHITE,
    BROWN,
    BLUE,
    PINK,
    SKYBLUE
}

public class PlayerController : MonoBehaviour
{
    private Vector3 lastPos = Vector3.zero;

    [HideInInspector] public float currentMoveS; // ���� �ӵ��� ������.

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    private float speed = 1f;
    public float _speed {
        get { return speed; }
        set { speed = value; }
    }

    [HideInInspector] public bool isGround;
    [HideInInspector] public bool isAnimationStun;
    [HideInInspector] public bool isPressJump; // ������ ���� ���带 ���� ������� ����..
    public bool reverseKey = false; // �̰� true�� �ϸ� �̵�Ű�� �ݴ�� �ٲ��.

    private bool isStun;
    private bool isSlide;

    public PlayerState playerState = PlayerState.NORMAL;

    private Rigidbody2D rb;
    private Animator an;
    private SpriteRenderer sr;

    public Ability[] abilitys;
    public int abilityNumber = 0;

    private Transform playerAnimObj = null;
    private Transform playerAbility = null;

    [Header("����� Ŭ��")]
    AudioSource audioSource = null;
    [SerializeField] AudioSource bgAudioSource = null;
    [SerializeField] AudioClip Audio_playerJump = null;
    [SerializeField] AudioClip Audio_playerDead = null;
    [SerializeField] AudioClip Audio_playerWing = null;
    public AudioClip Audio_playerHeartEat = null;
    public AudioClip Audio_playerTimeEat = null;
    public AudioClip getAudioWing() => Audio_playerWing; // ��� get�� �ϰڽ��ϴ�. PlayerAnimation.cs

    [Header("���� ��ũ��")]
    [SerializeField] public Image deathScreen = null;
    [SerializeField] ParticleSystem featherEffect = null;
    [SerializeField] Texture[] featherTextures = null;

    [Header("��Ÿ�� �̹���")]
    [SerializeField] GameObject CircleCoolTime;
    [SerializeField] Image CircleValue;
    [HideInInspector] public bool isTeleportAble = true;
    public ItemBlock collisionBlock;

    [Header("������ ����Ʈ")]
    [SerializeField] GameObject effect_1up;
    [SerializeField] TimerMinusEffect timerMinusEffect;

    [Header("������")]
    [SerializeField]
    ADs ads;

    //TO DO : Ʃ�丮��
    // abilityNumber, ������ ��������Ʈ, �ɷ� SetActive, ��������Ʈ ��Ʈ���� �ٲ������

    private void Awake()
    {       
        abilityNumber = SecurityPlayerPrefs.GetInt("inGame.saveCurrentChickenIndex", -1);
        if(SecurityPlayerPrefs.GetInt("inGame.saveMapid", 0) == 0)
        {
            abilityNumber = 0;
        }

        if(SceneController.isSavePointChecked)
        {
            transform.position = SceneController.savePointPos;
        }

        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        playerState = PlayerState.NORMAL;
    }

    private void Start()
    {
        playerAnimObj = GameManager.Instance.playerAnimObj;
        an = playerAnimObj.GetComponent<Animator>();
        sr = playerAnimObj.GetComponent<SpriteRenderer>();
        PlayerAbilitySet();
    }

    private void Update()
    {
        PlayerMove(Movetype.JUMP);
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.4f), 0.3f, 1 << LayerMask.NameToLayer("Ground"));

        AnimParametersSet();
        AbilityKey();

        if(rb.velocity.y > 0)
        {
            if(collisionBlock != null)
            {
                collisionBlock.JumpDetect();
            }
        }
    }

    void FixedUpdate()
    {
        PlayerMove(Movetype.MOVE);

        currentMoveS = Mathf.Abs(transform.position.x - lastPos.x);
        lastPos = transform.position;
    }

    void PlayerMove(Movetype type)
    {
        if (isStun) return;
        if (isAnimationStun) return;

        if(type == Movetype.JUMP)
        {
            // ����
            if (PlayerInput.Instance.KeyJump && isGround && Time.timeScale != 0)
            {
                GameManager.Instance.SetAudio(audioSource, Audio_playerJump, 0.7f, false);

                rb.velocity = Vector2.zero;
                float jS = speed != 1f ? speed * 0.8f : 1f;
                rb.AddForce(Vector2.up * jumpSpeed * jS); // speed �ɷ�2�� �����ϱ�����
                an.SetTrigger("jumpT");
                isPressJump = true;
            }
        }
        else if(type == Movetype.MOVE)
        {
            // �̵�
            float axis = PlayerInput.Instance.KeyHorizontalRaw * moveSpeed * speed * speed * Time.fixedDeltaTime; // speed �ɷ�2�� �����ϱ�����
            float simpleAxis = Mathf.Round(axis * 1000) / 1000;
            transform.Translate(new Vector2(simpleAxis, 0));

            if(PlayerInput.Instance.KeyHorizontalRaw!= 0)
                sr.flipX = (PlayerInput.Instance.KeyHorizontalRaw < 0);
            if (axis != 0)
            {
                an.SetBool("walk", true);
            }
            else an.SetBool("walk", false);
        }
    }

    public IEnumerator MoveSlide()
    {
        isSlide = true;
        float setKetHorizontalRaw = PlayerInput.Instance.KeyHorizontalRaw;
        while (isSlide)
        {
            if (Mathf.Abs(PlayerInput.Instance.KeyHorizontalRaw) >= Mathf.Abs(setKetHorizontalRaw) || (PlayerInput.Instance.KeyHorizontalRaw<0 && setKetHorizontalRaw>0) || (PlayerInput.Instance.KeyHorizontalRaw > 0 && setKetHorizontalRaw < 0))
            {
                setKetHorizontalRaw = PlayerInput.Instance.KeyHorizontalRaw;
            }

            float axis = setKetHorizontalRaw * moveSpeed * speed * speed * Time.fixedDeltaTime; // speed �ɷ�2�� �����ϱ�����
            float simpleAxis = Mathf.Round(axis * 1000) / 1000;
            transform.Translate(new Vector2(simpleAxis, 0));

            yield return new WaitForFixedUpdate();
        }
    }

    public void MoveSlideStop()
    {
        isSlide = false;
    }

    void AnimParametersSet()
    {
        an.SetBool("jump", !isGround);

        an.SetFloat("ySpeed", (_speed != 0) ? rb.velocity.y / _speed : 0);

        // ����Ű�� ������ �����ϰ� 3.5 �̸��̰ų�, �׳� -4 �̸��̰ų�
        if(((isPressJump && an.GetFloat("ySpeed") < 3.5f) || an.GetFloat("ySpeed") < -4f) && !isGround && playerState != PlayerState.DEAD)
        {
            GameManager.Instance.SetAudio(audioSource, Audio_playerWing, 0.8f, true);
        }else if(audioSource.clip == Audio_playerWing)
        {
            GameManager.Instance.SetAudio(audioSource, null, 1, false);
        }

        if (rb.velocity.y < -4f) an.SetBool("falling", true);
        if (rb.velocity.y < -15f) an.SetBool("land", true);

        an.SetBool("isDead", playerState == PlayerState.DEAD);
    }

    void PlayerAbilitySet()
    {
        //abilityNumber  0�̸� �����, 1�̸� ������, 2�̸� ������, 3�̸� ������, 4�� ������

        playerAnimObj.GetComponent<PlayerSprites>().targetSheet = abilityNumber;
        playerAbility = abilitys[abilityNumber].transform;

        for (int i = 0; i< abilitys.Length; i++)
        {
            abilitys[i].gameObject.SetActive(abilityNumber == i);
        }
    }

    public void PlayerBarrierSet()
    {
        playerAbility = abilitys[5].transform;

        for (int i = 0; i < abilitys.Length; i++)
        {
            abilitys[i].gameObject.SetActive(5 == i);
        }
    }

    //number�� �ɷ��� ������ (Ʃ�丮�� ����)
    public void PlayerAbilitySet(int number)
    {
        abilityNumber = number;
        playerAnimObj.GetComponent<PlayerSprites>().targetSheet = number;
        playerAbility = abilitys[number].transform;

        for (int i = 0; i < abilitys.Length; i++)
        {
            abilitys[i].gameObject.SetActive(false);
        }

        abilitys[abilityNumber].gameObject.SetActive(true);
    }

    void AbilityKey()
    {
        if (playerState == PlayerState.DEAD) return;

        if(PlayerInput.Instance.KeyAbility)
        {
            IAbility ability = playerAbility.GetComponent<IAbility>();

            ability.OnAbility();
        }
    }

    public void PlayerActCoolTimeSet(float timeSec, UnityAction action)
    {
        StartCoroutine(ActCooltime(timeSec, action));
    }

    public void PlayerActCoolTimeStop()
    {
        coolTime = -200f;
        CircleCoolTime.SetActive(false);
        CircleValue.fillAmount = 1;
    }

    private float coolTime = 0f;
    IEnumerator ActCooltime(float timeSec, UnityAction action)
    {
        coolTime = 0f;
        CircleCoolTime.SetActive(true);
        while (coolTime < timeSec)
        {
            if(coolTime < -100f)
            {
                coolTime = 0f;
                yield break;
            }

            coolTime += Time.deltaTime;
            CircleValue.fillAmount = 1 - (coolTime / timeSec);
            yield return null;
        }
        CircleCoolTime.SetActive(false);
        CircleValue.fillAmount = 1;
        coolTime = 0f;
        action.Invoke();
    }

    public void SetStun(float time)
    {
        StartCoroutine(ISetStun(time));
    }

    IEnumerator ISetStun(float time)
    {
        isStun = true;
        an.SetBool("walk", false);
        yield return new WaitForSeconds(time);
        isStun = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(0, -0.4f), 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.Instance.isGameStart) return;

        if(collision.CompareTag("DEADABLE") || collision.CompareTag("Boss"))
        {
            GameOver();
        }
        else if(collision.CompareTag("FALLINGABLE"))
        {
            FallGameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.Instance.isGameStart) return;

        if (collision.collider.CompareTag("DEADABLE") || collision.collider.CompareTag("Boss"))
        {
            GameOver();
        }
    }

    [ContextMenu("���ӿ���")]
    public void GameOver()
    {
        if (GameManager.Instance.isCleared) return;

        if (playerState != PlayerState.DEAD)
        {
            Ability_Barrier ability = (Ability_Barrier)abilitys[5];
            if(ability.enabled)
            {
                if (ability.isAbilityEnable)
                {
                    ability.ResetPlayer();
                    GetComponent<Collider2D>().enabled = false;
                    GetComponent<Collider2D>().enabled = true;
                    return;
                }
                else if(ability.isInvincible)
                {
                    GetComponent<Collider2D>().enabled = false;
                    GetComponent<Collider2D>().enabled = true;
                    return;
                }
            }
            playerState = PlayerState.DEAD;

            GameManager.Instance.SetAudioImmediate(audioSource, Audio_playerDead, 0.8f, false);

            an.SetTrigger("dead");
            DeathScreen();
        }
    }

    [ContextMenu("�������� ���ӿ���")]
    public void FallGameOver()
    {
        if (GameManager.Instance.isCleared) return;

        if (playerState != PlayerState.DEAD)
        {
            playerState = PlayerState.DEAD;

           GameManager.Instance.SetAudioImmediate(audioSource, Audio_playerDead, 0.8f, false);

            rb.simulated = false;
            an.SetTrigger("falldead");
            DeathScreen();
        }
    }

    public void TimeOver()
    {
        if (GameManager.Instance.isCleared) return;

        if (playerState != PlayerState.DEAD)
        {
            playerState = PlayerState.DEAD;

            GameManager.Instance.SetAudioImmediate(audioSource, Audio_playerDead, 0.8f, false);

            an.SetTrigger("dead");
            DeathScreen(true);
        }
    }

    private void DeathScreen(bool timeOver = false)
    {
        if (timeOver)
        {
            GameManager.Instance.timeOverUI.gameObject.SetActive(true);
            GameManager.Instance.timeOverUI.DOAnchorPos(new Vector2(0, -400), 1.5f).SetUpdate(true).SetEase(Ease.OutBounce);
        }

        if (abilitys[(int)Chickens.BROWN].enabled)
        {
            if (abilitys[(int)Chickens.BROWN].isAbilityEnable) return;
        }

        deathScreen.gameObject.SetActive(true);
        deathScreen.color = new Color(0, 0, 0, 0);
        deathScreen.DOFade(1f, 1);
        bgAudioSource.DOPitch(0, 2);
        foreach (AudioSource item in GameManager.Instance.SFXSources)
        {
            item.DOComplete();
            item.DOPitch(0, 2);
        }
        featherEffect.Play();
        featherEffect.GetComponent<ParticleSystemRenderer>().material.mainTexture = featherTextures[abilityNumber];
        playerAnimObj.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
        playerAnimObj.GetComponent<SpriteRenderer>().sortingOrder = 17;
    }

    public void GameClear()
    {
        Time.timeScale = 0;
        SceneController.isSavePointChecked = false;
        GameManager.Instance.isCleared = true;
        an.updateMode = AnimatorUpdateMode.UnscaledTime;
        an.SetTrigger("Clear");
        ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_GameClear, 1, true);
        GameManager.Instance.gameClearUI.gameObject.SetActive(true);
        GameManager.Instance.gameClearUI.DOAnchorPos(new Vector2(0, -400), 1.5f).SetUpdate(true).SetEase(Ease.OutBounce).SetDelay(1);
        GlitchEffect.Instance.colorIntensity = 0f;
        GlitchEffect.Instance.flipIntensity = 0f;
        GlitchEffect.Instance.intensity = 0f;
        StartCoroutine(ClearCoroutine());
    }

    IEnumerator ClearCoroutine()
    {
        yield return new WaitForSecondsRealtime(2);
        ads.CallFUllSizeAD();
        yield return new WaitForSecondsRealtime(2);
        DOTween.To(() => GameManager.Instance.bgAudioSource.volume, value => GameManager.Instance.bgAudioSource.volume = value, 0, 1.9f).SetUpdate(true);
        GameManager.Instance.FadeInOut(2, 0, 5, () => 
        {
            SceneController.targetMapId++;
            PoolManager.ResetPool();

            if (GameManager.Instance.curStageInfo.stageId == 0)
            {
                // Ʃ�丮�� �� ĳ���� ����

                SecurityPlayerPrefs.SetInt("inGame.saveCurrentChickenIndex", -1);
                SceneManager.LoadScene("ChickenSelectScene");

                return;
            }
            else if (GameManager.Instance.curStageInfo.stageId == 3)
            {
                // ���� ���� �ƾ�
                SecurityPlayerPrefs.SetInt("inGame.saveMapid", SceneController.targetMapId);
                GameManager.Instance.RemoveRemainChicken(abilityNumber);
                SceneManager.LoadScene("CutScenes");
                return;
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }


    public void EffectShow_1up()
    {
        effect_1up.transform.DOKill();
        effect_1up.transform.localPosition = Vector3.zero;
        effect_1up.SetActive(true);
        effect_1up.GetComponent<RectTransform>().DOAnchorPosY(100f, 1f).OnComplete(()=> {
            effect_1up.SetActive(false);
        });
    }

    public void EffectShow_TimerUp(int timer)
    {
        StartCoroutine(timerMinusEffect.OnEffect(timer, false));
    }
}