using UnityEngine;
using System.Collections;

public class WarriorPlayer : UserPlayer {

	// Use this for initialization
	override public void Start() {
		
		InitStats();
		base.Start();
	}

	// Update is called once per frame
	override public void Update() {
		base.Update();
	}

	void InitStats() {
		name = "Redgar";
		unitName = name;
		InitHP();
		currentHP = maxHP;
		weaponSlots = 2;
		arctifactSlots = 1;
		maxMovementPoints = 4;
		currentMovementPoints = maxMovementPoints;
		armorClass = 2;
		bagSlots = 4;
		hasSpecialActions = true;
	}

	override protected void InitHP() {
		switch(level) {
			case 1:
				maxHP = 8;
				break;
			case 2:
				maxHP = 12;
				break;
			case 3:
				maxHP = 15;
				break;
			default: //should not be used, in any case it will be considered LEVEL 1
				maxHP = 8;
				break;
		}
	}
	
}
