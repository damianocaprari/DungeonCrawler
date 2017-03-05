using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UserPlayer : Player {

	public int weaponSlots;
	public int arctifactSlots;

	// Use this for initialization
	override public void Start () {
		faction = PlayerFaction.PlayerControlled;
		base.Start();
	}
	
	// Update is called once per frame
	override public void Update () {
		base.Update();
	}

	virtual protected void InitHP() { }
	virtual protected void InitSpellPoints() { }
}
