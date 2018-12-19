using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Terrain : MonoBehaviour
{
	private int width;
	private int height;
	
	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private MeshCollider meshCollider;

	public Material groundMaterial;
	
	private CellularAutomata ca;
	private ProceduralMesh grid;
	
	// Optimisation
	private List<int> rooms;
	private int[,] map;
	private CellularAutomata automata;

	// Resources
	GameObject WallColliderPrefab;
	GameObject RockPrefab;
	Material GroundMaterial;

	public Vector2Int PlayerSpawn { get; protected set; }
	private GameObject[,] rockGrid;

	private void LoadResources()
	{
		WallColliderPrefab = Resources.Load<GameObject>("WallCollider");
		RockPrefab = Resources.Load<GameObject>("Rock");
		GroundMaterial = Resources.Load<Material>("GroundMaterial");
	}

	public void Generate(int width, int height, float initialProb, int birthLimit, int deathLimit)
	{
		Utility.World = this;
		this.width = width;
		this.height = height;
		LoadResources();
		
		meshFilter = gameObject.AddComponent<MeshFilter>();
		meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshRenderer.material = GroundMaterial;
		
		ca = new CellularAutomata(width, height, initialProb, birthLimit, deathLimit);
		ca.Simulate(10);
		RemoveUnreachableRooms(ca);
		
		grid = new ProceduralMesh(ca);
		meshFilter.mesh = grid.GetMesh();
		
		PlaceWalls();
		PlacePlayerSpawn();
		PlacePlayerRocks();
	}

	private void RemoveUnreachableRooms(CellularAutomata automata)
	{
		this.automata = automata;
		IdentifyRooms();
		RemoveUnreachableRooms();
		
		map = null;
		this.automata = null;
	}
	
	private void IdentifyRooms()
	{
		rooms = new List<int>();
		map = new int[ width, height];
		
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				// If we are in a room and it is not already identified
				if (!automata.Get(x, y) && map[x, y] == 0)
				{
					rooms.Add(1);
					map[x, y] = rooms.Count;
					
					Expand(x + 1, y, rooms.Count);
					Expand(x - 1, y, rooms.Count);
					Expand(x, y + 1, rooms.Count);
					Expand(x, y - 1, rooms.Count);
				}
			}
		}
	}

	private void Expand(int x, int y, int roomID)
	{
		if (x < 0 || x >= width || y < 0 || y >= height)
			return;

		if (!automata.Get(x, y) && map[x, y] == 0)
		{
			map[x, y] = roomID;
			rooms[roomID - 1]++;
			
			Expand(x + 1, y, roomID);
			Expand(x - 1, y, roomID);
			Expand(x, y + 1, roomID);
			Expand(x, y - 1, roomID);
		}
	}

	private void RemoveUnreachableRooms()
	{
		int roomToKeep = 0;
		
		for (int i = 0; i < rooms.Count; i++)
			if (rooms[i] > rooms[roomToKeep])
				roomToKeep = i;

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (map[x, y] > 0 && map[x, y] != roomToKeep + 1)
					automata.Invert(x, y);
			}
		}
	}

	private void PlaceWalls()
	{
		for (int y = -1; y <= height; y++)
		{
			for (int x = -1; x <= width; x++)
			{
				if (ca.IsWall(x, y))
				{
					GameObject wall = Instantiate(WallColliderPrefab, new Vector3(x + .5f, .5f, y + .5f), Quaternion.identity, transform);
				}
			}
		}
	}

	private void PlacePlayerSpawn() // Not my proudest...
	{
		int numberOfPossibleSpawnpoints = 0;
		
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (!ca.Get(x, y))
					numberOfPossibleSpawnpoints++;
			}
		}

		int spawnpointIndex = Random.Range(1, numberOfPossibleSpawnpoints - 1);
		
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (!ca.Get(x, y))
				{
					spawnpointIndex--;
					if (spawnpointIndex <= 0)
					{
						PlayerSpawn = new Vector2Int(x, y);
						return;
					}
				}
			}
		}
	}
	
	private void PlacePlayerRocks()
	{
		float rockSpawnChance = .65f;
		rockGrid = new GameObject[width, height];
		
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				rockGrid[x, y] = null;

				if (!ca.Get(x, y) && x != PlayerSpawn.x && y != PlayerSpawn.y) // Can place a rock at this position
				{
					if (Random.value <= rockSpawnChance)
					{
						float yPivot = Random.Range(0f, 90f);
						GameObject rock = Instantiate(RockPrefab, new Vector3(x + .5f, .2f, y + .5f), new Quaternion(45f, yPivot, 45f, 0f), transform);
						rockGrid[x, y] = rock;
					}
				}
			}
		}
	}

	public int DestroyRock(int x, int y)
	{
		int points = 0;

		if (rockGrid[x, y] != null)
		{
			Rock rock = rockGrid[x, y].GetComponent<Rock>();
			points += rock.Level;

			if (points == 4) // Extra point for max rock level
				points++;
			
			Destroy(rockGrid[x, y]);
			rockGrid[x, y] = null;
		}

		return points;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(new Vector3(PlayerSpawn.x + .5f, .5f, PlayerSpawn.y + .5f), .5f);
	}
}
