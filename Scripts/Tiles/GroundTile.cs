using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : Tile {

    private Renderer tileRenderer;
    private Animator anim;

    public event SelectionEvent OnGroundSelection;
    public event ActionEvent OnGroundAction;

    // Use this for initialization
    new void Start() {
        base.Start();
        type = TileType.GROUND;
        isWalkable = true;
        tileRenderer = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    new void Update() {
        base.Update();
        ManageTileColors();
    }

    void ManageTileColors() {
        if(tileRenderer) {
            tileRenderer.material.color = Color.white;

            if(isTrapRevealed) {
                anim.SetBool("revealed", isTrapRevealed);
            }
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

    protected override void OnMouseOver() {
        if(Input.GetMouseButtonDown(0)) {
            if(gameManager.IsTileOccupiedByPlayer(this)) {
                Player p = gameManager.GetPlayerByTile(this);
                p.FirePlayerSelectionEvent();
            }
            else {
                if(OnGroundSelection != null) {
                    OnGroundSelection(SelectionStates.OTHER, nodePosition);
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
                if(OnGroundAction != null) {
                    OnGroundAction(ActionStates.MOVEMENT, nodePosition);
                    Debug.Log("Cliccato 1 in " + this.type + " e lancio un Action tipo: " + ActionStates.MOVEMENT);
                }
            }
        }
    }

}
