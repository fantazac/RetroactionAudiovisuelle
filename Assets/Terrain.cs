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
	private ProceduralGrid grid;
	
	// Optimisation
	private List<int> rooms;
	private int[,] map;
	private CellularAutomata automata;
	

	public void Generate(int width, int height, float initialProb, int birthLimit, int deathLimit)
	{
		this.width = width;
		this.height = height;
		
		meshFilter = gameObject.AddComponent<MeshFilter>();
		meshRenderer = gameObject.AddComponent<MeshRenderer>();
		
		grid = new ProceduralGrid(width, height);
		ca = new CellularAutomata(width, height, initialProb, birthLimit, deathLimit);
		ca.Simulate(10);
		RemoveUnreachableRooms(ca);
		
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				if (!ca.Get(x, y))
					grid.AddQuad(x, y);
			}
		}

		meshFilter.mesh = grid.GetMesh();
	}

	private void RemoveUnreachableRooms(CellularAutomata automata)
	{
		this.automata = automata;
		IdentifyRooms();
		RemoveUnreachableRooms();
		
		Debug.Log("Identified " + rooms.Count + " rooms:");
		for (int i = 0; i < rooms.Count; i++)
			Debug.Log("Room_" + i + " - size: " + rooms[i]);
		
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
}
