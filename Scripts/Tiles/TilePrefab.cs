using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TilePrefab è una classe di utility.
 * Serve solo ad accorpare i prefabs delle caselle
 * Lo script viene accorpato all'oggetto Map sempre insieme allo script TileMap che utilizza questa classe
 * La necessità di dover usare una classe esterna per accorparle, invece che una lista direttamente in TileMap nasce dal
 * fatto che si creavano problemi e inconsistenze con la funzione LoadTileMapJson.
 */

[RequireComponent(typeof(TileMap))]
public class TilePrefab : MonoBehaviour
{
    public GameObject[] tilePrefabs;
}
