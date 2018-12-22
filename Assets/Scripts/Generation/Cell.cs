using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool Walkable { get; protected set; }
    public Rock Rock;
    
    public Cell(bool walkable)
    {
        Walkable = walkable;
    }
}
