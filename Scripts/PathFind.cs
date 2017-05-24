using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/* *
 * Classe che contiene le funzioni di PathFinding.
 * Si possono migliorare le funzioni slegandole dal concetto di Player, prendendo in input una posizione iniziale, una finale, e la mappa.
 * */

public class PathFind : MonoBehaviour {
    TileMap tileMap;

    void Start() {
        tileMap = FindObjectOfType<TileMap>();
    }

    /* *
     * Trova il percorso dal punto A al punto B (se esiste)
     * e salva il percorso trovato nella variabile " currentPath " del player selezionato
     * */
    public List<Tile> GeneratePathTo(Vector2 start, Vector2 destination) {
        List<Tile> path = new List<Tile>();
        Dictionary<Tile, float> dist = new Dictionary<Tile, float>();
        Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();
        //setup the Q: the list of nodes we have not check yet
        List<Tile> unvisited = new List<Tile>();

        Tile source = tileMap.tileMapOfTiles[(int)start.x, (int)start.y];
        Tile target = tileMap.tileMapOfTiles[(int)destination.x, (int)destination.y];

        dist[source] = 0;
        prev[source] = null;

        //initialize everything to have infinity distance, since we dont know any better now
        //also is possible that we cannot reach some node from the source
        foreach(Tile v in tileMap.tileMapOfTiles) {
            if(v != source) {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }
        while(unvisited.Count > 0) {
            //u is going to be the unvisited node with the smallest distance
            Tile u = null;
            foreach(Tile possibleU in unvisited) {
                if(u == null || dist[possibleU] < dist[u]) {
                    u = possibleU;
                }
            }
            if(u == target) {
                break;
            }
            unvisited.Remove(u);

            foreach(Tile v in u.neighbours) {
                float alt = dist[u] + u.DistanceTo(v);
                if(alt < dist[v]) {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }
        //if we get here, we either found the shortest route to our target
        //or there is NO ROUTE at all;
        if(prev[target] == null) {
            //no route between our target and the source
            return new List<Tile>();
        }
        //step through the "prev" chain and add it to our path
        Tile curr = target;
        while(curr != null) {
            path.Add(curr);
            curr = prev[curr];
        }
        //right now path describes a route from our target to our source
        //we need to invert it
        path.Reverse();
        return path;
    }

    /* *
     * Illumina le caselle raggiungibili camminando da un determinato giocatore (dopo che è stato scelto)
     * */
    public void HighLightReachableTiles(Vector2 start, int walkDistance, int attackDistance) {
        Dictionary<Tile, float> dist = new Dictionary<Tile, float>();
        Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();
        //setup the Q: the list of nodes we have not check yet
        List<Tile> unvisited = new List<Tile>();
        Tile source = tileMap.tileMapOfTiles[(int)start.x, (int)start.y];
        int maxReachDistance = walkDistance + attackDistance;
        dist[source] = 0;
        prev[source] = null;

        //initialize everything to have infinity distance, since we dont know any better now
        //also it'ss possible that we cannot reach some nodes from the source, so infinity is a reasonable value
        foreach(Tile v in tileMap.tileMapOfTiles) {
            if(v != source) {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            if(Mathf.Abs(v.nodePosition.x - source.nodePosition.x) <= maxReachDistance &&
            Mathf.Abs(v.nodePosition.y - source.nodePosition.y) <= maxReachDistance) {
                unvisited.Add(v);
            }
        }
        List<Tile> tilesHighlightedWalk = new List<Tile>();
        List<Tile> tilesHighlightedAttack = new List<Tile>();
        while(unvisited.Count > 0) {   //continue untill you visit ALL the nodes
            //u is going to be the unvisited node with the smallest distance
            Tile u = unvisited.OrderBy(n => dist[n]).First();
            unvisited.Remove(u);

            foreach(Tile v in u.neighbours) {
                float alt = dist[u] + u.DistanceTo(v);
                if(alt < dist[v]) { //if the new distance to the v-node is shorter
                    dist[v] = alt;  //set the distance to v-node = alt
                    prev[v] = u;    //set u-node as the previous node to reach the v-node
                }
                if(dist[v] <= walkDistance && !v.isHighlightedWalk) {
                    v.isHighlightedWalk = true;
                    tilesHighlightedWalk.Add(v);
                }
                if(dist[v] > walkDistance && dist[v] <= maxReachDistance && !v.isHighlightedAttack) {
                    v.isHighlightedAttack = true;
                    tilesHighlightedAttack.Add(v);
                }
            }
        }
        foreach(Tile t in tilesHighlightedWalk) {
            t.ManageHighLightShape("HighLightMarkerWalk");
        }
        foreach(Tile t in tilesHighlightedAttack) {
            t.ManageHighLightShape("HighLightMarkerAttack");
        }
    }

}
