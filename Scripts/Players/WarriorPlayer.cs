using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorPlayer : UserPlayer {

    // Use this for initialization
    new void Start() {
        InitStats();
        base.Start();
    }

    // Update is called once per frame
    new void Update() {
        base.Update();
    }

    void InitStats() {
        if(unitName == null || unitName == "") {
            unitName = "Redgar";
        }
        name = unitName;
        if(unitClass == null || unitClass == "") {
            unitClass = "Warrior";
        }
        if(unitRace == null || unitRace == "") {
            unitRace = "Human";
        }
        InitHP();
        currentHP = maxHP;
        weaponSlots = 2;
        arctifactSlots = 1;
        maxMovementPoints = 4;
        attackRange = 1;
        currentMovementPoints = maxMovementPoints;
        armorClass = 2;
        bagSlots = 4;
        hasSpecialActions = true;
        maxActionPoints = 2;
        currentActionPoints = maxActionPoints;
        if(attackDices == null || attackDices.Count == 0) {
            attackDices = new List<AttackDieTypes>();
            attackDices.Add(AttackDieTypes.orange_111122);
        }
    }

    public override void DealDamage(Player target) {
        int damage = 0;
        foreach(AttackDieTypes a in attackDices) {
            damage += AttackDie.RollDice(a);
        }
        target.GetDamaged(damage + 1);
        currentActionPoints--;
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
