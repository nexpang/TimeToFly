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
        // -> �̰� �÷��̾ �ٶ󺸴� ���� x �Ÿ��� ���ؼ� �� ������Ʈ�� ���� ������ (������)�� �����ִ°�.

        point.Add(master.transform.position); // �÷��̾� ��ġ
        point.Add(setRandomBezierPosition(master.transform.position, radiusA)); // ó������ ���� ��ġ
        point.Add(setRandomBezierPosition(objectPos, radiusB)); // �߰���
        point.Add(objectPos); // ������
    }

    private void Start()
    {
        master = GameObject.Find("DACK"); // �ְ� ���� ��ġ  = �÷��̾� ��ġ

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

        t += Time.deltaTime * spd; // t���� ���� �ְ� ������

        transform.position = MoveBezier(); // �̰ŷ� �����̴°�
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