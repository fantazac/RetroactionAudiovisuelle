using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    private GameObject map;

    private int width;
    private int height;
    private int birthLimit;
    private int deathLimit;
    private float initialProbability;

    public MapTerrain MapTerrain { get; private set; }

    public MapGenerator()
    {
        width = 60;
        height = 60;
        birthLimit = 4;
        deathLimit = 3;
        initialProbability = 0.6f;
    }

    public void GenerateMap()
    {
        if (MapTerrain)
        {
            Destroy(MapTerrain.gameObject);
        }

        Random.InitState(Environment.TickCount);
        map = new GameObject("Map");
        map.transform.position = Vector3.zero;

        MapTerrain = map.AddComponent<MapTerrain>();
        MapTerrain.Generate(width, height, initialProbability, birthLimit, deathLimit);
    }
}
