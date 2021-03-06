﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

    //da eliminare in futuro
    //variabili usate per creare più velocemente le mappe
    public int NUMERO_STANZA = 1;
    public int NUMERO_TYLETYPE = 0;
    public string mapJson;
    public bool colorTileRooms = false;

    TileMap tileMap;
	public Vector2 mapSize = new Vector2(10, 10);

	public List<Player> listOfPlayers = new List<Player>();
	public int currentPlayer = 0;

	public GameObject clericPrefab;
	public GameObject magePrefab;
	public GameObject warriorPefab;
	public GameObject thiefPrefab;
	public GameObject enemyPrefab;

	public Vector2 tilePressed = new Vector2(-1, -1); // (-1, -1) is to be considered a null value for tilePressed.
	public PathFind pathFind;

	// Use this for initialization
	void Start() {
		tileMap = FindObjectOfType<TileMap>();		
		tilePressed = new Vector2(-1, -1);
		pathFind = FindObjectOfType<PathFind>();
        
        //tileMap.CreateMap((int)mapSize.x, (int)mapSize.y);
        LoadMap(mapJson);
    }

	// Update is called once per frame
	void Update() {
		if(tilePressed != new Vector2(-1, -1)) {    //  (-1, -1) is to be considered a null value for tilePressed.
            ManageTilePressed();

            ///* CODICE PER MODIFICARE LE STANZE DELLA MAPPA IN MODO PIU COMODO */
            //tileMap.tileMapOfRooms[(int)(tilePressed.x + tilePressed.y * tileMap.mapSizeX)] = NUMERO_STANZA;
            ///* CODICE PER MODIFICARE LE CASELLE MAPPA IN MODO PIU COMODO */
            //tileMap.tileMapOfTypes[(int)(tilePressed.x + tilePressed.y * tileMap.mapSizeX)] = NUMERO_TYLETYPE;
            //tilePressed = new Vector2(-1, -1);
            //tileMap.SaveTileMapJson(mapJson);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            int dmg = Random.Range(0, 4);
            Debug.Log("Deal {" + dmg + "-"+ GetSelectedPlayer().armorClass+"} dmg to: " + GetSelectedPlayer().name);
            GetSelectedPlayer().Hit(dmg);
        }
    }

    void ManageTilePressed()
    {
        Debug.Log("TilePressed: " + tilePressed.ToString());
        if (pathFind == null)
            Debug.Log("Pathfind: NULL");
        if (!IsTileOccupiedByPlayer(tilePressed))
        {
            if (GetSelectedPlayer().isPlayerControlled)
            {
                pathFind.GeneratePathTo(GetSelectedPlayer(), tilePressed);
                tileMap.RemoveAllHighlights();
                //GetSelectedPlayer().DrawPathLine();
            }
        }
        else
        {
            SelectPlayerWithMouse(tilePressed);
        }
        tilePressed = new Vector2(-1, -1);
    }

	void GeneratePlayers() {
		Player cleric = ((GameObject)Instantiate(clericPrefab,
												new Vector3(tileMap.clericSpawnPoint.x - Mathf.Floor(tileMap.mapSizeX / 2), -tileMap.clericSpawnPoint.y + Mathf.Floor(tileMap.mapSizeY / 2), 0),
														Quaternion.identity)).GetComponent<ClericPlayer>();
		cleric.position = tileMap.clericSpawnPoint;
		listOfPlayers.Add(cleric);

		Player mage = ((GameObject)Instantiate(magePrefab,
												new Vector3(tileMap.mageSpawnPoint.x - Mathf.Floor(tileMap.mapSizeX / 2), -tileMap.mageSpawnPoint.y + Mathf.Floor(tileMap.mapSizeY / 2), 0),
														Quaternion.identity)).GetComponent<MagePlayer>();
		mage.position = tileMap.mageSpawnPoint;
		listOfPlayers.Add(mage);

		Player thief = ((GameObject)Instantiate(thiefPrefab,
												new Vector3(tileMap.thiefSpawnPoint.x - Mathf.Floor(tileMap.mapSizeX / 2), -tileMap.thiefSpawnPoint.y + Mathf.Floor(tileMap.mapSizeY / 2), 0),
														Quaternion.identity)).GetComponent<ThiefPlayer>();
		thief.position = tileMap.thiefSpawnPoint;
		listOfPlayers.Add(thief);

		Player warrior = ((GameObject)Instantiate(warriorPefab,
												new Vector3(tileMap.warriorSpawnPoint.x - Mathf.Floor(tileMap.mapSizeX / 2), -tileMap.warriorSpawnPoint.y + Mathf.Floor(tileMap.mapSizeY / 2), 0),
														Quaternion.identity)).GetComponent<WarriorPlayer>();
		warrior.position = tileMap.warriorSpawnPoint;
		listOfPlayers.Add(warrior);

		Player zombie = ((GameObject)Instantiate(enemyPrefab,
												new Vector3(7 - Mathf.Floor(tileMap.mapSizeX / 2), -3 + Mathf.Floor(tileMap.mapSizeY / 2), 0),
														Quaternion.identity)).GetComponent<EnemyPlayer>();
		zombie.position = new Vector2(7, 3);
		listOfPlayers.Add(zombie);
	}

	void OnGUI() {
		Rect rect = new Rect(10, Screen.height - 100, 100, 55);
		if(GUI.Button(rect, "SearchTraps")) {
			Debug.Log("##########################");
			ThiefPlayer thief = FindObjectOfType<ThiefPlayer>();
			thief.SearchForTrapsInTheRoom();
		}
		rect = new Rect(10, Screen.height - 100 - 60, 100, 55);
		if(GUI.Button(rect, "Next Player")) {
			if(currentPlayer == listOfPlayers.Count - 1)
				currentPlayer = 0;
			else
				currentPlayer++;
			tilePressed = new Vector2(-1, -1);
			Debug.Log("Selezionato: " + GetSelectedPlayer().unitName);
		}
        rect = new Rect(10, Screen.height - 200 - 60, 100, 55);
        if (GUI.Button(rect, "SALVA MAPPA"))
        {
            tileMap.SaveTileMapJson(mapJson);
        }
        rect = new Rect(10, Screen.height - 300 - 60, 100, 55);
        if (GUI.Button(rect, "CARICA MAPPA"))
        {
            tileMap.LoadTileMapJson(mapJson);
            //GeneratePlayers();
        }
    }

	public Player GetSelectedPlayer() {
		return listOfPlayers[currentPlayer];
	}

	public void SetSelectedPlayer(int playerIndex) {
		currentPlayer = playerIndex;
		Debug.Log("Selezionato: " + GetSelectedPlayer().unitName);
		tileMap.RemoveAllHighlights();
		pathFind.HighLightReachableTiles(GetSelectedPlayer());
	}

	public void SelectPlayerWithMouse(Vector2 tilePressed) {
		if(tilePressed != new Vector2(-1, -1)) {
			int playerIndex = 0;
			foreach(Player p in listOfPlayers) {
				if(tilePressed == p.position) {
					SetSelectedPlayer(playerIndex);
					return;
				}
				playerIndex++;
			}
		}
		return;
	}

	public bool IsTileOccupiedByPlayer(Vector2 tileToCheck) {
		foreach(Player p in listOfPlayers) {
			if(tileToCheck == p.position) {
				return true;
			}
		}
		return false;
	}

    void LoadMap(string jsonName)
    {
        tileMap.LoadTileMapJson(jsonName);
        GeneratePlayers();
    }

}
