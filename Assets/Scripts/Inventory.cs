using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    // static int size = 5;
    public TileInfo[] contents;

    public Inventory(TileInfo[] contents)
    {
        this.contents = contents;
    }
}
