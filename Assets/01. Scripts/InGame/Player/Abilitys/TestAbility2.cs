using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAbility2 : MonoBehaviour, IAbility
{
    public void OnAbility()
    {
        Debug.Log("�ɷ� ������������ ����");
    }

    private void Update()
    {
        //PlayerInput.KeyHorizontalRaw = PlayerInput.KeyHorizontalRaw * 2;
    }
}
