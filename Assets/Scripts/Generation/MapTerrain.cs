using UnityEngine;

public class MapTerrain : MonoBehaviour
{
    private int width;
    private int height;
    private Grid grid;
    private GameObject RockPrefab;

    public Material GroundMaterial { get; protected set; }
    public Vector3 PlayerSpawn { get; protected set; }

    private void Awake()
    {
        Utility.World = this;

        RockPrefab = Resources.Load<GameObject>("Rock");
        GroundMaterial = Resources.Load<Material>("MainMaterial");
        GroundMaterial = Resources.Load<Material>("MainMaterial");
    }

    public void Generate(int width, int height, float initialProb, int birthLimit, int deathLimit)
    {
        this.width = width;
        this.height = height;

        CellularAutomata ca = new CellularAutomata(width, height, initialProb, birthLimit, deathLimit);
        ca.Simulate(10);
        ca.Optimise();

        grid = new Grid(ca);
        new ProceduralMesh(grid, transform);

        PlacePlayerSpawn();
        PlaceRocks();
    }

    public bool CanMove(Vector3 to)
    {
        float size = .3f;
        
        return grid.IsEmpty((int)(to.x + size), (int)(to.z + size)) && // Top Right
               grid.IsEmpty((int)(to.x + size), (int)(to.z - size)) && // Bottom Right
               grid.IsEmpty((int)(to.x - size), (int)(to.z + size)) && // Top Left
               grid.IsEmpty((int)(to.x - size), (int)(to.z - size));   // Bottom Left
    }

    private void PlacePlayerSpawn() // Not my proudest...
    {
        int numberOfPossibleSpawnpoints = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid.Get(x, y))
                    numberOfPossibleSpawnpoints++;
            }
        }

        int spawnpointIndex = Random.Range(1, numberOfPossibleSpawnpoints - 1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid.Get(x, y))
                {
                    spawnpointIndex--;
                    if (spawnpointIndex <= 0)
                    {
                        PlayerSpawn = new Vector3(x + 0.5f, 0.5f, y + 0.5f);
                        return;
                    }
                }
            }
        }
    }

    private void PlaceRocks()
    {
        float rockSpawnChance = 0.35f;
        GameObject rockHolder = new GameObject("Rocks");
        rockHolder.transform.parent = transform;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid.Get(x, y) && x != (int)PlayerSpawn.x && y != (int)PlayerSpawn.y) // Can place a rock at this position
                {
                    if (Random.value <= rockSpawnChance)
                    {
                        float yPivot = Random.Range(0f, 90f);
                        GameObject goRock = Instantiate(RockPrefab, new Vector3(x + .5f, .2f, y + .5f), new Quaternion(45f, yPivot, 45f, 0f), rockHolder.transform);
                        grid.SetRock(x, y, goRock.GetComponent<Rock>());
                    }
                }
            }
        }

        UpgradeRocks(0.28f, 3);
    }

    private void UpgradeRocks(float prob, int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            CellularAutomata cell = new CellularAutomata(width, height, prob, 5, 4);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!cell.Get(x, y))
                        grid.UpgradeRock(x, y);
                }
            }
        }
    }
}
