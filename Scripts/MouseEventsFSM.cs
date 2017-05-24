using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum SelectionStates {
    PLAYER_SELECTED,
    ENEMY_SELECTED,
    CHEST_SELECTED,
    TRAP_SELECTED,
    OTHER
}

public enum ActionStates {
    MOVEMENT,
    ATTACK,
    HEAL,
    DEFUSE_TRAP,
    OPEN_CHEST,
    OPEN_DOOR,
    OTHER
}

public class MouseEventsFSM : MonoBehaviour {

    private SelectionStates currentSelectionState;
    private ActionStates currentActionState;
    private Vector2 clickedPosition;
    private GameManager gameManager;

    public delegate void SelectionStateChange(SelectionStates newState, Vector2 position);
    public event SelectionStateChange OnSelectionStateChange;
    public delegate void ActionStateChange(ActionStates newState, Vector2 position);
    public event ActionStateChange OnActionStateChange;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine("LateStart");
    }

    void Update() {

    }

    public IEnumerator LateStart() {
        yield return new WaitForSeconds(1f);
        currentSelectionState = SelectionStates.OTHER;
        currentActionState = ActionStates.OTHER;
        if(OnSelectionStateChange != null) {
            OnSelectionStateChange(currentSelectionState, Vector2.zero);
        }
        if(OnActionStateChange != null) {
            OnActionStateChange(currentActionState, Vector2.zero);
        }
        /* Subscribe to Events */
        UserPlayer[] userPlayers = FindObjectsOfType<UserPlayer>();
        //Debug.Log("userPlayers: " + userPlayers.Count());
        foreach(UserPlayer up in userPlayers) {
            up.OnPlayerSelection += ChangeSelectionState;
            up.OnPlayerAction += ChangeActionState;
        }
        EnemyPlayer[] enemies = FindObjectsOfType<EnemyPlayer>();
        //Debug.Log("enemies: " + enemies.Count());
        foreach(EnemyPlayer e in enemies) {
            e.OnEnemySelection += ChangeSelectionState;
            e.OnEnemyAction += ChangeActionState;
        }
        var grounds = FindObjectsOfType<GroundTile>();
        //Debug.Log("grounds: " + grounds.Count());
        foreach(GroundTile g in grounds) {
            g.OnGroundSelection += ChangeSelectionState;
            g.OnGroundAction += ChangeActionState;
        }
        var doors = FindObjectsOfType<DoorTile>();
        //Debug.Log("doors: " + doors.Count());
        foreach(DoorTile d in doors) {
            d.OnDoorSelection += ChangeSelectionState;
            d.OnDoorAction += ChangeActionState;
        }
        var traps = FindObjectsOfType<TrapTile>();
        //Debug.Log("traps: " + traps.Count());
        foreach(TrapTile t in traps) {
            t.OnTrapSelection += ChangeSelectionState;
            t.OnTrapAction += ChangeActionState;
        }
        var chests = FindObjectsOfType<ChestTile>();
        //Debug.Log("chests: " + chests.Count());
        foreach(ChestTile c in chests) {
            c.OnChestSelection += ChangeSelectionState;
            c.OnChestAction += ChangeActionState;
        }
        Tile[] others = FindObjectsOfType<Tile>();
        //delete from others the tiles that have already been looped through
        others = others.Except(grounds)
                       .Except(doors)
                       .Except(traps)
                       .Except(chests)
                       .ToArray();
        //Debug.Log("others: " + others.Count());
        foreach(Tile t in others) {
            t.OnOtherSelection += ChangeSelectionState;
            t.OnOtherAction += ChangeActionState;
        }
    }

    private void ChangeSelectionState(SelectionStates newState, Vector2 newPosition) {
        Debug.Log("tile: " + newPosition.ToString() + " Change SelectionState from " + currentSelectionState + " to " + newState);
        if(currentSelectionState != SelectionStates.OTHER && newState == SelectionStates.OTHER) {
            gameManager.tileMap.RemoveAllHighlights();
        }
        currentSelectionState = newState;
        clickedPosition = newPosition;
        if(OnSelectionStateChange != null) {
            OnSelectionStateChange(currentSelectionState, clickedPosition);
        }
    }

    private void ChangeActionState(ActionStates newState, Vector2 newPosition) {
        Debug.Log("tile: " + newPosition.ToString() + " Change SelectionState from " + currentActionState + " to " + newState);
        currentActionState = newState;
        clickedPosition = newPosition;
        if(OnActionStateChange != null) {
            OnActionStateChange(currentActionState, clickedPosition);
        }
    }
}
