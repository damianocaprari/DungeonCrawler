using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

/* *
 * TileMap è la classe che rappresenta la mappa di gioco.
 * Contiene informazioni quali: 
 *  dimensioni della mappa
 *  lista delle Tile instanziate
 *  lista delle stanze (gruppi di caselle che formano una stessa stanza)
 *  spawn points delle unità
 * */

[System.Serializable]
[RequireComponent(typeof(TilePrefab))]
public class TileMap : MonoBehaviour {

    public Tile[,] tileMapOfTiles;
    public int[] tileMapOfTypes;
    public int[] tileMapOfRooms;
    public int mapSizeX, mapSizeY;
    private GameObject[] tilePrefabs;
    private TilePrefab prefabs;
    public List<List<Tile>> listOfRooms = new List<List<Tile>>();
    int roomCount = 0;
    public Vector2 clericSpawnPoint;
    public Vector2 mageSpawnPoint;
    public Vector2 thiefSpawnPoint;
    public Vector2 warriorSpawnPoint;
    public List<Vector2> enemiesSpawnPoints;

    //Json related variables
    string jsonFilePath;
    string tileMapJson;
    /* [System.Serializable]
       public struct tileMapMetadataWrapper
       {
           public int mapSizeX;
           public int mapSizeY;
           public int[] tileMapOfTypes;
           public int[] tileMapOfRooms;
           //public Vector2 clericSpawnPoint;
           //public Vector2 mageSpawnPoint;
           //public Vector2 thiefSpawnPoint;
           //public Vector2 warriorSpawnPoint;
           //public List<Vector2> enemiesSpawnPoints;
           public tileMapMetadataWrapper(int mapX, int mapY, int[] types, int[] rooms)
           {
               mapSizeX = mapX;
               mapSizeY = mapY;
               tileMapOfTypes = types;
               tileMapOfRooms = rooms;
               //clericSpawnPoint;
               //mageSpawnPoint;
               //thiefSpawnPoint;
               //warriorSpawnPoint;
               //enemiesSpawnPoints;
           }
       }*/

    void Start() {
        //	CreateMap();
        //	CreateRooms();		
        prefabs = GetComponent<TilePrefab>();
        jsonFilePath = Application.streamingAssetsPath + "/mapdata_adventure1.json";
        LoadTilePrefabs();
    }

    /* *
     * CreateMap instanzia tutte le Tiles secondo le informazioni prese da LoadMapTilesJson
     * Nel caso LoadMap non sia mai stato chiamato (non dovrebbe succedere in condizioni normali) crea una mappa di caselle base (Ground)
     * Oltre a instanziare le Tiles, popola l'array che aggrega le Tiles
     * Setta i confini della telecamera
     * Infine chiama la funzione che crea le liste di Rooms
     * */
    public void CreateMap(int new_mapSizeX, int new_mapSizeY) {
        // Debug.Log("Creo la Mappa");
        mapSizeX = new_mapSizeX;
        mapSizeY = new_mapSizeY;
        CameraController camCtrl = FindObjectOfType<CameraController>();
        camCtrl.ResetCameraBorders(mapSizeX, mapSizeY);
        if(tileMapOfTiles == null)
            tileMapOfTiles = new Tile[mapSizeX, mapSizeY];
        bool typesIsNull = false;
        if(tileMapOfTypes.Length == 0) {
            tileMapOfTypes = new int[mapSizeX * mapSizeY];
            typesIsNull = true;
        }
        bool roomsIsNull = false;
        if(tileMapOfRooms.Length == 0) {
            tileMapOfRooms = new int[mapSizeX * mapSizeY];
            roomsIsNull = true;
        }
        for(int y = 0; y < mapSizeY; y++) {
            for(int x = 0; x < mapSizeX; x++) {
                if(typesIsNull)
                    tileMapOfTypes[x + y * mapSizeX] = 0;
                if(roomsIsNull)
                    tileMapOfRooms[x + y * mapSizeX] = 0;
                if(tilePrefabs.Length != 0) {
                    Tile newTile = ((GameObject)Instantiate(tilePrefabs[tileMapOfTypes[x + y * mapSizeX]],
                                                            new Vector3(x - Mathf.Floor(mapSizeX / 2), -y + Mathf.Floor(mapSizeY / 2), 0),
                                                            Quaternion.identity, transform)).GetComponent<Tile>();
                    newTile.nodePosition = new Vector2(x, y);
                    newTile.roomNumber = tileMapOfRooms[x + y * mapSizeX];
                    if(newTile.roomNumber > roomCount - 1) //roomNumbers range from 0 to n; roomCount ranges from 1 to n+1
                        roomCount = newTile.roomNumber;
                    tileMapOfTiles[x, y] = newTile;
                }
            }
        }
        typesIsNull = false;
        CreateRooms();
    }

