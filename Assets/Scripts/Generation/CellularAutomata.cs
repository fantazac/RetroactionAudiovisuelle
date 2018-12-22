using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CellularAutomata
{
    private bool[,] grid;
    private float initialRatio;
    
    public int Width { get; protected set;  }
    public int Height { get; protected set;  }
    public int BirthLimit { get; set; }
    public int DeathLimit { get; set; }
    
    #region Optimisation Attributes
    private List<int> rooms;
    private int[,] map;
    #endregion

    public CellularAutomata(int _width, int _height, float _initialRatio = .45f, int birthLimit = 4, int deathLimit = 3)
    {
        Width = _width;
        Height = _height;
        initialRatio = _initialRatio;
        grid = new bool[Width, Height];
        BirthLimit = birthLimit;
        DeathLimit = deathLimit;
        
        InitialiseGrid();
    }

    private void InitialiseGrid()
    {
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                grid[x, y] = Random.value >= initialRatio;
    }

    public void Simulate(int iteration)
    {
        for (int i = 0; i < iteration; i++)
            DoSimulationStep();
    }
    
    private void DoSimulationStep()
    {
        bool[,] tmpGrid = new bool[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                int neighbours = CountNeighbours(x, y);

                if (grid[x, y])
                {
                    if (neighbours < DeathLimit)
                        tmpGrid[x, y] = false;
                    else
                        tmpGrid[x, y] = true;
                }
                else
                {
                    if (neighbours > BirthLimit)
                        tmpGrid[x, y] = true;
                    else
                        tmpGrid[x, y] = false;
                }
            }
        }

        grid = tmpGrid;
    }

    private int CountNeighbours(int x, int y)
    {
        int count = 0;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) // ignore itself
                    continue;
                
                if (x + i < 0 || y + j < 0 || x + i >= Width || y + j >= Height) // off grid 
                    count++;
                else if (grid[x + i, y + j]) // check neighbour
                    count++;
            }
        }
        return count;
    }

    public bool Get(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return true; // Wall
        
        return grid[x, y];
    }

    public void Invert(int x, int y)
    {
        grid[x, y] = !grid[x, y];
    }

    
    #region Optimisation Methods

    public void Optimise()
    {
        RemoveUnreachableRooms();
        RecalculateBounds();
    }
    
    private void RemoveUnreachableRooms()
    {
        rooms = new List<int>();
        map = new int[Width, Height];
        int roomToKeep = 0;
        IdentifyRooms();
		
        for (int i = 0; i < rooms.Count; i++)
            if (rooms[i] > rooms[roomToKeep])
                roomToKeep = i;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (map[x, y] > 0 && map[x, y] != roomToKeep + 1)
                    Invert(x, y);
            }
        }

        rooms = null;
        map = null;
    }
    
    private void IdentifyRooms()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                // If we are in a room and it is not already identified
                if (!Get(x, y) && map[x, y] == 0)
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
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return;

        if (!Get(x, y) && map[x, y] == 0)
        {
            map[x, y] = roomID;
            rooms[roomID - 1]++;
			
            Expand(x + 1, y, roomID);
            Expand(x - 1, y, roomID);
            Expand(x, y + 1, roomID);
            Expand(x, y - 1, roomID);
        }
    }
    
    public void RecalculateBounds()
    {
        Vector2Int min = new Vector2Int(Width, Height);
        Vector2Int max = new Vector2Int(0, 0);
        
        // Calculate boundaries
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (!Get(x, y))
                {
                    // Upper bound
                    if (x > max.x)
                        max.x = x;
                    if (y > max.y)
                        max.y = y;
                    
                    // Lower bound
                    if (x < min.x)
                        min.x = x;
                    if (y < min.y)
                        min.y = y;
                }
            }
        }

        // Update boundaries
        Width = max.x - min.x;
        Height = max.y - min.y;
        bool[,] oldGrid = grid;
        grid = new bool[Width, Height];

        for (int x = min.x; x < max.x; x++)
        {
            for (int y = min.y; y < max.y; y++)
            {
                grid[x - min.x, y - min.y] = oldGrid[x, y];
            }
        }
    }

    #endregion
    
}
