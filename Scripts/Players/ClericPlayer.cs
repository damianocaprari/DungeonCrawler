using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClericPlayer : UserPlayer {

	// Use this for initialization
	new void Start() {
		InitStats();
		base.Start();
	}

	// Update is called once per frame
	new void Update() {
		base.Update();
	}

	void InitStats()
    {
        if (unitName == null || unitName == "")
        {
            unitName = "Jozan";
        }
        name = unitName;
        if (unitClass == null || unitClass == "")
        {
            unitClass = "Cleric";
        }
        if (unitRace == null || unitRace == "")
        {
            unitRace = "Human";
        }
        InitHP();
		currentHP = maxHP;
		InitSpellPoints();
		currentSpellPoints = maxSpellPoints;
		spellSlots = 1;
		weaponSlots = 1;
		arctifactSlots = 1;
		maxMovementPoints = 5;
        attackRange = 2;
		currentMovementPoints = maxMovementPoints;
		armorClass = 2;
		bagSlots = 5;
		hasSpecialActions = true;
        maxActionPoints = 2;
        currentActionPoints = maxActionPoints;
        if(attackDices == null || attackDices.Count == 0) {
            attackDices = new List<AttackDieTypes>();
            attackDices.Add(AttackDieTypes.yellow_001111);
            attackDices.Add(AttackDieTypes.orange_111122);
        }
    }

	override protected void InitHP() {
		switch(level) {
			case 1:
				maxHP = 5;
				break;
			case 2:
				maxHP = 7;
				break;
			case 3:
				maxHP = 9;
				break;
			default: //should not be used, in any case it will be considered LEVEL 1
				maxHP = 5;
				break;
		}
	}

	override protected void InitSpellPoints() {
		switch(level) {
			case 1:
				maxSpellPoints = 5;
				break;
			case 2:
				maxSpellPoints = 7;
				break;
			case 3:
				maxSpellPoints = 9;
				break;
			default: //should not be used, in any case it will be considered LEVEL 1
				maxSpellPoints = 5;
				break;
		}
	}

    override protected void Heal(Vector2 targetPosition) {
        int distanceToTarget = (int)(Mathf.Abs(position.x - targetPosition.x) + Mathf.Abs(position.y - targetPosition.y));
        if(distanceToTarget > 1) { //if absolute distance is greater than 1, return
            Debug.Log("absolute distance (" + distanceToTarget + ") is greater than 1");
        }
        else {
            if(currentSpellPoints < 2) {
                Debug.Log("currentSpellPoints (" + currentSpellPoints + ") not enough to cast the Heal spell");
            }
            else { 
                Player targetPlayer = gameManager.GetPlayerByTile(targetPosition);
                if(targetPlayer.currentHP != targetPlayer.maxHP) {
                    targetPlayer.GetHealed(4);
                    currentActionPoints--;
                    currentSpellPoints -= 2;
                }
                else {
                    Debug.Log("targetPlayer does not need to be healed");
                }
            }
        }
    }

}
