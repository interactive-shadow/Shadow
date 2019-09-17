using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDetectManager : MonoBehaviour
{
    Queue<Vector2Int> shadowPositions = new Queue<Vector2Int>();

    public static ShadowDetectManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void AddPosition(Vector2Int pos)
    {
        shadowPositions.Enqueue(pos);
    }

    public void GenerateShadow(string shadowClass)
    {
        Vector2Int pos = shadowPositions.Dequeue();
        //TODO 影を生成する処理
    }

    public static ShadowDetectManager GetInstance()
    {
        return Instance;
    }
}
