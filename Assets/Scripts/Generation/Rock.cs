using UnityEngine;

public class Rock : MonoBehaviour
{
    public int Value { get; protected set; }

    private int health;

    MeshFilter meshFilter;
    Mesh mesh;
    Vector2[] uvs;

    private Rock()
    {
        Value = 0;
        health = 3;
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        uvs = mesh.uv;
    }

    private void Start()
    {
        UpdateUvs();
        if (Value == 3)
        {
            Value++;
            Utility.ParticleManager.PlaceDiamond(transform);
        }
    }

    public int Hit()
    {
        Utility.ParticleManager.PlayRockHit(transform.position);
        
        for (int i = 0; i < uvs.Length; i++)
            uvs[i].y--;
        mesh.uv = uvs;
        
        StaticObjects.SoundEffectManager.PlaySound(4);
        return --health;
    }

    public void LevelUp()
    {
        Value++;
    }

    private void UpdateUvs()
    {
        // Front
        uvs[0] = new Vector2(Value, 3);
        uvs[1] = new Vector2(Value + 1, 3);
        uvs[2] = new Vector2(Value, 4);
        uvs[3] = new Vector2(Value + 1, 4);

        // Top
        uvs[8] = new Vector2(Value, 3);
        uvs[9] = new Vector2(Value + 1, 3);
        uvs[4] = new Vector2(Value, 4);
        uvs[5] = new Vector2(Value + 1, 4);

        // Back
        uvs[10] = new Vector2(Value, 3);
        uvs[11] = new Vector2(Value + 1, 3);
        uvs[6] = new Vector2(Value, 4);
        uvs[7] = new Vector2(Value + 1, 4);

        // Bottom
        uvs[12] = new Vector2(Value, 4);
        uvs[14] = new Vector2(Value + 1, 3);
        uvs[15] = new Vector2(Value + 1, 4);
        uvs[13] = new Vector2(Value, 3);

        // Left
        uvs[16] = new Vector2(Value, 4);
        uvs[18] = new Vector2(Value + 1, 3);
        uvs[19] = new Vector2(Value + 1, 4);
        uvs[17] = new Vector2(Value, 3);

        // Right        
        uvs[20] = new Vector2(Value + 1, 3);
        uvs[21] = new Vector2(Value, 3);
        uvs[22] = new Vector2(Value, 4);
        uvs[23] = new Vector2(Value + 1, 4);

        mesh.uv = uvs;
    }
}
