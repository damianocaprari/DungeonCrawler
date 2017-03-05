using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
 * TileType è una classe ENUMERABILE
 * definisce quanti e quali tipi di caselle esistono
 * ogni casella avrà associata ad essa un tipo-di-casella che permette un confronto più semplice con le altre
 * 
 */


[System.Serializable]
public enum TileType  {
	Wall,
	Ground,
	Door,
	Trap,
    Column,
    Transparent

	//quando ci cammini sopra succede qualcosa
	//ChestTrigger,
	//DoorTrigger,
	//EventTrigger
}
