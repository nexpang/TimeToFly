using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBoom : MonoBehaviour
{
    [SerializeField] GameObject ability = null;
    [SerializeField] GameObject explosionEffect = null;

    [SerializeField] Ability_TimeBomb sATimeBoom = null;

    [SerializeField] float explosionTime = 10f;
    [SerializeField] float defaultEffectCool = 1f;
    float effectCool = 1f;
    bool coolStart = false;
    bool isAlreadyExplosion = false;

    SpriteRenderer spriteRenderer = null;
    [SerializeField]Sprite defaultSprite = null;
    [SerializeField]Sprite warningEffect = null;

    [SerializeField] float explosionRadius = 1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        coolStart = false;
        isAlreadyExplosion = false;
    }

    private void Update()
    {
        if(sATimeBoom.isAnimationPlaying == false && !coolStart)
        {
            coolStart = true;
            Invoke("ExTime", explosionTime);
            effectCool = defaultEffectCool;
            StartCoroutine(ShowEffect());
        }
        if (transform.parent == null && transform.position.y < -20)
        {
            Debug.Log("»ç¶óÁü");
            transform.SetParent(ability.transform);
            Destroy(GetComponent<Rigidbody2D>());
            GetComponent<CircleCollider2D>().enabled = false;
            gameObject.SetActive(false);
        }
    }

    void ExTime()
    {
        if(!isAlreadyExplosion)
            Boom();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.SetParent(ability.transform);
        Destroy(GetComponent<Rigidbody2D>());
        GetComponent<CircleCollider2D>().enabled = false;
        Boom();
        gameObject.SetActive(false);
        Debug.Log("Æø¹ß");
    }

    void Boom()
    {
        isAlreadyExplosion = true;
        explosionEffect.SetActive(true);
        explosionEffect.transform.position = transform.position;
        RaycastHit2D[] raycastHit2D = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.up);
        foreach (var hit in raycastHit2D)
        {
            if(hit.collider.tag == "DEADABLE")
            {
                Debug.Log(hit.collider.name + "À» ¾ø¾Ú");
                hit.collider.gameObject.SetActive(false);
            }
            Debug.Log(hit.collider.tag);
        }
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    IEnumerator ShowEffect()
    {
        yield return new WaitForSeconds(effectCool);
        spriteRenderer.sprite = warningEffect;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = defaultSprite;
        effectCool -= 0.1f;
        StartCoroutine(ShowEffect());
    }
}
