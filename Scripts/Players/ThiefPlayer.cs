using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ThiefPlayer : UserPlayer {

	public int defusedTraps;
    public List<bool> canSearchInRoom = new List<bool>();
	public List<Tile> trapsFound = new List<Tile>();
	TileMap tileMap;


	// Use this for initialization
	override public void Start() {
		base.Start();
		tileMap = FindObjectOfType<TileMap>();        
		InitStats();
	}

	// Update is called once per frame
	override public void Update() {

		//when lidda defuses 3 traps she heals for 2HPs
		if(defusedTraps == 3)
			HealFromDefusingTraps();

		base.Update();
	}

	void InitStats() {
		name = "Lidda";
		unitName = name;
		InitHP();
		currentHP = maxHP;
		weaponSlots = 1;
		arctifactSlots = 1;
		maxMovementPoints = 6;
		currentMovementPoints = maxMovementPoints;
		armorClass = 2;
		bagSlots = 4;
		hasSpecialActions = true;
		defusedTraps = 0;
        for (int i = 0; i < tileMap.listOfRooms.Count; i++) {
            canSearchInRoom.Add(true);
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

	void HealFromDefusingTraps() {
		if(defusedTraps == 3) {
			defusedTraps = 0;
			if(currentHP < maxHP - 2)
				currentHP += 2;
			else
				currentHP = maxHP;
		}
	}

	public void SearchForTrapsInTheRoom() {
        int currentRoom = tileMap.tileMapOfTiles[(int)position.x, (int)position.y].roomNumber;
        if (!canSearchInRoom[currentRoom]) {
			return;
		}
		//Debug.Log("currentRoom: " + currentRoom);
        string strRoom = "currentRoom: " + currentRoom + "\t|\t";
        List <Tile> trapsToReveal = new List<Tile>();
		foreach(Tile tile in tileMap.listOfRooms[currentRoom]) {
			if(tile.isTrap == true && !tile.isTrapRevealed) {
				trapsToReveal.Add(tile);
			}
		}
		//Debug.Log("Trappole nella stanza: " + trapsToReveal.Count);
        string strTraps = "Trappole nella stanza: " + trapsToReveal.Count + "\n";
        if (trapsToReveal.Count == 0) {
			Debug.Log("Nessuna trappola da trovare!");
            return;
		}
		int diceRoll = Random.Range(0, 6);
		//Debug.Log("ROLL del dado: " + diceRoll);
        string roll = "ROLL del dado: " + diceRoll + "\n";
        string result = "";
        switch (diceRoll) {
			case 0: //0 = double success!! reveal 2 traps if there are						
                if (trapsToReveal.Count >= 2)
                {
                    trapsToReveal[0].isTrapRevealed = true;
                    trapsToReveal[1].isTrapRevealed = true;
                    trapsFound.Add(trapsToReveal[0]);
                    trapsFound.Add(trapsToReveal[1]);
                    trapsToReveal.RemoveAt(1);
                    trapsToReveal.RemoveAt(0);
                    //Debug.Log("Trovate due trappole! (case: 0)");
                    result = "Trovate due trappole! (Doppio Successo)\n";
                }
                else if (trapsToReveal.Count == 1)
                {
                    trapsToReveal[0].isTrapRevealed = true;
                    trapsFound.Add(trapsToReveal[0]);
                    trapsToReveal.RemoveAt(0);
                    Debug.Log("Trovata una trappola! (case: 0)");
                    result = "Trovata una trappola! (Doppio Successo)\n";
                }
                else
                {
                    //Debug.Log("Nessuna trappola trovata! (case: 0)");
                    result = "Nessuna trappola trovata! (Doppio Successo)\n";
                }
				break;
			case 1: // 1 = single success! reveal 1 trap if there is
    //            if (trapsToReveal.Count >= 1)
    //            {
    //                trapsToReveal[0].isTrapRevealed = true;
    //                trapsFound.Add(trapsToReveal[0]);
    //                trapsToReveal.RemoveAt(0);
    //                Debug.Log("Trovata una trappola! (case: 1)");
    //                result = "Trovata una trappola! (Singolo Successo)\n";
    //            }
    //            else
    //            {
    //                Debug.Log("Nessuna trappola trovata! (case: 1)");
    //                result = "Nessuna trappola trovata! (Singolo Successo)\n";
    //            }
				//break;
			case 2: // 2 = single success! reveal 1 trap if there is
				if(trapsToReveal.Count >= 1) {
					trapsToReveal[0].isTrapRevealed = true;
					trapsFound.Add(trapsToReveal[0]);
					trapsToReveal.RemoveAt(0);
					//Debug.Log("Trovata una trappola! (case: 2)");
                    result = "Trovata una trappola! (Singolo Successo)\n";
                }
				else
                {
                    //Debug.Log("Nessuna trappola trovata! (case: 2)");
                    result = "Nessuna trappola trovata! (Singolo Successo)\n";
                }
				break;
			case 3: // 3 = critic failure!!! cannot search traps anymore
				canSearchInRoom[currentRoom] = false;
				//Debug.Log("Nessuna trappola trovata! (case: 3)");
                result = "Fallimento Critico\n";
				break;
			case 4: // 4 = failure! was not able to find a trap
                    //Debug.Log("Nessuna trappola trovata! (case: 4)");
                    //result = "Nessuna trappola trovata! (fallimento)";
                //break;
            case 5: // 5 = failure! was not able to find a trap
				//Debug.Log("Nessuna trappola trovata! (case: 5)");
                result = "Nessuna trappola trovata! (fallimento)";
                break;
		}
        Debug.Log(strRoom + strTraps + result);
		return;
	}

	public void DefuseTrap(Tile targetTile) {
		if(targetTile.isTrapRevealed) {
			int diceRoll = Random.Range(0, 5);
			if(diceRoll == 0) { //0 = critical failure! the trap activates!
                Debug.Log("FALLIMENTO! la trappola si attiva.");
				targetTile.ActivateTrap();
			}
			else {  //1-5 = Success! defuse the trap and try to heal
                Debug.Log("SUCCESSO! la trappola si disattiva.");
                targetTile.DefuseTrap();
				defusedTraps++;
				HealFromDefusingTraps();
			}
		}
	}
}