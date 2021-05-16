using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability1_SleepBubble : MonoBehaviour
{
    private Animator animator = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void BubbleAwake() // �ش� �Լ��� Sleeping_SpeechBubble_Awake�� �������.
    {
        animator.applyRootMotion = false;
        transform.localPosition = new Vector3(1.51f, 1.19f, 0);
    }

    void BubbleIdle() // �ش� �Լ��� Sleeping_SpeechBubble_Idle�� �������.
    {
        animator.applyRootMotion = true;
    }
}
