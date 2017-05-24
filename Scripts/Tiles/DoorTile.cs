using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTile : Tile {

    private Renderer tileRenderer;
    private Animator anim;    
    
    public event SelectionEvent OnDoorSelection;    
    public event ActionEvent OnDoorAction;

    // Use this for initialization
    new void Start() {
        base.Start();
        type = TileType.DOOR;
        isWalkable = false;
        isDoorClosed = true;
        anim = GetComponent<Animator>();
        tileRenderer = GetComponent<Renderer>();
        DetermineOrientation();
    }

    // Update is called once per frame
    new void Update() {
        base.Update();
        ManageTileColors();
        //if(Input.GetKeyDown(KeyCode.D)) {
        //    //should be when a player clicks on a door with (1)
        //    StartCoroutine("OpenDoor");
        //}
    }

    void DetermineOrientation() {
        foreach(Tile t in neighbours) {
            if(t.type == TileType.WALL) {
                if(t.nodePosition.x == this.nodePosition.x) {
                    anim.SetBool("IsVertical", true);
                }
                else {
                    anim.SetBool("IsVertical", false);
                }
                return;
            }
        }
    }

    void ManageTileColors() {
        if(tileRenderer) {
            tileRenderer.material.color = Color.white;            
            if(isHighlightedWalk) {
                transform.Find("HighLightMarkerWalk").gameObject.SetActive(true);
            }
            else {
                transform.Find("HighLightMarkerWalk").gameObject.SetActive(false);
            }
            if(isHighlightedAttack) {
                transform.Find("HighLightMarkerAttack").gameObject.SetActive(true);
            }
            else {
                transform.Find("HighLightMarkerAttack").gameObject.SetActive(false);
            }
        }
    }

    public void OpenDoor() {
        if(isDoorClosed) {
            StartCoroutine("OpenDoorCoroutine");
        }
    }

    public IEnumerator OpenDoorCoroutine() {
        //this code manages the visuals
        anim.SetTrigger("IsOpening");
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("IsOpen");
        isWalkable = true;
        isDoorClosed = false;
    }

    protected override void OnMouseOver() {
        if(Input.GetMouseButtonDown(0)) {
            if(gameManager.IsTileOccupiedByPlayer(this)) {  //tile occupied by a character, select it no matter what (trap has been fired or defused anyway)
                Player p = gameManager.GetPlayerByTile(this);
                p.FirePlayerSelectionEvent();
            }
            else {
                if(OnDoorSelection != null) {
                    OnDoorSelection(SelectionStates.OTHER, nodePosition);
                    Debug.Log("Cliccato 0 in " + this.type + " e lancio un Selection tipo: " + SelectionStates.OTHER);
                }
            }
        }
        if(Input.GetMouseButtonDown(1)) {
            if(gameManager.IsTileOccupiedByPlayer(this)) { //tile occupied by a character, select it no matter what (trap has been fired or defused anyway)
                Player p = gameManager.GetPlayerByTile(this);
                p.FirePlayerActionEvent();
            }
            else {
                if(OnDoorAction != null) {
                    if(isDoorClosed) { //door still closed -> can action on the door to open it
                        OnDoorAction(ActionStates.OPEN_DOOR, nodePosition);
                        Debug.Log("Cliccato 1 in " + this.type + " e lancio un Action tipo: " + ActionStates.OPEN_DOOR);
                    }
                    else { //trap hidden or defused, treat it as a ground tile
                        OnDoorAction(ActionStates.MOVEMENT, nodePosition);
                        Debug.Log("Cliccato 1 in " + this.type + " e lancio un Action tipo: " + ActionStates.MOVEMENT);
                    }
                }
            }
        }
    }
}
