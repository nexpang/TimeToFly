using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAbility2 : Ability, IAbility
{
    public void OnAbility()
    {
        Debug.Log("능력 빠지지직빠직 빠슝");
    }
}
