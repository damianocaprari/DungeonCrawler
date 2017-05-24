using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackDieTypes {
    yellow_001111,
    orange_111122,
    red_012223,
    violet_222233
}

public static class AttackDie {
	public static int RollDice(AttackDieTypes attackDiceType) {
        int roll = Random.Range(0, 6);
        int result = 0;
        switch(attackDiceType) {
            case AttackDieTypes.yellow_001111:
                result = (roll < 2) ? 0 : 1;
                break;
            case AttackDieTypes.orange_111122:
                result = (roll < 4) ? 1 : 2;
                break;
            case AttackDieTypes.violet_222233:
                result = (roll < 4) ? 2 : 3;
                break;
            case AttackDieTypes.red_012223:
                if(roll == 0) { result = 0; break; }
                if(roll == 1) { result = 1; break; }
                if(roll == 5) { result = 3; break; }
                result = 2; break;
        }
        Debug.Log("Rolled face number: " + roll + " on the Die: " + attackDiceType + ". Result: " + result);
        return result;
    }
}
