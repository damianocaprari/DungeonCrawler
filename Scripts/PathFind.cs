using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/* Classe che contiene le funzioni di PathFinding.
 * Si possono migliorare le funzioni slegandole dal concetto di Player, prendendo in input una posizione iniziale, una finale, e la mappa.
 */

public class PathFind : MonoBehaviour
{
    //GameManager gameManager;
    TileMap tileMap;
    //Player selectedUnit;
    List<Tile> currentPath;

    void Start()
    {
        tileMap = FindObjectOfType<TileMap>();
        //	gameManager = FindObjectOfType<GameManager>();
    }

    /* 
     * Trova il percorso dal punto A al punto B (se esiste)
     * e salva il percorso trovato nella variabile " currentPath " del player selezionato
     */
    public void GeneratePathTo(Player selectedUnit, Vector2 destination)
    {
        //	tileMap = FindObjectOfType<TileMap>();
        //	gameManager = FindObjectOfType<GameManager>();
        //	selectedUnit = gameManager.GetSelectedPlayer();
        selectedUnit.currentPath = null;

        //Debug.Log("GeneratePathTo chiamato da: " + selectedUnit.unitName);

        Dictionary<Tile, float> dist = new Dictionary<Tile, float>();
        Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();
        //setup the Q: the list of nodes we have not check yet
        List<Tile> unvisited = new List<Tile>();

        Tile source = tileMap.tileMapOfTiles[(int)selectedUnit.position.x, (int)selectedUnit.position.y];
        Tile target = tileMap.tileMapOfTiles[(int)destination.x, (int)destination.y];

        dist[source] = 0;
        prev[source] = null;

        //initialize everything to have infinity distance, since we dont know any better now
        //also is possible that we cannot reach some node from the source
        foreach (Tile v in tileMap.tileMapOfTiles)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }
        while (unvisited.Count > 0)
        {
            //u is going to be the unvisited node with the smallest distance
            Tile u = null;
            foreach (Tile possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }
            if (u == target)
            {
                break;
            }
            unvisited.Remove(u);

            foreach (Tile v in u.neighbours)
            {
                float alt = dist[u] + u.DistanceTo(v);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }
        //if we get here, we either found the shortest route to our target
        //or there is NO ROUTE at all;
        if (prev[target] == null)
        {
            //no route between our target and the source
            return;
        }
        //step through the "prev" chain and add it to our path
        currentPath = new List<Tile>();
        Tile curr = target;
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }
        //right now currentPath describes a route from our target to our source
        //we need to invert it
        currentPath.Reverse();
        selectedUnit.GetComponent<Player>().currentPath = currentPath;
    }

    /*
     * Illumina le caselle raggiungibili da un determinato giocatore (dopo che è stato scelto)
     */
    public void HighLightReachableTiles(Player selectedUnit)
    {
        int maxDistanceToHighlight = selectedUnit.currentMovementPoints;
        Dictionary<Tile, float> dist = new Dictionary<Tile, float>();
        Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();
        //setup the Q: the list of nodes we have not check yet
        List<Tile> unvisited = new List<Tile>();
        Tile source = tileMap.tileMapOfTiles[(int)selectedUnit.position.x, (int)selectedUnit.position.y];
        dist[source] = 0;
        prev[source] = null;

        //initialize everything to have infinity distance, since we dont know any better now
        //also it'ss possible that we cannot reach some nodes from the source, so infinity is a reasonable value
        foreach (Tile v in tileMap.tileMapOfTiles)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            if (Mathf.Abs(v.nodePosition.x - source.nodePosition.x) <= maxDistanceToHighlight + 1 &&
            Mathf.Abs(v.nodePosition.y - source.nodePosition.y) <= maxDistanceToHighlight + 1)
            {
                unvisited.Add(v);
            }
        }
        //foreach(Tile v in unvisited)
        //{
        //    Debug.Log(v.nodePosition.ToString());
        //}
        while (unvisited.Count > 0)
        {   //continue untill you visit ALL the nodes
            //u is going to be the unvisited node with the smallest distance
            Tile u = unvisited.OrderBy(n => dist[n]).First();
            unvisited.Remove(u);

            foreach (Tile v in u.neighbours)
            {
                float alt = dist[u] + u.DistanceTo(v);
                if (alt < dist[v])
                { //if the new distance to the v-node is shorter
                    dist[v] = alt;  //set the distance to v-node = alt
                    prev[v] = u;    //set u-node as the previous node to reach the v-node
                }
                if (dist[v] <= maxDistanceToHighlight && !v.isHighlighted)
                {
                    v.isHighlighted = true;
                }
            }
        }
    }
}