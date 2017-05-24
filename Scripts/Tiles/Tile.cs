using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class Tile : MonoBehaviour {

    public Vector2 nodePosition = Vector2.zero;
    public List<Tile> neighbours = new List<Tile>();                // only the 4 cardinal directions
    public List<Tile> neighboursAllDirections = new List<Tile>();   // cardinal directions + diagonal corners
    public TileType type;
    public bool isWalkable = true;
    public bool isDoorClosed = false;
    public bool isChestClosed = false;
    public bool isTrap = false;
    public bool isTrapRevealed = false;
    public bool isTrapDefused = false;
    public bool isHighlightedWalk = false;
    public bool isHighlightedAttack = false;
    //public bool eventTrigger = false;
    protected GameManager gameManager;
    protected TileMap mapInstance;
    public int roomNumber;

    //protected Renderer tileRenderer;
    //protected Animator anim;
    protected Animator animHighLight;

    public delegate void SelectionEvent(SelectionStates newState, Vector2 newPosition);
    public event SelectionEvent OnOtherSelection;
    public delegate void ActionEvent(ActionStates newState, Vector2 newPosition);
    public event ActionEvent OnOtherAction;

    // Use this for initialization
    protected void Start() {
        mapInstance = transform.parent.GetComponent<TileMap>();
        gameManager = FindObjectOfType<GameManager>();
        GenerateNeighbours();
        //tileRenderer = GetComponent<Renderer>();
        //anim = GetComponent<Animator>();
        FindObjectOfType<MouseEventsFSM>().OnSelectionStateChange += this.SelectedOther;
        /* moved to subclasses */
        //if(this.type == TileType.WALL)
        //{
        //    ManageWallShape();
        //}
        //if (this.type == TileType.Ground || this.type == TileType.Trap || this.type == TileType.Door)
        //{
        //    animHighLight = transform.FindChild("HighLightMarkerWalk").GetComponent<Animator>();
        //    animHighLightAttack = transform.FindChild("HighLightMarkerAttack").GetComponent<Animator>();
        //}
    }

    // Update is called once per frame
    protected void Update() {
        /* moved to subclasses */
        //ManageTileColors();

        /* moved to subclasses */
        //if (this.type == TileType.CHEST && Input.GetKeyDown(KeyCode.C))
        //{
        //    StartCoroutine("OpenChest");
        //}
    }

    /* moved to subclasses */
    //void ManageTileColors()
    //{
    //    if (tileRenderer)
    //    {
    //        tileRenderer.material.color = Color.white;
    //        //if (isTrap)
    //        //    tileRenderer.material.color = Color.yellow;

    //        if (isTrapRevealed)
    //        {
    //            //    tileRenderer.material.color = Color.green;
    //            anim.SetBool("revealed", isTrapRevealed);
    //        }
    //        //if (!isTrap && !isTrapRevealed)
    //        //    tileRenderer.material.color = Color.white;

    //        if (isHighlightedWalk)
    //        {
    //            //tileRenderer.material.color = Color.cyan;
    //            if (this.type == TileType.GROUND || this.type == TileType.TRAP || this.type == TileType.DOOR)
    //                transform.Find("HighLightMarkerWalk").gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            if (this.type == TileType.GROUND || this.type == TileType.TRAP || this.type == TileType.DOOR)
    //                transform.Find("HighLightMarkerWalk").gameObject.SetActive(false);
    //        }
    //        if (isHighlightedAttack)
    //        {
    //            //tileRenderer.material.color = Color.red;
    //            if (this.type == TileType.GROUND || this.type == TileType.TRAP || this.type == TileType.DOOR)
    //                transform.Find("HighLightMarkerAttack").gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            if (this.type == TileType.GROUND || this.type == TileType.TRAP || this.type == TileType.DOOR)
    //                transform.Find("HighLightMarkerAttack").gameObject.SetActive(false);
    //        }
    //        if (gameManager.colorTileRooms)
    //        {
    //            if (roomNumber % 5 == 1)
    //                tileRenderer.material.color = Color.red;
    //            if (roomNumber % 5 == 2)
    //                tileRenderer.material.color = Color.cyan;
    //            if (roomNumber % 5 == 3)
    //                tileRenderer.material.color = Color.blue;
    //            if (roomNumber % 5 == 4)
    //                tileRenderer.material.color = Color.yellow;
    //            if (roomNumber % 5 == 0 && roomNumber != 0)
    //                tileRenderer.material.color = Color.green;
    //        }
    //    }
    //}

    void GenerateNeighbours() {
        if(mapInstance == null)
            Debug.Log("map instance è NULL");
        Vector2 neighbour = new Vector2();
        if(nodePosition.x > 0) { //left one
            neighbour = new Vector2(nodePosition.x - 1, nodePosition.y);
            neighbours.Add(mapInstance.GetTileFromCoord(neighbour));
        }
        if(nodePosition.y > 0) { //up one
            neighbour = new Vector2(nodePosition.x, nodePosition.y - 1);
            neighbours.Add(mapInstance.GetTileFromCoord(neighbour));
        }
        if(nodePosition.x < mapInstance.mapSizeX - 1) { //right one
            neighbour = new Vector2(nodePosition.x + 1, nodePosition.y);
            neighbours.Add(mapInstance.GetTileFromCoord(neighbour));
        }
        if(nodePosition.y < mapInstance.mapSizeY - 1) { //down one
            neighbour = new Vector2(nodePosition.x, nodePosition.y + 1);
            neighbours.Add(mapInstance.GetTileFromCoord(neighbour));
        }

        //fill neighboursAllDirections
        neighboursAllDirections.AddRange(neighbours);
        if(nodePosition.x > 0 && nodePosition.y > 0) { //left-up one
            neighbour = new Vector2(nodePosition.x - 1, nodePosition.y - 1);
            neighboursAllDirections.Add(mapInstance.GetTileFromCoord(neighbour));
        }
        if(nodePosition.x < mapInstance.mapSizeX - 1 && nodePosition.y > 0) { //right-up one
            neighbour = new Vector2(nodePosition.x + 1, nodePosition.y - 1);
            neighboursAllDirections.Add(mapInstance.GetTileFromCoord(neighbour));
        }
        if(nodePosition.x < mapInstance.mapSizeX - 1 && nodePosition.y < mapInstance.mapSizeY - 1) { //right-down one
            neighbour = new Vector2(nodePosition.x + 1, nodePosition.y + 1);
            neighboursAllDirections.Add(mapInstance.GetTileFromCoord(neighbour));
        }
        if(nodePosition.x > 0 && nodePosition.y < mapInstance.mapSizeY - 1) { //left-down one
            neighbour = new Vector2(nodePosition.x - 1, nodePosition.y + 1);
            neighboursAllDirections.Add(mapInstance.GetTileFromCoord(neighbour));
        }

    }

    /* moved to subclasses */
    //void ManageWallShape()
    //{
    //    bool top = false, bottom = false, right = false, left = false;
    //    foreach(Tile t in neighbours)
    //    {
    //        if(t.type == TileType.WALL)
    //        {
    //            if(t.nodePosition.x > this.nodePosition.x && t.nodePosition.y == this.nodePosition.y)   //we have a wall at our right
    //            {
    //                right = true;
    //            }
    //            if (t.nodePosition.x < this.nodePosition.x && t.nodePosition.y == this.nodePosition.y)   //we have a wall at our left
    //            {
    //                left = true;
    //            }
    //            if (t.nodePosition.y > this.nodePosition.y && t.nodePosition.x == this.nodePosition.x)   //we have a wall at our bottom
    //            {
    //                bottom = true;
    //            }
    //            if (t.nodePosition.y < this.nodePosition.y && t.nodePosition.x == this.nodePosition.x)   //we have a wall at our up
    //            {
    //                top = true;
    //            }
    //        }
    //    }
    //    anim.SetBool("connectedTop", top);
    //    anim.SetBool("connectedBottom", bottom);
    //    anim.SetBool("connectedRight", right);
    //    anim.SetBool("connectedLeft", left);
    //}

    public void ManageHighLightShape(string childName) {
        if(this.type == TileType.GROUND || this.type == TileType.TRAP || this.type == TileType.DOOR) {
            transform.Find(childName).gameObject.SetActive(true);
            animHighLight = transform.Find(childName).GetComponent<Animator>();
            bool top = false, bottom = false, right = false, left = false;
            bool topRight = false, bottomRight = false, topLeft = false, bottomLeft = false;
            foreach(Tile t in neighboursAllDirections) {
                if(t.isHighlightedWalk || t.isHighlightedAttack) {
                    if(t.nodePosition.x > this.nodePosition.x)     // right neighbours
                    {
                        if(t.nodePosition.y < this.nodePosition.y) // node y position is upside-down
                            topRight = true;
                        else if(t.nodePosition.y > this.nodePosition.y)
                            bottomRight = true;
                        else
                            right = true;
                    }
                    else if(t.nodePosition.x < this.nodePosition.x)// left neighbours
                    {
                        if(t.nodePosition.y < this.nodePosition.y) // node y position is upside-down
                            topLeft = true;
                        else if(t.nodePosition.y > this.nodePosition.y)
                            bottomLeft = true;
                        else
                            left = true;
                    }
                    else // vertical neighbours
                    {
                        if(t.nodePosition.y < this.nodePosition.y) // node y position is upside-down
                            top = true;
                        else
                            bottom = true;
                    }
                }
            }
            animHighLight.SetBool("top", top);
            animHighLight.SetBool("bottom", bottom);
            animHighLight.SetBool("left", left);
            animHighLight.SetBool("right", right);
            animHighLight.SetBool("topRight", topRight);
            animHighLight.SetBool("topLeft", topLeft);
            animHighLight.SetBool("bottomRight", bottomRight);
            animHighLight.SetBool("bottomLeft", bottomLeft);
        }
    }

    public float DistanceTo(Tile otherTile) {
        if(!otherTile.isWalkable)
            return Mathf.Infinity;
        if(gameManager.IsTileOccupiedByPlayer(otherTile.nodePosition)) {
            return Mathf.Infinity;
        }
        float distance = Mathf.Abs(nodePosition.x - otherTile.nodePosition.x) + Mathf.Abs(nodePosition.y - otherTile.nodePosition.y);
        return distance;
    }


    virtual public void ActivateTrap() { }

    /* moved to subclasses */
    virtual public void DefuseTrap() { }

    protected virtual void OnMouseOver() {
        if(Input.GetMouseButtonDown(0)) {
            if(gameManager.IsTileOccupiedByPlayer(this)) {
                Player p = gameManager.GetPlayerByTile(this);
                p.FirePlayerSelectionEvent();
            }
            else {
                if(OnOtherSelection != null) {
                    OnOtherSelection(SelectionStates.OTHER, nodePosition);
                    Debug.Log("Cliccato 0 in " + this.type + " e lancio un Selection tipo: " + SelectionStates.OTHER);
                }
            }
        }
        if(Input.GetMouseButtonDown(1)) {
            if(gameManager.IsTileOccupiedByPlayer(this)) {
                Player p = gameManager.GetPlayerByTile(this);
                p.FirePlayerActionEvent();
            }
            else {
                if(OnOtherAction != null) {
                    OnOtherAction(ActionStates.OTHER, nodePosition);
                    Debug.Log("Cliccato 1 in " + this.type + " e lancio un Action tipo: " + ActionStates.OTHER);
                }
            }
        }
        ///*########################*/
        //// codice per usare il mouse per cambiare una tile in una trappola
        //if(Input.GetMouseButtonDown(0)) {
        ////	Debug.Log("Cambio la tile in posizione: (" + nodePosition.x + ", " + nodePosition.y + ").");
        //	isTrap = !isTrap;
        //}
        ////fine codice per cambiare una tile in una trappola

        //if(Input.GetMouseButtonDown(0)) {
        //    Debug.Log("Premuta la tile: " + nodePosition.ToString());
        //    gameManager.tilePressed = nodePosition;
        //    gameManager.mouseButtonPressed = 0;
        //}
        //if(Input.GetMouseButtonDown(1)) {
        //    if(this.type == TileType.TRAP) {
        //        Debug.Log("Provo a defusare la tile: " + nodePosition.ToString());
        //        ThiefPlayer thief = FindObjectOfType<ThiefPlayer>();
        //        thief.DefuseTrap(this);
        //        gameManager.mouseButtonPressed = 1;
        //    }
        //}
    }

    private void SelectedOther(SelectionStates state, Vector2 position) {
        if(state.Equals(SelectionStates.OTHER) && position.Equals(nodePosition)) {

        }
        else {

        }
    }

    /* moved to subclasses */
    //public IEnumerator OpenChest() //finish to write this function
    //{
    //    //this code manages the visuals
    //    anim.SetTrigger("IsOpening");
    //    yield return new WaitForSeconds(1f);
    //    anim.SetTrigger("IsOpen");


    //    //add the loot code here        
    //}

    /* moved to subclasses */
    //public void OpenDoor() {
    //    //todo code for opening the door (both visuals and logic)
    //}

}
