using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTile : Tile {

    private Animator anim;

    // Use this for initialization
    new void Start() {
        base.Start();
        type = TileType.WALL;
        isWalkable = false;
        anim = GetComponent<Animator>();
        ManageWallShape();
    }

    // Update is called once per frame
    new void Update() {
        base.Update();
    }

    void ManageWallShape() {
        bool top = false, bottom = false, right = false, left = false;
        foreach(Tile t in neighbours) {
            if(t.type == TileType.WALL) {
                if(t.nodePosition.x > this.nodePosition.x && t.nodePosition.y == this.nodePosition.y)   //we have a wall at our right
                {
                    right = true;
                }
                if(t.nodePosition.x < this.nodePosition.x && t.nodePosition.y == this.nodePosition.y)   //we have a wall at our left
                {
                    left = true;
                }
                if(t.nodePosition.y > this.nodePosition.y && t.nodePosition.x == this.nodePosition.x)   //we have a wall at our bottom
                {
                    bottom = true;
                }
                if(t.nodePosition.y < this.nodePosition.y && t.nodePosition.x == this.nodePosition.x)   //we have a wall at our up
                {
                    top = true;
                }
            }
        }
        anim.SetBool("connectedTop", top);
        anim.SetBool("connectedBottom", bottom);
        anim.SetBool("connectedRight", right);
        anim.SetBool("connectedLeft", left);
    }
}
