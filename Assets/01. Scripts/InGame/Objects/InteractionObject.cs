using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    public float interactionDistance = 1;
    public virtual bool OnInteraction()
    {
        return true;
    }
}
