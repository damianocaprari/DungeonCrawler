using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnTile : Tile {

    // Use this for initialization
    new void Start() {
        base.Start();
        type = TileType.COLUMN;
        isWalkable = false;
    }

    // Update is called once per frame
    new void Update() {
        base.Update();
    }
}
