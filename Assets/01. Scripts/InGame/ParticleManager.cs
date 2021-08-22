using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockParticleType
{
    BOX,
    BRICK,
    DARKBRICK
}

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    [Header("블럭 파티클")]
    public Texture2D boxFrag;
    public Texture2D BrickFrag;
    public Texture2D darkBrickFrag;
    [Space(10)]

    [Header("일반 파티클(풀매니저)")]
    public GameObject effect_StoneFragment;
    public GameObject effect_PortalMoving;
    public GameObject effect_PortalDirt;
    public GameObject effect_EagleFeather;

    [Header("특수 이펙트")]
    public GameObject PatternWarningBox;
    public Transform WarningBox_Box;

    [Space(10)]
    public GameObject blockParticle;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PoolManager.CreatePool<Effect_StoneFrag>(effect_StoneFragment, this.transform);
        PoolManager.CreatePool<Effect_PortalMoving>(effect_PortalMoving, this.transform);
        PoolManager.CreatePool<Effect_PortalDirt>(effect_PortalDirt, this.transform);
        PoolManager.CreatePool<Effect_EagleFeather>(effect_EagleFeather, this.transform);
        PoolManager.CreatePool<Effect_WarningBox>(PatternWarningBox, WarningBox_Box);
    }

    public static void CreateBlockParticle(BlockParticleType type, Vector2 pos, float destroyTime)
    {
        GameObject particle = Instantiate(Instance.blockParticle, Instance.gameObject.transform);

        if (type == BlockParticleType.BOX)
        {
            particle.GetComponent<ParticleSystemRenderer>().material.mainTexture = Instance.boxFrag;
            ParticleSystem.TextureSheetAnimationModule module = particle.GetComponent<ParticleSystem>().textureSheetAnimation;
            module.numTilesY = 4;
        }
        else if (type == BlockParticleType.BRICK)
        {
            particle.GetComponent<ParticleSystemRenderer>().material.mainTexture = Instance.BrickFrag;
            ParticleSystem.TextureSheetAnimationModule module = particle.GetComponent<ParticleSystem>().textureSheetAnimation;
            module.numTilesY = 3;
        }
        else if (type == BlockParticleType.DARKBRICK)
        {
            particle.GetComponent<ParticleSystemRenderer>().material.mainTexture = Instance.darkBrickFrag;
            ParticleSystem.TextureSheetAnimationModule module = particle.GetComponent<ParticleSystem>().textureSheetAnimation;
            module.numTilesY = 3;
        }

        particle.transform.position = pos;
        Destroy(particle, destroyTime);
    }

    public static void CreateParticle<T>(Vector2 pos) where T : MonoBehaviour
    {
        T obj = PoolManager.GetItem<T>();
        obj.transform.position = pos;
    }

    public static void CreateWarningBox(Vector2 position, Vector2 size, float waitTime)
    {
        Effect_WarningBox obj = PoolManager.GetItem<Effect_WarningBox>();
        obj.Create(position, size, waitTime);
    }

    public static void CreateWarningBox(Vector2 position, Vector2 size, float waitTime, Color startColor, Color endColor, float colorInterval)
    {
        Effect_WarningBox obj = PoolManager.GetItem<Effect_WarningBox>();
        obj.Create(position, size, waitTime, startColor, endColor, colorInterval);
    }
}
