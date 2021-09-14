using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractionObject : MonoBehaviour
{
    public float interactionDistance = 1;
    [HideInInspector, NonSerialized] public bool isAlreadyChange = true;

    private void Awake()
    {
        isAlreadyChange = true;
    }
    public virtual bool OnInteraction()
    {
        return true;
    }
}
