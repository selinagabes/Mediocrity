using UnityEngine;
using System;
using System.Collections;

public class MapGenerator : MonoBehaviour
{
    [Range(0,100)]
    public int randomFillPercent;
    public int width; 
    public int height;
    public string seed;
    public bool useRandomSeed;
    int[,] _map;
    //grid of integers, tile = 0 (Empty), tile = 1(Wall)

    void Start()
    {
        GenerateMap();
    }
    void GenerateMap()
    {
        _map = new int[width, height];
        RandomFillMap();
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }


        System.Random pseudoRandom = new System.Random(int.Parse(seed));

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
            }
        }
    }

    void OnDrawGizmos()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Gizmos.color = (_map[x, y] == 1 ? Color.black : Color.white);
                Vector3 pos = new Vector3(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f, 0);
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }
    }
}
