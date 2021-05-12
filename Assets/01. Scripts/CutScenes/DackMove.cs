using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DackMove : MonoBehaviour
{
    List<Vector2> point = new List<Vector2>();

    [SerializeField]
    [Range(0, 1)]
    private float t = 0;

    public float spd = 5f;
    public float radiusA = 0.55f;
    public float radiusB = 0.45f;

    public GameObject master;
    Vector3 objectPos;
    private float count = 1;


    private void MakeNextPoint()
    {
        point.Clear();

        objectPos = new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), 0);
        // -> 이게 플레이어가 바라보는 방향 x 거리를 곱해서 이 오브젝트가 향할 목적지 (종착점)을 정해주는거.

        point.Add(master.transform.position); // 플레이어 위치
        point.Add(setRandomBezierPosition(master.transform.position, radiusA)); // 처음으로 향할 위치
        point.Add(setRandomBezierPosition(objectPos, radiusB)); // 중간점
        point.Add(objectPos); // 종착점
    }

    private void Start()
    {
        master = GameObject.Find("DACK"); // 애가 나올 위치  = 플레이어 위치

        MakeNextPoint();
    }

    private void Update()
    {
        if (t > 1 && count >= 5)
        {
            return;
        }
        else if(t>1)
        {
            count++;
            t = 0;
            MakeNextPoint();
        }

        t += Time.deltaTime * spd; // t값에 따라 애가 움직임

        transform.position = MoveBezier(); // 이거로 움직이는거
    }

    Vector2 MoveBezier()
    {
        return new Vector2(
            FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x),
            FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y)
            );
    }

    float FourPointBezier(float a, float b, float c, float d)
    {
        return Mathf.Pow(1 - t, 3) * a
            + Mathf.Pow(1 - t, 2) * 3 * t * b
            + Mathf.Pow(t, 2) * 3 * (1 - t) * c
            + Mathf.Pow(t, 3) * d;
    }

    Vector2 setRandomBezierPosition(Vector2 origin, float radius)
    {
        return new Vector2(
            radius * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x,
            radius * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.y
            );
    }
}