﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MagePlayer : UserPlayer {


	public int spellSlots;
	public int maxSpellPoints;
	public int currentSpellPoints;

	// Use this for initialization
	override public void Start() {
		InitStats();
		base.Start();
	}

	// Update is called once per frame
	override public void Update() {
		armorClass = ArmorClassFromSpellPoints();

		base.Update();
	}

	void InitStats() {
		name = "Mialee";
		unitName = name;
		InitHP();
		currentHP = maxHP;
		InitSpellPoints();
		currentSpellPoints = maxSpellPoints;
		spellSlots = 1;
		weaponSlots = 1;
		arctifactSlots = 1;
		maxMovementPoints = 5;
		currentMovementPoints = maxMovementPoints;
		armorClass = ArmorClassFromSpellPoints();
		bagSlots = 5;
		hasSpecialActions = true;
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

	int ArmorClassFromSpellPoints() {
		if(currentSpellPoints >= 3)
			return 2;
		if(currentSpellPoints >= 1)
			return 1;
		else
			return 0;
	}
}