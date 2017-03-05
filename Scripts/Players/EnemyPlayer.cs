using UnityEngine;
using System.Collections;

public class EnemyPlayer : Player {

	// Use this for initialization
	override public void Start() {
		faction = PlayerFaction.AIEnemy;
		base.Start();
	}

	// Update is called once per frame
	override public void Update() {
		base.Update();
	}
}
