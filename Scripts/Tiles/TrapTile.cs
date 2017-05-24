using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrapTile : Tile {

    private Renderer tileRenderer;
    private Animator anim;

    public event SelectionEvent OnTrapSelection;
    public event ActionEvent OnTrapAction;

    // Use this for initialization
    new void Start() {
        base.Start();
        type = TileType.TRAP;
        isWalkable = true;
        isTrap = true;
        isTrapRevealed = false;
        isTrapDefused = false;
        tileRenderer = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
        FindObjectOfType<MouseEventsFSM>().OnSelectionStateChange += this.SelectedTrap;
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

    override public void ActivateTrap() {
        /* CODICE PER LE CONSEGUENZE DELL'ATTIVAZIONE DI UNA TRAPPOLA
		 * 
		 * 
		 */
    }

    override public void DefuseTrap() {
        /* CODICE PER LA DISATTIVAZIONE DI UNA TRAPPOLA
		 * 
		 * 
		 */
        isTrapDefused = true;
        anim.SetBool("defused", isTrapDefused);
    }

    protected override void OnMouseOver() {
        if(Input.GetMouseButtonDown(0)) {
            if(gameManager.IsTileOccupiedByPlayer(this)) {  //tile occupied by a character, select it no matter what (trap has been fired or defused anyway)
                Player p = gameManager.GetPlayerByTile(this);
                p.FirePlayerSelectionEvent();
            }
            else {
                if(OnTrapSelection != null) {
                    if(isTrapRevealed && !isTrapDefused) {  //trap visible and still active -> can select the trap to show what it does
                        OnTrapSelection(SelectionStates.TRAP_SELECTED, nodePosition);
                        Debug.Log("Cliccato 0 in " + this.type + " e lancio un Selection tipo:\t" + SelectionStates.TRAP_SELECTED);
                    }
                    else { //trap hidden or defused, treat it as a ground tile
                        OnTrapSelection(SelectionStates.OTHER, nodePosition);
                        Debug.Log("Cliccato 0 in " + this.type + " e lancio un Selection tipo:\t" + SelectionStates.OTHER);
                    }
                }
            }
        }
        if(Input.GetMouseButtonDown(1)) {
            if(gameManager.IsTileOccupiedByPlayer(this)) { //tile occupied by a character, select it no matter what (trap has been fired or defused anyway)
                Player p = gameManager.GetPlayerByTile(this);
                p.FirePlayerActionEvent();
            }
            else {
                if(OnTrapAction != null) {
                    if(isTrapRevealed && !isTrapDefused) { //trap visible and still active -> can action on the trap to defuse it
                        OnTrapAction(ActionStates.DEFUSE_TRAP, nodePosition);
                        Debug.Log("Cliccato 1 in " + this.type + " e lancio un Action tipo: " + ActionStates.DEFUSE_TRAP);
                    }
                    else { //trap hidden or defused, treat it as a ground tile
                        OnTrapAction(ActionStates.MOVEMENT, nodePosition);
                        Debug.Log("Cliccato 1 in " + this.type + " e lancio un Action tipo: " + ActionStates.MOVEMENT);
                    }
                }
            }
        }
    }

    private void SelectedTrap(SelectionStates state, Vector2 position) {
        if(state.Equals(SelectionStates.TRAP_SELECTED) && position.Equals(nodePosition)) {
            /* CODE WHEN THE TRAP IS SELECTED */
        }
        else {
            /* else state */
        }
    }
}
