using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private int coinAmount = 20;
    [SerializeField]
    private int obstacleTypeCount = 20;
    [SerializeField]
    private GameObject coin;
    [SerializeField]
    private List<GameObject> obstacles;

    private List<Vector3> usedPositions;
    private int boundSize = 6;
    private Vector2 bounds = new Vector2(-20, 20);
    private int maxAttempts = 200;

    public static MapManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        usedPositions = new List<Vector3>();
        usedPositions.Add(Vector3.zero);
        GenerateMap();
    }

    public void GenerateMap()
    {
        //generate coins
        for (int i = 0; i < coinAmount; i++)
        {
            Vector3 vec = GetUniquePosition();
            if (!vec.Equals(Vector3.negativeInfinity))
            {
                Instantiate(coin, vec, Quaternion.identity);
                usedPositions.Add(vec);
            }
        }
        //generate obstacles
        for(int i = 0; i < obstacles.Count; i++)
        {
            for (int j = 0; j < obstacleTypeCount; j++)
            {
                Vector3 vec = GetUniquePosition();
                if(!vec.Equals(Vector3.negativeInfinity))
                {
                    Instantiate(obstacles[i], vec, Quaternion.identity);
                    usedPositions.Add(vec);
                }
            }
        }
        
    }

    private Vector3 GetUniquePosition()
    {
        for(int i = 0; i < maxAttempts; i++)
        {
            Vector3 vec = GenerateRandomPos();
            
            if(IsClearPos(vec))
                return vec;
        }
        return Vector3.negativeInfinity;
    }

    private Vector3 GenerateRandomPos()
    {
        int randx = Random.Range((int)bounds.x, (int)bounds.y);
        int randy = Random.Range((int)bounds.x, (int)bounds.y);
        Vector3 vec = new Vector3(randx, randy, 0);
        return vec;
    }

    private bool IsClearPos(Vector3 vec)
    {
        foreach (var item in usedPositions)
        {
            if (IsInAnotherObjectBounds(vec, item))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsInAnotherObjectBounds(Vector3 pos, Vector3 usedPos)
    {
        Vector3 minBounds = usedPos - Vector3.one * (boundSize / 2);
        Vector3 maxBounds = usedPos + Vector3.one * (boundSize / 2);

        return (pos.x >= minBounds.x && pos.x <= maxBounds.x) &&
               (pos.y >= minBounds.y && pos.y <= maxBounds.y);
    }
}
