using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static MapTerrain World;
    public static PlayerController Player;
    
    public static Vector2Int GetPlayerActionPosition(Transform player)
    {
        float playerRange = .5f; // Distance the player 'arms' can reach from player position
        
        Vector3 actionPosition = player.position + player.rotation * Vector3.forward * playerRange;
        
        return GetGridPosition(actionPosition);
    }
    
    public static Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int((int) worldPosition.x, (int) worldPosition.z);
    }
}
