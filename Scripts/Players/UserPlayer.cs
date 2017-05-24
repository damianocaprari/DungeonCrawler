using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class UserPlayer : Player {

    private bool isSelected;

    public int weaponSlots;
    public int arctifactSlots;
    public delegate void SelectionEvent(SelectionStates newState, Vector2 newPosition);
    public event SelectionEvent OnPlayerSelection;
    public delegate void ActionEvent(ActionStates newState, Vector2 newPosition);
    public event ActionEvent OnPlayerAction;

    // Use this for initialization
    new protected void Start() {
        faction = PlayerFaction.PlayerControlled;
        this.SetMarkerColor(Color.green);
        FindObjectOfType<MouseEventsFSM>().OnSelectionStateChange += this.SelectedPlayer;
        FindObjectOfType<MouseEventsFSM>().OnActionStateChange += this.Actions;
        base.Start();
    }

    // Update is called once per frame
    new protected void Update() {
        base.Update();
    }

    virtual protected void InitHP() { }
    virtual protected void InitSpellPoints() { }

    override public void FirePlayerSelectionEvent() {
        if(OnPlayerSelection != null) {
            OnPlayerSelection(SelectionStates.PLAYER_SELECTED, position);
            Debug.Log("Cliccato 0 in UserPlayer" + " e lancio un Selection tipo: " + SelectionStates.PLAYER_SELECTED);
        }
    }

    override public void FirePlayerActionEvent() {
        if(OnPlayerAction != null) {
            OnPlayerAction(ActionStates.HEAL, position);
            Debug.Log("Cliccato 1 in UserPlayer" + " e lancio un Action tipo: " + ActionStates.HEAL);
        }
    }

    private void SelectedPlayer(SelectionStates state, Vector2 position) {
        if(state.Equals(SelectionStates.PLAYER_SELECTED) && position.Equals(this.position)) {
            isSelected = true;
            Debug.Log("Selezionato il PLAYER " + unitName);
            map.RemoveAllHighlights();
            if(currentActionPoints > 0) {
                gameManager.pathFind.HighLightReachableTiles(position, currentMovementPoints, attackRange);
            }
        }
        else {
            isSelected = false;
        }
    }

    private void Actions(ActionStates state, Vector2 targetPosition) {
        if(isSelected && currentActionPoints > 0) {
            map.RemoveAllHighlights();
            switch(state) {
                case ActionStates.ATTACK:
                    Debug.Log(state + " Player on " + targetPosition.ToString());
                    Attack(targetPosition);
                    break;
                case ActionStates.HEAL:
                    Debug.Log(state + " Player on " + targetPosition.ToString());
                    Heal(targetPosition);
                    break;
                case ActionStates.DEFUSE_TRAP:
                    Debug.Log(state + " Trap on " + targetPosition.ToString());
                    DefuseTrap(targetPosition);
                    break;
                case ActionStates.MOVEMENT:
                    Debug.Log(state + " to " + targetPosition.ToString());
                    Movement(targetPosition);
                    break;
                case ActionStates.OPEN_CHEST:
                    Debug.Log(state + " on " + targetPosition.ToString());
                    OpenChest(targetPosition);
                    break;
                case ActionStates.OPEN_DOOR:
                    Debug.Log(state + " on " + targetPosition.ToString());
                    OpenDoor(targetPosition);
                    break;
                case ActionStates.OTHER:
                    Debug.Log(state + " - " + targetPosition.ToString());
                    break;
            }
        }
    }

    private void Attack(Vector2 targetPosition) {
        int distanceToTarget = (int)(Mathf.Abs(position.x - targetPosition.x) + Mathf.Abs(position.y - targetPosition.y));
        if(distanceToTarget > attackRange) { //if absolute distance is greater than attackrange, return
            Debug.Log("absolute distance (" + distanceToTarget + ") is greater than attackrange (" + attackRange + ")");
        }
        else {
            List<Tile> pathToTarget = gameManager.pathFind.GeneratePathTo(position, targetPosition);
            if(pathToTarget.Count > attackRange) { //if actual distance is greater than attackrange, return
                Debug.Log("actual distance (" + pathToTarget.Count + ") is greater than attackrange (" + attackRange + ")");
            }
            else {
                Debug.Log("target is in attackRange. Attacking him");
                DealDamage(gameManager.GetPlayerByTile(targetPosition));
            }
        }
    }

    protected virtual void Heal(Vector2 targetPosition) { }
    protected virtual void DefuseTrap(Vector2 targetPosition) { }

    private void Movement(Vector2 targetPosition) {
        int distanceToTarget = (int)(Mathf.Abs(position.x - targetPosition.x) + Mathf.Abs(position.y - targetPosition.y));
        if(distanceToTarget > currentMovementPoints) { //if absolute distance is greater than currentMovementPoints, return
            Debug.Log("absolute distance (" + distanceToTarget + ") is greater than currentMovementPoints (" + currentMovementPoints + ")");
        }
        else {
            List<Tile> path = gameManager.pathFind.GeneratePathTo(position, targetPosition);
            if(path.Count != 0) {
                currentPath = path;
                //gameManager.tileMap.RemoveAllHighlights();
                currentActionPoints--;
            }
        }
    }

    private void OpenChest(Vector2 targetPosition) {
        int distanceToTarget = (int)(Mathf.Abs(position.x - targetPosition.x) + Mathf.Abs(position.y - targetPosition.y));
        if(distanceToTarget > 1) { //if absolute distance is greater than 1, return
            Debug.Log("absolute distance (" + distanceToTarget + ") is greater than 1");
        }
        else {
            ((ChestTile)gameManager.tileMap.GetTileFromCoord(targetPosition)).OpenChest();
        }
    }

    private void OpenDoor(Vector2 targetPosition) {
        int distanceToTarget = (int)(Mathf.Abs(position.x - targetPosition.x) + Mathf.Abs(position.y - targetPosition.y));
        if(distanceToTarget > 1) { //if absolute distance is greater than 1, return
            Debug.Log("absolute distance (" + distanceToTarget + ") is greater than 1");
        }
        else {
            ((DoorTile)gameManager.tileMap.GetTileFromCoord(targetPosition)).OpenDoor();
        }
    }



}
