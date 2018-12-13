using UnityEngine;

public class CellularAutomata
{
    private bool[,] grid;
    private int width;
    private int height;
    private float initialRatio;
    
    public int BirthLimit { get; set; }
    public int DeathLimit { get; set; }

    public CellularAutomata(int _width, int _height, float _initialRatio = .45f, int birthLimit = 4, int deathLimit = 3)
    {
        width = _width;
        height = _height;
        initialRatio = _initialRatio;
        grid = new bool[width, height];
        BirthLimit = birthLimit;
        DeathLimit = deathLimit;
        
        InitialiseGrid();
    }

    private void InitialiseGrid()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y] = Random.value >= initialRatio;
    }

    public void Simulate(int iteration)
    {
        for (int i = 0; i < iteration; i++)
            DoSimulationStep();
    }
    
    private void DoSimulationStep()
    {
        bool[,] tmpGrid = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
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
                
                if (x + i < 0 || y + j < 0 || x + i >= width || y + j >= height) // off grid 
                    count++;
                else if (grid[x + i, y + j]) // check neighbour
                    count++;
            }
        }
        return count;
    }

    public bool Get(int x, int y)
    {
        return grid[x, y];
    }

    public void Invert(int x, int y)
    {
        grid[x, y] = !grid[x, y];
    }

}
