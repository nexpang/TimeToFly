using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    // �̺�Ʈ �Ŵ��� v.1 made by ��

    // �̺�Ʈ ������ ������� ��������?
    // ���� �̸��� �ٿ��� �������� �̺�Ʈ�� �ϳ��� Ŭ������ ������ �� �ֽ��ϴ�

    // ���� ���� �������� ���ڸ� �޴� �Լ��� �߰� ����..


    private static Dictionary<string, UnityEvent> events = new Dictionary<string, UnityEvent>();

    public static void AddEvent(string name, UnityAction action)    // Ư�� �̸��� �̺�Ʈ�� �߰��ϰ� �Լ��� ���� �߰�
    {
        if (!events.ContainsKey(name))
        {
            events.Add(name, new UnityEvent());
        }

        events[name].AddListener(action);
    }

    public static void Invoke(string name)
    {
        if (events.ContainsKey(name))
        {
            events[name].Invoke();
        }
    }

    public static void RemoveEvent(string name, UnityAction action) // Ư�� �̸��� �̺�Ʈ �ȿ� �ִ� �Լ��� ����
    {
        if (events.ContainsKey(name))
        {
            events[name].RemoveListener(action);
        }
    }

    public static void RemoveAllEvent(string name)  // Ư�� �̸��� �̺�Ʈ�� �ʱ�ȭ
    {
        if (events.ContainsKey(name))
        {
            events[name].RemoveAllListeners();
        }
    }

    public static void RemoveAllEvent()             // ��ųʸ� �ȿ� �ִ� �̺�Ʈ�� �ϴ� �ʱ�ȭ (���� ���ϰ� ������)
    {
        foreach (var key in events.Keys)
        {
            events[key].RemoveAllListeners();
        }
    }
}
