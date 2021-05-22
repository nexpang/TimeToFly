using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private Vector3 lastPos = Vector3.zero;

    [HideInInspector] public float currentMoveS; // 현재 속도를 감지함.

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    private float speed = 1f;
    public float _speed {
        get { return speed; }
        set { speed = value; }
    }

    public bool isGround;
    public bool isStun;
    public bool isPressJump; // 오로지 나는 사운드를 위해 만들어진 변수..

    public PlayerState playerState = PlayerState.NORMAL;

    private Rigidbody2D rb;
    private Animator an;
    private SpriteRenderer sr;
    [HideInInspector]
    public Ability_FutureCreate ability1;

    [SerializeField] GameObject[] abilitys;
    public int abilityNumber = 0;

    [SerializeField] private Transform playerAnim = null;
    private Transform playerAbility = null;

    [Header("오디오 클립")]
    AudioSource audioSource = null;
    [SerializeField] AudioSource bgAudioSource = null;
    [SerializeField] AudioClip Audio_playerJump = null;
    [SerializeField] AudioClip Audio_playerDead = null;
    [SerializeField] AudioClip Audio_playerWing = null;
    public AudioClip getAudioWing() => Audio_playerWing; // 잠시 get좀 하겠습니다. PlayerAnimation.cs

    [Header("데스 스크린")]
    [SerializeField] Image deathScreen = null;
    [SerializeField] ParticleSystem featherEffect = null;
    [SerializeField] Texture[] featherTextures = null;

    private void Awake()
    {
        Instance = this;

        PlayerAbilitySet();
        rb = GetComponent<Rigidbody2D>();
        an = playerAnim.GetComponent<Animator>();
        sr = playerAnim.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        playerAbility = GameObject.FindGameObjectWithTag("Ability").transform;
        playerState = PlayerState.NORMAL;
    }

    private void Start()
    {
        ability1 = FindObjectOfType<Ability_FutureCreate>();
    }

    private void Update()
    {
        PlayerMove(Movetype.JUMP);
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.4f), 0.3f, 1 << LayerMask.NameToLayer("Ground"));

        AnimParametersSet();
        AbilityKey();
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

        if(type == Movetype.JUMP)
        {
            // 점프
            if (PlayerInput.Instance.KeyJump && isGround && Time.timeScale != 0)
            {
                GameManager.Instance.SetAudio(audioSource, Audio_playerJump, 0.7f, false);

                rb.velocity = Vector2.zero;
                float jS = speed != 1f ? speed * 0.8f : 1f;
                rb.AddForce(Vector2.up * jumpSpeed * jS); // speed 능력2를 구현하기위함
                an.SetTrigger("jumpT");
                isPressJump = true;
            }
        }
        else if(type == Movetype.MOVE)
        {
            // 이동
            float axis = PlayerInput.Instance.KeyHorizontalRaw * moveSpeed * speed * speed * Time.fixedDeltaTime; // speed 능력2를 구현하기위함
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

    void AnimParametersSet()
    {
        an.SetBool("jump", !isGround);
        an.SetFloat("ySpeed", rb.velocity.y / _speed);

        // 점프키를 눌러서 점프하고 3.5 미만이거나, 그냥 -4 미만이거나
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
        //abilityNumber  0이면 기상이, 1이면 시한이, 2이면 동진이, 3이면 지향이, 4면 소전이

        playerAnim.GetComponent<PlayerSprites>().targetSheet = abilityNumber;

        abilitys[0].SetActive(abilityNumber == 1);
        abilitys[1].SetActive(abilityNumber == 2);
        abilitys[2].SetActive(abilityNumber == 3);
        abilitys[3].SetActive(abilityNumber == 4);
        abilitys[4].SetActive(abilityNumber == 0);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(0, -0.4f), 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("DEADABLE"))
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
        if(collision.collider.CompareTag("DEADABLE"))
        {
            GameOver();
        }
    }

    [ContextMenu("게임오버")]
    public void GameOver()
    {
        if (playerState != PlayerState.DEAD)
        {
            playerState = PlayerState.DEAD;

            GameManager.Instance.SetAudioImmediate(audioSource, Audio_playerDead, 0.8f, false);

            an.SetTrigger("dead");
            RealDeath();
        }
    }

    [ContextMenu("떨어져서 게임오버")]
    public void FallGameOver()
    {
        if (playerState != PlayerState.DEAD)
        {
            playerState = PlayerState.DEAD;

           GameManager.Instance.SetAudioImmediate(audioSource, Audio_playerDead, 0.8f, false);

            rb.simulated = false;
            an.SetTrigger("falldead");
            RealDeath();
        }
    }

    private void RealDeath()
    {
        if (ability1 != null)
        {
            if (ability1.IsSleep()) return;
        }
        deathScreen.gameObject.SetActive(true);
        deathScreen.color = new Color(0, 0, 0, 0);
        deathScreen.DOFade(0.75f, 1);
        bgAudioSource.DOPitch(0, 2);
        featherEffect.Play();
        featherEffect.GetComponent<ParticleSystemRenderer>().material.mainTexture = featherTextures[abilityNumber];
        playerAnim.GetComponent<SpriteRenderer>().sortingOrder = 17;
    }
}