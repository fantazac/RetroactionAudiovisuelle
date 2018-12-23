using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static MapTerrain World;
    public static ParticleManager ParticleManager;
    
    public static Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int((int) worldPosition.x, (int) worldPosition.z);
    }
}
