using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentTile : Tile {

    Renderer tileRenderer;
    //Collider2D tileCollider;

    // Use this for initialization
    new void Start() {
        base.Start();
        type = TileType.TRANSPARENT;
        isWalkable = false;
        tileRenderer = GetComponent<Renderer>();
        tileRenderer.enabled = false;
        //tileCollider = GetComponent<Collider2D>();
        //tileCollider.enabled = false;
    }

    // Update is called once per frame
    new void Update() {
        base.Update();
    }
}
