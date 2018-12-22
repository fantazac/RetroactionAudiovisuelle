using Boo.Lang;
using UnityEngine;

public class ProceduralMesh
{
    private List<Vector3> vertices;
    private List<int> triangles;
    private List<Vector2> uvs;
    private Grid grid;

    public ProceduralMesh(Grid grid, Transform parent)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
        this.grid = grid;
        int borderSize = 5;
        float height = 1f;
        
        for (int y = -borderSize; y < grid.Height + borderSize; y++)
        {   
            for (int x = -borderSize; x < grid.Width + borderSize; x++)
            {
                if (grid.Get(x, y)) // Ground
                    AddTile(x, y, 0f, new Vector2(Random.value >= .2f ? 0f : 1f, 1f));
                else // Walls
                {
                    AddTile(x, y, height, new Vector2(1f, 0f));
                    AddWalls(x, y, height);
                }
            }
        }

        GameObject holder = new GameObject("Ground");
        holder.transform.parent = parent;
        MeshFilter meshFilter = holder.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = holder.AddComponent<MeshRenderer>();
        meshRenderer.material = Utility.World.GroundMaterial;
        
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    private void AddTile(int x, int y, float height, Vector2 uvOffset)
    {
        int triangleIndex = vertices.Count;
        
        // Vertices
        vertices.Add(new Vector3(x, height, y));
        vertices.Add(new Vector3(x + 1, height, y)); 
        vertices.Add(new Vector3(x, height, y + 1));
        vertices.Add(new Vector3(x + 1, height, y + 1));
        // We generate all four vertices for the tile (and no longer use neighbour tile vertices) so we can edit per tile UVs
        
        // UVs
        uvs.Add(new Vector2(0, 0) + uvOffset);
        uvs.Add(new Vector2(1, 0) + uvOffset);
        uvs.Add(new Vector2(0, 1) + uvOffset);
        uvs.Add(new Vector2(1, 1) + uvOffset);

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

    private void AddWalls(int x, int y, float height)
    {
        int triangleIndex = vertices.Count;
        int count = 0;
        
        //todo: FIX
        
        // Top
        if (grid.Get(x, y + 1))
        {
            vertices.Add(new Vector3(x, 0, y + 1));
            vertices.Add(new Vector3(x + 1, 0, y + 1));
            vertices.Add(new Vector3(x, height, y + 1));
            vertices.Add(new Vector3(x + 1, height, y + 1));
            count++;
        }
        
        // Bottom
        if (grid.Get(x, y - 1))
        {
            vertices.Add(new Vector3(x + 1, 0, y));
            vertices.Add(new Vector3(x, 0, y));
            vertices.Add(new Vector3(x + 1, height, y));
            vertices.Add(new Vector3(x, height, y));
            count++;
        }
        
        // Right
        if (grid.Get(x + 1, y))
        {
            vertices.Add(new Vector3(x + 1, 0, y + 1));
            vertices.Add(new Vector3(x + 1, 0, y));
            vertices.Add(new Vector3(x + 1, height, y + 1));
            vertices.Add(new Vector3(x + 1, height, y));
            count++;
        }
        
        // Left
        if (grid.Get(x - 1, y))
        {
            vertices.Add(new Vector3(x, 0, y));
            vertices.Add(new Vector3(x, 0, y + 1));
            vertices.Add(new Vector3(x, height, y));
            vertices.Add(new Vector3(x, height, y + 1));
            count++;
        }
        
        for (int i = 0; i < count; i++)
        {
            // UVs
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
    
            // Triangles
            // Upper Right
            triangles.Add(triangleIndex);
            triangles.Add(triangleIndex + 3);
            triangles.Add(triangleIndex + 2);
    
            // Bottom Left
            triangles.Add(triangleIndex);
            triangles.Add(triangleIndex + 1);
            triangles.Add(triangleIndex + 3);
        }
    }
}
