using UnityEngine;
using System.Collections;

public class EnemyPlayer : Player {

    public delegate void SelectionEvent(SelectionStates newState, Vector2 newPosition);
    public event SelectionEvent OnEnemySelection;
    public delegate void ActionEvent(ActionStates newState, Vector2 newPosition);
    public event ActionEvent OnEnemyAction;

    // Use this for initialization
    new protected void Start() {
        FindObjectOfType<MouseEventsFSM>().OnSelectionStateChange += SelectedEnemy;
        faction = PlayerFaction.AIEnemy;
        base.Start();
    }

    // Update is called once per frame
    new protected void Update() {
        base.Update();
    }

    override public void FirePlayerSelectionEvent() {
        if(OnEnemySelection != null) {
            OnEnemySelection(SelectionStates.ENEMY_SELECTED, position);
            Debug.Log("Cliccato 0 in Enemy" + " e lancio un Selection tipo: " + SelectionStates.ENEMY_SELECTED);
        }
    }

    override public void FirePlayerActionEvent() {
        if(OnEnemyAction != null) {
            OnEnemyAction(ActionStates.ATTACK, position);
            Debug.Log("Cliccato 1 in Enemy" + " e lancio un Action tipo: " + ActionStates.ATTACK);
        }
    }

    private void SelectedEnemy(SelectionStates state, Vector2 position) {
        if(state.Equals(SelectionStates.ENEMY_SELECTED) && position.Equals(this.position)) {
            Debug.Log("Selezionato il ENEMY");
        }
        else {

        }
    }

}
