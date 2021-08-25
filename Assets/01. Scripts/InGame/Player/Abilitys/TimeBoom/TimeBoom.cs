using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeBoom : MonoBehaviour
{
    [SerializeField] GameObject ability = null;
    [SerializeField] GameObject explosionEffect = null;

    [SerializeField] Ability_TimeBomb sATimeBoom = null;

    [SerializeField] GameObject tileMap = null;

    [SerializeField] float defaultEffectCool = 1f;
    float effectCool = 1f;
    bool coolStart = false;

    public bool isAlreadyExplosion = false;


    [SerializeField] Animator animator = null;
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField]Sprite defaultSprite = null;
    [SerializeField]Sprite warningEffect = null;

    [SerializeField] float explosionRadius = 1f;

    [Header("¿Àµð¿À Å¬¸³")]
    [SerializeField] AudioSource SFXSpeaker = null;
    [SerializeField] AudioSource SFXSpeaker2 = null;
    [SerializeField] AudioClip Audio_TimeBomb = null;
    [SerializeField] AudioClip Audio_BombTimer = null;

    private void OnEnable()
    {
        animator.enabled = true;
        coolStart = false;
        isAlreadyExplosion = false;
    }
    
    private void Update()
    {
        if (isAlreadyExplosion)
            Boom();
        if(sATimeBoom.isAnimationPlaying == false && !coolStart)
        {
            coolStart = true;
            effectCool = defaultEffectCool;
            StartCoroutine(ShowEffect());
        }
        if (transform.parent == null && transform.position.y < -20)
        {
            Debug.Log("»ç¶óÁü");
            transform.SetParent(ability.transform);
            if(GetComponent<Rigidbody2D>() != null)
                Destroy(GetComponent<Rigidbody2D>());
            GetComponent<CircleCollider2D>().enabled = false;
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.collider.CompareTag("Player")) return;
        Boom();
        Debug.Log("Æø¹ß");
    }

    void Boom()
    {
        GameManager.Instance.SetAudio(SFXSpeaker, Audio_TimeBomb, 1, false);

        transform.SetParent(ability.transform);
        if (GetComponent<Rigidbody2D>() != null)
            Destroy(GetComponent<Rigidbody2D>());
        GetComponent<CircleCollider2D>().enabled = false;

        isAlreadyExplosion = false;
        explosionEffect.SetActive(true);
        explosionEffect.transform.position = transform.position;
        RaycastHit2D[] raycastHit2D = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.up);
        foreach (var hit in raycastHit2D)
        {
            if((hit.collider.CompareTag("DEADABLE") || hit.collider.CompareTag("TrapTrigger") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy")) && hit.collider.gameObject != tileMap)
            {
                Debug.Log(hit.collider.name + "À» ¾ø¾Ú");
                SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();
                Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D>();
                
                if(rb != null)
                {
                    rb.simulated = false;
                }

                if (sr != null)
                {
                    hit.collider.enabled = false;
                    sr.DOColor(Color.cyan, 1).OnComplete(() =>
                    {
                        sr.DOFade(0, 1).OnComplete(() =>
                        {

                            hit.collider.gameObject.SetActive(false);
                        });
                    });
                }
                else
                    hit.collider.gameObject.SetActive(false);
            }
            else if(hit.collider.CompareTag("Boss"))
            {
                Boss boss =  hit.collider.GetComponent<Boss>();
                if(boss)
                {
                    boss.BossHitBoom();
                }
                else
                {
                    boss = hit.collider.transform.parent.GetComponent<Boss>();
                    if (boss)
                    {
                        boss.BossHitBoom();
                    }
                }
            }
            Debug.Log(hit.collider.tag);
        }

        gameObject.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    IEnumerator ShowEffect()
    {
        animator.enabled = false;
        //Debug.Log("¿ö´×");
        yield return new WaitForSeconds(effectCool);
        spriteRenderer.sprite = warningEffect;
        GameManager.Instance.SetAudioImmediate(SFXSpeaker2, Audio_BombTimer, 1, false);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = defaultSprite;
        effectCool -= 0.08f;
        if (effectCool < 0.1f)
            effectCool = 0.05f;
        StartCoroutine(ShowEffect());
    }
}
