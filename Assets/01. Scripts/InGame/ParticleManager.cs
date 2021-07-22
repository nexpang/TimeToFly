using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType
{
    BOX,
    BRICK,
    DARKBRICK
}

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    public Texture2D boxFrag;
    public Texture2D BrickFrag;
    public Texture2D darkBrickFrag;

    public GameObject blockParticle;

    private void Awake()
    {
        Instance = this;
    }

    [ContextMenu("ÆÄÆ¼Å¬")]
    private void Test()
    {
        CreateParticle(ParticleType.BRICK, transform.position,1);
    }

    public static void CreateParticle(ParticleType type, Vector2 pos, float destroyTime)
    {
        GameObject particle = Instantiate(Instance.blockParticle, Instance.gameObject.transform);

        if (type == ParticleType.BOX)
        {
            particle.GetComponent<ParticleSystemRenderer>().material.mainTexture = Instance.boxFrag;
            ParticleSystem.TextureSheetAnimationModule module = particle.GetComponent<ParticleSystem>().textureSheetAnimation;
            module.numTilesY = 4;
        }
        else if (type == ParticleType.BRICK)
        {
            particle.GetComponent<ParticleSystemRenderer>().material.mainTexture = Instance.BrickFrag;
            ParticleSystem.TextureSheetAnimationModule module = particle.GetComponent<ParticleSystem>().textureSheetAnimation;
            module.numTilesY = 3;
        }
        else if (type == ParticleType.DARKBRICK)
        {
            particle.GetComponent<ParticleSystemRenderer>().material.mainTexture = Instance.darkBrickFrag;
            ParticleSystem.TextureSheetAnimationModule module = particle.GetComponent<ParticleSystem>().textureSheetAnimation;
            module.numTilesY = 3;
        }

        particle.transform.position = pos;
        Destroy(particle, destroyTime);
    }
}
