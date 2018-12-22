using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Size { get; protected set; } // Player hitbox size
    public int Score { get; protected set; }

    private void Awake()
    {
        Utility.Player = this;
        Size = .3f;
    }

    // Update is called once per frame
    void Update()
    {
        DoAction();
    }

    void DoAction()
    {
        if (Utility.World != null)
        {
            Vector2Int pos = Utility.GetPlayerActionPosition(transform);
            int points = Utility.World.DestroyRock(pos.x, pos.y);

            if (points > 0)
                OnReceivePoints(points);
        }
    }

    private void OnReceivePoints(int value)
    {
        Score += value;
        // todo: Update UI
        // todo: Play Hit particles
    }

    private void OnDrawGizmosSelected()
    {
        // Player
        Gizmos.color = Color.grey;
        Gizmos.DrawCube(transform.position, new Vector3(.5f, .5f, .5f));
        
        // Action Zone
        Gizmos.color = Color.yellow;
        Vector3 pos = transform.position + transform.rotation * Vector3.forward * .5f;
        Gizmos.DrawLine(pos + Vector3.up, pos + Vector3.down);
        
        // Affected tile
        Gizmos.color = Color.red;
        Vector2Int coords = Utility.GetPlayerActionPosition(transform);
        Gizmos.DrawWireSphere(new Vector3(coords.x + .5f, .5f, coords.y + .5f), .2f);
    }
}
