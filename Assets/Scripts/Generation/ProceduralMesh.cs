using Boo.Lang;
using UnityEngine;

public class ProceduralMesh
{
    private List<Vector3> vertices;
    private List<int> triangles;
    private List<Vector2> uvs;

    public ProceduralMesh(CellularAutomata ca, float scale = 1)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();

        for (int y = 0; y < ca.Height; y++)
        {   
            for (int x = 0; x < ca.Width; x++)
            {
                if (!ca.Get(x, y))
                    AddTile(x, y);
            }
        }
    }

    private void AddTile(int x, int y)
    {
        int triangleIndex = vertices.Count;
        int offset = Random.Range(0, 4);
        
        // Vertices
        vertices.Add(new Vector3(x, 0f, y));
        vertices.Add(new Vector3(x + 1, 0f, y)); 
        vertices.Add(new Vector3(x, 0f, y + 1));
        vertices.Add(new Vector3(x + 1, 0f, y + 1));
        // We generate all four vertices per tile (and no longer use neighbour tile vertices) so we can edit per tile UVs
        
        // UVs
        uvs.Add(new Vector2(0 + offset, 0));
        uvs.Add(new Vector2(1 + offset, 0));
        uvs.Add(new Vector2(0 + offset, 1));
        uvs.Add(new Vector2(1 + offset, 1));

        // Triangles
        // Upper Right
        triangles.Add(triangleIndex);
        triangles.Add(triangleIndex + 3);
        triangles.Add(triangleIndex + 1);

        // Bottom Left
        triangles.Add(triangleIndex);
        triangles.Add(triangleIndex + 2);
        triangles.Add(triangleIndex + 3);
        
    }
    
    public Mesh GetMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        
        return mesh;
    }
}