    /* *
     * Funzione di utility.
     * Popola la lista di stanze " listOfRooms "
     * */
    private void CreateRooms() {
        //	Debug.Log("INIZIO: CreateRooms()");
        //	Debug.Log("roomCount: " + roomCount);
        for(int i = 0; i < roomCount + 1; i++) {
            List<Tile> room = new List<Tile>();
            listOfRooms.Add(room);
            foreach(Tile tile in tileMapOfTiles) {
                if(tile.roomNumber == i) {
                    listOfRooms[i].Add(tile);
                }
            }
        }
    }

    /* *
     * Funzione di utility.
     * Data la posizione "virtuale" di una Tile
     * Ritorna la posizione della Tile nel mondo "reale" di Unity
     * */
    public Vector3 TileCoordToWorldCoord(Vector2 nodePosition) {
        float Xpos = nodePosition.x - mapSizeX / 2;
        float Ypos = mapSizeY / 2 - nodePosition.y;
        return new Vector3(Xpos, Ypos, 0);
    }

    /* *
     * Funzione di utility.
     * Ritorna un Tile object, prendendo in input la sua posizione come Vector2
     * */
    public Tile GetTileFromCoord(Vector2 nodePosition) {
        return tileMapOfTiles[(int)nodePosition.x, (int)nodePosition.y];
    }

    /* *
     * FUnzione di utility.
     * Se ci sono caselle evidenziate (ad esempio dopo la selezione di un personaggio) spegne ogni evidenziatura
     * */
    public void RemoveAllHighlights() {
        foreach(Tile t in tileMapOfTiles) {
            t.isHighlightedWalk = false;
            t.isHighlightedAttack = false;
        }
    }

    /* *
     * Funzione di utility.
     * Salva le informazioni contenute nella classe in un file Json con un nome dato in input
     * */
    public void SaveTileMapJson(string jsonName) {
        jsonFilePath = Application.streamingAssetsPath + "/" + jsonName;
        // tileMapMetadataWrapper wrapper = new tileMapMetadataWrapper(mapSizeX, mapSizeY, tileMapOfTypes, new int[0]);
        tileMapJson = JsonUtility.ToJson(this, true);
        File.WriteAllText(jsonFilePath, tileMapJson);
        Debug.Log("Scritto JSON: " + jsonName);
    }

    /* *
     * Funzione di utility.
     * Carica nella classe le informazioni contenute in un file Json con un nome dato in input
     * */
    public void LoadTileMapJson(string jsonName) {
        //     tileMapOfTypes = wrapper.tileMapOfTypes;
        jsonFilePath = Application.streamingAssetsPath + "/" + jsonName;
        if(tileMapOfTiles != null) {
            //Debug.Log("PROVO A DISTRUGGERE LA MAPPA");
            for(int x = 0; x < mapSizeX; x++) {
                for(int y = 0; y < mapSizeY; y++) {
                    //Debug.Log("distruggo Tile[" +x+", "+y+"]\n");
                    Destroy(tileMapOfTiles[x, y].gameObject);
                }
            }
            tileMapOfTiles = null;
        }
        tileMapJson = File.ReadAllText(jsonFilePath);
        //Debug.Log(mapSizeX + " | " + mapSizeY + " | " + tileMapOfRooms.Length);
        JsonUtility.FromJsonOverwrite(tileMapJson, this);
        //Debug.Log(mapSizeX + " | " + mapSizeY + " | " + tileMapOfRooms.Length);
        LoadTilePrefabs();
        CreateMap(mapSizeX, mapSizeY);
    }

    /* *
     * funzione di utility
     * Carica la lista di prefabs delle Tile dall'altro script fratello attaccato all'oggetto Map
     * */
    void LoadTilePrefabs() {
        this.tilePrefabs = prefabs.tilePrefabs;
    }

}
