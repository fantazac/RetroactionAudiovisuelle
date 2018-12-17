using Boo.Lang;
using UnityEngine;

public class ProceduralMesh
{
    private int width;
    private int height;

    private List<int> triangleList;
    
    private Vector3[] vertices;
    private int[] triangles;
    private CellularAutomata ca;
    private float scale;
    private Vector3 offset;


    public ProceduralMesh(CellularAutomata ca, float scale = 1)
    {
        width = ca.Width + 1;
        height = ca.Height + 1;
        this.ca = ca;
        
        //vertices = new Vector3[(width + 1) * (height + 1)];
        vertices = new Vector3[width * height];
        triangleList = new List<int>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Vertices
                vertices[y * width + x] = new Vector3(x * scale, 0f, y * scale) - offset;

                // Triangles
                if (!ca.Get(x, y))
                {
                    // Upper Right
                    triangleList.Add(y * width + x);
                    triangleList.Add((y + 1) * width + x + 1);
                    triangleList.Add(y * width + x + 1);
    
                    // Bottom Left
                    triangleList.Add(y * width + x);
                    triangleList.Add((y + 1) * width + x);
                    triangleList.Add((y + 1) * width + x + 1);
                }
            }
        }
    }
    
    /*
    public ProceduralMesh(int _width = 50, int _height = 50, float scale = 1)
    {
        width = _width + 1;
        height = _height + 1;
        vertices = new Vector3[width * height];
        triangleList = new List<int>();
        
        for (int y = 0; y <= height + 1; y++)
            for (int x = 0; x <= width + 1; x++)
                vertices[y * width + x] = new Vector3(x * scale, 0f, y * scale) - offset;
    }
    //*/

    public void SetHeight(int x, int y, float height)
    {
        Vector3 vert = vertices[y * width + x];
        vert.y = height;
        vertices[y * width + x] = vert;
    }

    public Mesh GetMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangleList.ToArray();
        
        return mesh;
    }

    public void AddQuad(int x, int y)
    {
        // Upper Right
        triangleList.Add(y * width + x);
        triangleList.Add((y + 1) * width + x + 1);
        triangleList.Add(y * width + x + 1);
        
        // Bottom Left
        triangleList.Add(y * width + x);
        triangleList.Add((y + 1) * width + x);
        triangleList.Add((y + 1) * width + x + 1);
    }
}
