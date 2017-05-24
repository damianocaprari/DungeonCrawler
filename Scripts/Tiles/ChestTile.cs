using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestTile : Tile {

    private Animator anim;

    public event SelectionEvent OnChestSelection;
    public event ActionEvent OnChestAction;

    // Use this for initialization
    new void Start() {
        base.Start();
        type = TileType.CHEST;
        isWalkable = false;
        isChestClosed = true;
        anim = GetComponent<Animator>();
        FindObjectOfType<MouseEventsFSM>().OnSelectionStateChange += this.SelectedChest;
    }

    // Update is called once per frame
    new void Update() {
        base.Update();
        //if(Input.GetKeyDown(KeyCode.C) && isChestClosed) {
        //    StartCoroutine("OpenChest");
        //}
    }

    protected override void OnMouseOver() {
        if(Input.GetMouseButtonDown(0)) {
            if(OnChestSelection != null) {
                OnChestSelection(SelectionStates.CHEST_SELECTED, nodePosition);
                Debug.Log("Cliccato 0 in " + this.type + " e lancio un Selection tipo: " + SelectionStates.CHEST_SELECTED);
            }
        }
        if(Input.GetMouseButtonDown(1)) {
            if(OnChestAction != null) {
                if(isChestClosed) {
                    OnChestAction(ActionStates.OPEN_CHEST, nodePosition);
                    Debug.Log("Cliccato 1 in " + this.type + " e lancio un Action tipo: " + ActionStates.OPEN_CHEST);

                }
                else {
                    OnChestAction(ActionStates.OTHER, nodePosition);
                    Debug.Log("Cliccato 1 in " + this.type + " e lancio un Action tipo: " + ActionStates.OTHER);
                }
            }
        }
    }

    private void SelectedChest(SelectionStates state, Vector2 position) {
        if(state.Equals(SelectionStates.CHEST_SELECTED) && position.Equals(nodePosition)) {
            /* CODE TO HANDLE ACTIONS LINKED TO THE SELECTION OF THE CHEST */
        }
        else {
            /* else path */
        }
    }

    public void OpenChest() {
        if(isChestClosed) {
            StartCoroutine("OpenChestCoroutine");
        }
    }

    //finish to write this function
    public IEnumerator OpenChestCoroutine() {
        //this code manages the visuals
        anim.SetTrigger("IsOpening");
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("IsOpen");
        isChestClosed = false;

        /* TODO
         * add the loot code here 
         *
         */
    }
}
