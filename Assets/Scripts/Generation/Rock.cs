using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public int Level { get; protected set; }

    private void Start()
    {
        Level = 0;
        // Todo reposition & resize collider
    }

    public void LevelUp()
    {
        if (Level < 3)
        {
            Level++;
            UpdateUvs();
        }
    }

    public void UpdateUvs()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        Vector2[] uvs = mesh.uv;
        
        // Front
        uvs[0] = new Vector2(Level, 0);
        uvs[1] = new Vector2(Level + 1, 0);
        uvs[2] = new Vector2(Level, 1);
        uvs[3] = new Vector2(Level + 1, 1);
     
        // Top
        uvs[8] = new Vector2(Level, 0);
        uvs[9] = new Vector2(Level + 1, 0);
        uvs[4] = new Vector2(Level, 1);
        uvs[5] = new Vector2(Level + 1, 1);
        
        // Back
        uvs[10] = new Vector2(Level, 0);
        uvs[11] = new Vector2(Level + 1, 0);
        uvs[6] = new Vector2(Level, 1);
        uvs[7] = new Vector2(Level + 1, 1);
     
        // Bottom
        uvs[12] = new Vector2(Level, 1);
        uvs[14] = new Vector2(Level + 1, 0);
        uvs[15] = new Vector2(Level + 1, 1);
        uvs[13] = new Vector2(Level, 0);
     
        // Left
        uvs[16] = new Vector2(Level, 1);
        uvs[18] = new Vector2(Level + 1, 0);
        uvs[19] = new Vector2(Level + 1, 1);
        uvs[17] = new Vector2(Level, 0);
     
        // Right        
        uvs[20] = new Vector2(Level + 1, 0);
        uvs[21] = new Vector2(Level, 0);
        uvs[22] = new Vector2(Level, 1);
        uvs[23] = new Vector2(Level + 1, 1);
     
        mesh.uv = uvs;
    }
}
