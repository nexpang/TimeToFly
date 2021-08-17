using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MateralResetDebug : MonoBehaviour
{
    [Header("Unsaveable Materials")]
    public Material[] materials;

    private void OnDisable()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetTextureOffset("_MainTex", Vector2.zero);
        }
    }
}
