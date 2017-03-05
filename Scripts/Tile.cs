using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class Tile : MonoBehaviour {

	public Vector2 nodePosition = Vector2.zero;
	public List<Tile> neighbours = new List<Tile>();
    public TileType type;
	public bool isWalkable = true;
	public bool isTrap = false;
	public bool isTrapRevealed = false;
    public bool isTrapDefused = false;
	public bool isHighlighted = false;
	//public bool eventTrigger = false;
	GameManager gameManager;
	TileMap mapInstance;
	public int roomNumber;

	Renderer tileRenderer;
    Animator anim;

	// Use this for initialization
	void Start () {
        mapInstance = transform.parent.GetComponent<TileMap>();
        gameManager = FindObjectOfType<GameManager>();
        GenerateNeighbours();
        tileRenderer = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
        if(this.type == TileType.Wall)
        {
            ManageWallShape();
        }
    }
	
	// Update is called once per frame
	void Update () {
        ManageTileColors();
    }

    void ManageTileColors()
    {
        if (tileRenderer)
        {
            tileRenderer.material.color = Color.white;
            //if (isTrap)
            //    tileRenderer.material.color = Color.yellow;

            if (isTrapRevealed)
            {
                //    tileRenderer.material.color = Color.green;
                anim.SetBool("revealed", isTrapRevealed);
            }
            //if (!isTrap && !isTrapRevealed)
            //    tileRenderer.material.color = Color.white;

            if (isHighlighted)
                tileRenderer.material.color = Color.cyan;
            if (gameManager.colorTileRooms)
            {
                if (roomNumber % 5 == 1)
                    tileRenderer.material.color = Color.red;
                if (roomNumber % 5 == 2)
                    tileRenderer.material.color = Color.cyan;
                if (roomNumber % 5 == 3)
                    tileRenderer.material.color = Color.blue;
                if (roomNumber % 5 == 4)
                    tileRenderer.material.color = Color.yellow;
                if (roomNumber % 5 == 0 && roomNumber != 0)
                    tileRenderer.material.color = Color.green;
            }
        }
    }

	void GenerateNeighbours() {
		if(mapInstance == null)
			Debug.Log("map instance è NULL");
		Vector2 neighbour = new Vector2();
		if(nodePosition.x > 0) { //left one
			neighbour = new Vector2(nodePosition.x - 1, nodePosition.y);
			neighbours.Add(mapInstance.tileMapOfTiles[(int)neighbour.x, (int)neighbour.y]);
		}
		if(nodePosition.y > 0) { //up one
			neighbour = new Vector2(nodePosition.x, nodePosition.y - 1);
			neighbours.Add(mapInstance.tileMapOfTiles[(int)neighbour.x, (int)neighbour.y]);
		}
		if(nodePosition.x < mapInstance.mapSizeX - 1) { //right one
			neighbour = new Vector2(nodePosition.x + 1, nodePosition.y);
			neighbours.Add(mapInstance.tileMapOfTiles[(int)neighbour.x, (int)neighbour.y]);
		}
		if(nodePosition.y < mapInstance.mapSizeY - 1) { //down one
			neighbour = new Vector2(nodePosition.x, nodePosition.y + 1);
			neighbours.Add(mapInstance.tileMapOfTiles[(int)neighbour.x, (int)neighbour.y]);
		}
	}

    void ManageWallShape()
    {
        bool top = false, bottom = false, right = false, left = false;
        foreach(Tile t in neighbours)
        {
            if(t.type == TileType.Wall)
            {
                if(t.nodePosition.x > this.nodePosition.x && t.nodePosition.y == this.nodePosition.y)   //we have a wall at our right
                {
                    right = true;
                }
                if (t.nodePosition.x < this.nodePosition.x && t.nodePosition.y == this.nodePosition.y)   //we have a wall at our left
                {
                    left = true;
                }
                if (t.nodePosition.y > this.nodePosition.y && t.nodePosition.x == this.nodePosition.x)   //we have a wall at our bottom
                {
                    bottom = true;
                }
                if (t.nodePosition.y < this.nodePosition.y && t.nodePosition.x == this.nodePosition.x)   //we have a wall at our up
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

	public float DistanceTo(Tile otherTile) {
		if(!otherTile.isWalkable)
			return Mathf.Infinity;
		if(gameManager.IsTileOccupiedByPlayer(otherTile.nodePosition)) {
			return Mathf.Infinity;
		}
        float distance = (nodePosition.x - otherTile.nodePosition.x) * (nodePosition.x - otherTile.nodePosition.x) + 
                         (nodePosition.y - otherTile.nodePosition.y) * (nodePosition.y - otherTile.nodePosition.y);
        return distance;
	}

	public void ActivateTrap() {
		/* CODICE PER LE CONSEGUENZE DELL'ATTIVAZIONE DI UNA TRAPPOLA
		 * 
		 * 
		 */
	}

	public void DefuseTrap() {
        /* CODICE PER LA DISATTIVAZIONE DI UNA TRAPPOLA
		 * 
		 * 
		 */
        isTrapDefused = true;
        anim.SetBool("defused", isTrapDefused);
	}

	void OnMouseOver() {
		///*########################*/
		//// codice per usare il mouse per cambiare una tile in una trappola
		//if(Input.GetMouseButtonDown(0)) {
		////	Debug.Log("Cambio la tile in posizione: (" + nodePosition.x + ", " + nodePosition.y + ").");
		//	isTrap = !isTrap;
		//}
		////fine codice per cambiare una tile in una trappola

		if(Input.GetMouseButtonDown(0))
        {
			Debug.Log("Premuta la tile: " + nodePosition.ToString());
			gameManager.tilePressed = nodePosition;
		}
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("Provo a defusare la tile: " + nodePosition.ToString());
            ThiefPlayer thief = FindObjectOfType<ThiefPlayer>();
            thief.DefuseTrap(this);
        }
	}

}
