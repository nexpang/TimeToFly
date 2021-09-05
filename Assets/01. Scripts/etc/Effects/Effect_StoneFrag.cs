using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_StoneFrag : Effect
{
    protected override void OnEnable()
    {
        base.OnEnable();
        ObjectManager.PlaySound(ObjectManager.Instance.soundData.Audio_Rock_Breaking, 1f, true);
    }
}
