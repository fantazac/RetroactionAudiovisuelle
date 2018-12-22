using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Grid
{
    public int Width { get; protected set; }
    public int Height { get; protected set; }

    private Cell[,] grid; 
    
    public Grid(CellularAutomata ca)
    {
        Width = ca.Width;
        Height = ca.Height;
        grid = new Cell[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                grid[x, y] = new Cell(!ca.Get(x, y));
            }
        }
    }

    public bool IsNextToWalkable(int x, int y)
    {
        return Get(x + 1, y) && Get(x - 1, y) && Get(x, y + 1) && Get(x, y - 1);
    }
    
    public bool Get(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return false;
        return grid[x, y].Walkable;
    }
    
    public Rock GetRock(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return null;
        return grid[x, y].Rock;
    }

    public void SetRock(int x, int y, Rock rock)
    {
        grid[x, y].Rock = rock;
    }

    public void UpgradeRock(int x, int y)
    {
        if (Get(x, y) && grid[x, y].Rock != null)
            grid[x, y].Rock.LevelUp();
    }
    
    // An empty cell is a walkable cell that contains no rock.
    public bool IsEmpty(int x, int y)
    {
        return Get(x, y) && GetRock(x, y) == null;
    }
    
    public bool CanMove(Vector2Int from, Vector2Int to)
    {
        return IsEmpty(to.x, to.y) && CanPerformActionWithoutObstacles(from, to);
    }

    // Ensures that at least one of the two neighbouring cells are walkable before doing an action.
    // This prevents the player from moving and breaking rocks through walls and edges.
    public bool CanPerformActionWithoutObstacles(Vector2Int from, Vector2Int to)
    {
		return Get(to.x, from.y) || Get(from.x, to.y);
    }
}
