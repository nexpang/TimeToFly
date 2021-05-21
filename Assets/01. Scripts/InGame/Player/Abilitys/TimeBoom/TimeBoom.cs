using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log("�����");
            transform.SetParent(ability.transform);
            if(GetComponent<Rigidbody2D>() != null)
                Destroy(GetComponent<Rigidbody2D>());
            GetComponent<CircleCollider2D>().enabled = false;
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Boom();
        Debug.Log("����");
    }

    void Boom()
    {
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
            if(hit.collider.tag == "DEADABLE" && hit.collider.gameObject != tileMap)
            {
                Debug.Log(hit.collider.name + "�� ����");
                hit.collider.gameObject.SetActive(false);
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
        Debug.Log("����");
        yield return new WaitForSeconds(effectCool);
        spriteRenderer.sprite = warningEffect;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = defaultSprite;
        effectCool -= 0.08f;
        if (effectCool < 0.1f)
            effectCool = 0.05f;
        StartCoroutine(ShowEffect());
    }
}