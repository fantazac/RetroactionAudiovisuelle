using UnityEngine;

public class Rock : MonoBehaviour
{
    public int Level { get; protected set; }

    private int health;

    private Rock()
    {
        Level = 0;
        health = 3;
    }

    private void Start()
    {
        // Todo reposition & resize collider
    }

    public int Hit()
    {
        return --health;
    }

    public void LevelUp()
    {
        Level++;
        UpdateUvs();
        if (Level == 3)
        {
            Level++;
        }
    }

    private void UpdateUvs()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        Vector2[] uvs = mesh.uv;

        // Front
        uvs[0] = new Vector2(Level, 3);
        uvs[1] = new Vector2(Level + 1, 3);
        uvs[2] = new Vector2(Level, 4);
        uvs[3] = new Vector2(Level + 1, 4);

        // Top
        uvs[8] = new Vector2(Level, 3);
        uvs[9] = new Vector2(Level + 1, 3);
        uvs[4] = new Vector2(Level, 4);
        uvs[5] = new Vector2(Level + 1, 4);

        // Back
        uvs[10] = new Vector2(Level, 3);
        uvs[11] = new Vector2(Level + 1, 3);
        uvs[6] = new Vector2(Level, 4);
        uvs[7] = new Vector2(Level + 1, 4);

        // Bottom
        uvs[12] = new Vector2(Level, 4);
        uvs[14] = new Vector2(Level + 1, 3);
        uvs[15] = new Vector2(Level + 1, 4);
        uvs[13] = new Vector2(Level, 3);

        // Left
        uvs[16] = new Vector2(Level, 4);
        uvs[18] = new Vector2(Level + 1, 3);
        uvs[19] = new Vector2(Level + 1, 4);
        uvs[17] = new Vector2(Level, 3);

        // Right        
        uvs[20] = new Vector2(Level + 1, 3);
        uvs[21] = new Vector2(Level, 3);
        uvs[22] = new Vector2(Level, 4);
        uvs[23] = new Vector2(Level + 1, 4);

        mesh.uv = uvs;
    }
}
