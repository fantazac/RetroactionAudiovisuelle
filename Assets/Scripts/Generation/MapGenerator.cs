using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private GameObject map;

    private int birthLimit;
    private int deathLimit;
    private float initialProbability;

    public MapTerrain MapTerrain { get; private set; }

    public MapGenerator()
    {
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
        
        map = new GameObject("Map");
        map.transform.position = Vector3.zero;

        MapTerrain = map.AddComponent<MapTerrain>();
        MapTerrain.Generate(60, 60, initialProbability, birthLimit, deathLimit);
    }
}
