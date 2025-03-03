using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Map Configuration")]

    [SerializeField]
    private int mazeWidth;
    [SerializeField]
    private int mazeHeight;
    [SerializeField]
    private int coinAmount;
    [SerializeField]
    private int obstacleTypeCount;
    [SerializeField]
    private List<GameObject> objects;
    [SerializeField]
    private GameObject bound;
    [SerializeField]
    private GameObject backgroundTile;

    private int[,] maze;
    private List<Vector2Int> usedPositions;

    private static int BOUND_SIZE = 6;
    private static int MAX_ATTEMPTS = 200;

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
        maze = new int[mazeWidth, mazeHeight];
        usedPositions = new List<Vector2Int>();
        GenerateMaze();
        try
        {
            GenerateCoins();
        }
        catch (System.Exception exception)
        {
            Debug.LogError(exception.Message);
        }
        try
        {
            GenerateObstacles();
        }
        catch (System.Exception exception)
        {
            Debug.LogError(exception.Message);
        }
        BuildMaze();
    }

    private void GenerateMaze()
    {
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                maze[x, y] = 0; // fill with walls
            }
        }

        // position 0,0 is always starting point
        Carve(0, 0);
    }

    private void Carve(int x, int y)
    {
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        maze[x, y] = 1; // change the tile to empty

        // shuffle the directions
        for (int i = 0; i < 4; i++)
        {
            int r = Random.Range(i, 4);
            int temp = dx[r];
            dx[r] = dx[i];
            dx[i] = temp;

            temp = dy[r];
            dy[r] = dy[i];
            dy[i] = temp;
        }

        // recursively carve the maze
        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i] * 2;
            int ny = y + dy[i] * 2;

            if (nx >= 0 && nx < mazeWidth && ny >= 0 && ny < mazeHeight && maze[nx, ny] == 0)
            {
                maze[x + dx[i], y + dy[i]] = 1; // carve empty tile
                Carve(nx, ny);
            }
        }
    }

    private void GenerateCoins()
    {
        List<Vector2Int> foundPositions = new List<Vector2Int>();
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                //make sure to leave starting point clear
                if (x == 0 && y == 0)
                    continue;
                if (maze[x, y] == 1)
                {
                    foundPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        if (foundPositions.Count < coinAmount)
            throw new System.Exception("Too little space to generate all coins!");
        else
        {
            for (int i = 0; i < coinAmount; i++)
            {
                var uniqueVec = GetUniquePosition(foundPositions);

                if (uniqueVec == new Vector2Int(-1, -1))
                    throw new System.Exception("Couldn't find unique position for a coin");
                else
                    maze[uniqueVec.x, uniqueVec.y] = 2;
            }
        }
    }

    private void GenerateObstacles()
    {
        List<Vector2Int> foundPositions = new List<Vector2Int>();
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                //make sure to leave starting point clear
                if (x == 0 && y == 0)
                    continue;
                if (maze[x, y] == 1)
                {
                    foundPositions.Add(new Vector2Int(x, y));
                }
            }
        }
        if (foundPositions.Count < obstacleTypeCount)
            throw new System.Exception("Too little space to generate all obstacles!");
        else
        {
            for (int i = 0; i < obstacleTypeCount; i++)
            {
                var uniqueVec = GetUniquePosition(foundPositions);

                if (uniqueVec == new Vector2Int(-1, -1))
                    throw new System.Exception("Couldn't find unique position for an obstacle");
                else
                    maze[uniqueVec.x, uniqueVec.y] = Random.Range(3, objects.Count);
            }
        }
    }

    private void BuildMaze()
    {
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                //create background tile
                Instantiate(backgroundTile, new Vector3(x * BOUND_SIZE, y * BOUND_SIZE, 0), Quaternion.identity);

                //make sure to leave starting point clear
                if (x == 0 && y == 0)
                    continue;

                if(maze[x, y] != 1)
                {
                    Instantiate(objects[maze[x, y]], new Vector3(x * BOUND_SIZE, y * BOUND_SIZE, 0), Quaternion.identity);
                }
            }
        }

        for(int i = -1; i < mazeWidth + 1; i++)
        {
            //create background tile
            Instantiate(backgroundTile, new Vector3(-1 * BOUND_SIZE, i * BOUND_SIZE, 0), Quaternion.identity);
            Instantiate(backgroundTile, new Vector3(mazeWidth * BOUND_SIZE, i * BOUND_SIZE, 0), Quaternion.identity);

            //create bounds
            Instantiate(bound, new Vector3(-1 * BOUND_SIZE, i * BOUND_SIZE, 0), Quaternion.identity);
            Instantiate(bound, new Vector3(mazeWidth * BOUND_SIZE, i * BOUND_SIZE, 0), Quaternion.identity);
        }
        for (int j = 0; j < mazeHeight; j++)
        {
            //create background tile
            Instantiate(backgroundTile, new Vector3(j * BOUND_SIZE, -1 * BOUND_SIZE, 0), Quaternion.identity);
            Instantiate(backgroundTile, new Vector3(j * BOUND_SIZE, mazeHeight * BOUND_SIZE, 0), Quaternion.identity);

            //create bounds
            Instantiate(bound, new Vector3(j * BOUND_SIZE, -1 * BOUND_SIZE, 0), Quaternion.identity);
            Instantiate(bound, new Vector3(j * BOUND_SIZE, mazeHeight * BOUND_SIZE, 0), Quaternion.identity);
        }
    }
    private Vector2Int GetUniquePosition(List<Vector2Int> list)
    {
        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            Vector2Int vec = GenerateRandomPos(list);

            if (!usedPositions.Contains(vec))
            {
                usedPositions.Add(vec);
                return vec;
            }
        }
        return new Vector2Int(-1, -1);
    }

    private Vector2Int GenerateRandomPos(List<Vector2Int> list)
    {
        Vector2Int vec = list[Random.Range(0, list.Count)];
        return vec;
    }
}
