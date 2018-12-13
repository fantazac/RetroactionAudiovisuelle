using Boo.Lang;
using UnityEngine;

public class ProceduralGrid
{
    private int width;
    private int height;

    private List<int> triangleList;
    
    private Vector3[] vertices;
    private int[] triangles;
    

    public ProceduralGrid(int _width = 50, int _height = 50, float horizontalScale = 1)
    {
        width = _width + 1;
        height = _height + 1;
        // todo cell grid should be width + 1 and height + 1
        
        vertices = new Vector3[width * height];
        //triangles = new int[(width - 1) * (height - 1) * 3 * 2];
        triangleList = new List<int>();
        
        Vector3 offset = new Vector3(width * horizontalScale / 2f, 0f, _height * horizontalScale / 2f);
        //int triangleIndex = 0;
        
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                vertices[y * width + x] = new Vector3(x * horizontalScale, 0f, y * horizontalScale) - offset;
        
        /*
        // Fill vertices and triangles
        for (int y = 0; y < _height; y++)
            for (int x = 0; x < width; x++)
            {
                // Vertices
                vertices[y * width + x] = new Vector3(x * horizontalScale, 0f, y * horizontalScale) - offset;

                // Triangles (ignore last row / column)
                if (x != width - 1 && y != _height - 1)
                {
                    // Upper Right
                    triangles[triangleIndex++] = y * width + x;
                    triangles[triangleIndex++] = (y + 1) * width + x + 1;
                    triangles[triangleIndex++] = y * width + x + 1;
                    
                    // Bottom Left
                    triangles[triangleIndex++] = y * width + x;
                    triangles[triangleIndex++] = (y + 1) * width + x;
                    triangles[triangleIndex++] = (y + 1) * width + x + 1;
                }
            }
        //*/
    }

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
        //mesh.triangles = triangles;
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
