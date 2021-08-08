using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    // 이벤트 매니져 v.1 made by 백

    // 이벤트 여러개 만드느라 귀찮았죠?
    // 이젠 이름을 붙여서 여러개의 이벤트를 하나의 클래스로 관리할 수 있습니다

    // 물론 아직 여러개의 인자를 받는 함수는 추가 못함..


    private static Dictionary<string, UnityEvent> events = new Dictionary<string, UnityEvent>();

    public static void AddEvent(string name, UnityAction action)    // 특정 이름의 이벤트를 추가하고 함수도 같이 추가
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

    public static void RemoveEvent(string name, UnityAction action) // 특정 이름의 이벤트 안에 있는 함수를 지움
    {
        if (events.ContainsKey(name))
        {
            events[name].RemoveListener(action);
        }
    }

    public static void RemoveAllEvent(string name)  // 특정 이름의 이벤트를 초기화
    {
        if (events.ContainsKey(name))
        {
            events[name].RemoveAllListeners();
        }
    }

    public static void RemoveAllEvent()             // 딕셔너리 안에 있는 이벤트를 싹다 초기화 (생각 잘하고 쓰세요)
    {
        foreach (var key in events.Keys)
        {
            events[key].RemoveAllListeners();
        }
    }
}
