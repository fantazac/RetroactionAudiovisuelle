using UnityEngine;

public class MapTerrain : MonoBehaviour
{
    private int width;
    private int height;
    private Grid grid;

    private ProceduralMesh proceduralMesh;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    // Resources
    private GameObject WallColliderPrefab;
    private GameObject RockPrefab;

    public Material GroundMaterial { get; protected set; }
    public Vector3 PlayerSpawn { get; protected set; }

    private void Awake()
    {
        Utility.World = this;

        WallColliderPrefab = Resources.Load<GameObject>("WallCollider");
        RockPrefab = Resources.Load<GameObject>("Rock");
        GroundMaterial = Resources.Load<Material>("GroundMaterial");
    }

    public void Generate(int width, int height, float initialProb, int birthLimit, int deathLimit)
    {
        this.width = width;
        this.height = height;

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = GroundMaterial;

        CellularAutomata ca = new CellularAutomata(width, height, initialProb, birthLimit, deathLimit);
        ca.Simulate(10);
        ca.Optimise();

        grid = new Grid(ca);
        proceduralMesh = new ProceduralMesh(grid, transform);

        PlacePlayerSpawn();
        PlaceRocks();
    }

    public bool CanMove(Vector3 to)
    {
        float size = Utility.Player.Size;

        return grid.IsEmpty((int)(to.x + size), (int)(to.y + size)) && // Top Right
               grid.IsEmpty((int)(to.x + size), (int)(to.y - size)) && // Bottom Right
               grid.IsEmpty((int)(to.x - size), (int)(to.y + size)) && // Top Left
               grid.IsEmpty((int)(to.x - size), (int)(to.y - size));   // Bottom Left
    }

    /* V1. Backup si nouveau mouvement pas bien
	public bool CanMove(Vector3 from, Vector3 to)
	{
		Vector2Int fromV2I = new Vector2Int((int) from.x, (int) from.z);
		Vector2Int toV2I = new Vector2Int((int) to.x, (int) to.z);

		if (fromV2I == toV2I) // still on the same cell
			return true;
		return grid.CanMove(fromV2I, toV2I);
	}
	//*/

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
        float rockSpawnChance = .35f;
        GameObject rockHolder = new GameObject("Rocks");
        rockHolder.transform.parent = transform;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid.Get(x, y) && x != PlayerSpawn.x && y != PlayerSpawn.y) // Can place a rock at this position
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

        UpgradeRocks(.28f, 2);
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

    public int DestroyRock(int x, int y)
    {
        int points = 0;

        Rock rock = grid.GetRock(x, y);
        if (rock != null)
        {
            points += rock.Level;
            if (points == 4) // Extra point for max rock level
                points++;

            Destroy(rock.gameObject);

        }

        return points;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(new Vector3(PlayerSpawn.x + .5f, .5f, PlayerSpawn.y + .5f), .5f);

        /*
		Gizmos.color = Color.yellow;
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (grid.Get(x, y))
					Gizmos.DrawCube(new Vector3(x + .5f, 0f, y + .5f), new Vector3(.9f, .1f, .9f));
			}
		}
		//*/
    }
}
