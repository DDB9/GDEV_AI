using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class responsible for the pathfinding.

public class Astar : MonoBehaviour {

    PathRequestManager requestManager;      // Reference to the path request manager.
    TileGrid grid;                          // Reference to the grid

    void Awake() {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<TileGrid>();    // Assigning the grid.
    }

    public void StartFindPath(Vector3 startPosition, Vector3 targetPosition) {
        CalculatePath(startPosition, targetPosition);
    }

    void CalculatePath(Vector3 cp_start, Vector3 cp_target) {
        Debug.Log("Calculating Path");

        grid.Reset();

        Tile startTile = grid.TileFromWorldPosition(cp_start);   // translate the start position to a grid tile.
        Tile targetTile = grid.TileFromWorldPosition(cp_target); // translate the target position to a grid tile.

        List<Tile> openList = new List<Tile>();           // List to check for neighbours.
        HashSet<Tile> closedList = new HashSet<Tile>();   // HashSet for checked neighbours. 
                                                          // (HashSet for better performance).
        Vector3[] waypoints = null;
        bool pathSucces = false;

        if (!startTile.isWall && !targetTile.isWall) {  // Only if the start and end tile are walkable initiate the pathfinding.
            openList.Add(startTile);                    // Start with the first tile (Start Position).
            while (openList.Count > 0) {
                Tile currentTile = openList[0];
                for (int i = 0; i < openList.Count; i++) {
                    // Check if the current node's f-cost is lower or the same as 
                    if (openList[i].f < currentTile.f || (openList[i].f == currentTile.f && openList[i].h < currentTile.h)) {
                        currentTile = openList[i];
                    }
                }

                openList.Remove(currentTile);   // If the tile has been checked, remove it from the openList.
                closedList.Add(currentTile);    // And add it to the closedList, so it won't be checked again.

                if (currentTile == targetTile) {            // If the current tile is the target tile
                    pathSucces = true;
                    break;
                }

                foreach (Tile neighbour in grid.GetNeighbourTiles(currentTile)) {
                    if (neighbour.isWall || closedList.Contains(neighbour)) {
                        continue;   // Ignore the neighbour if it's a wall.
                    }

                    int moveCost = currentTile.g + GetManhattenDistance(currentTile, neighbour);
                    if (!openList.Contains(neighbour) || moveCost < neighbour.g) {
                        neighbour.g = moveCost;                                     // Assigns the move cost (g);
                        neighbour.h = GetManhattenDistance(neighbour, targetTile);  // the manhatten distance to target (h);
                        neighbour.parent = currentTile;                             // and sets the current tile to the last tile's parent. 

                        if (!openList.Contains(neighbour)) {
                            openList.Add(neighbour);    // Adds the neighbours to the openList.
                        }
                    }
                }
            }
        }  
        //yield return null;  // Waits for one frame.
        if (pathSucces) {
            waypoints = GetFinalPath(startTile, targetTile);  // Backtrace the parents to calculate the actual path.

        }
        requestManager.FinishedProcessingPath(waypoints, pathSucces);
    }

    Vector3[] GetFinalPath(Tile fp_startTile, Tile fp_targetTile) {
        List<Tile> finalPath = new List<Tile>();    // List for the path.
        Tile currentTile = fp_targetTile;           // convert parameter to local variable.

        while (currentTile != fp_startTile) {
            finalPath.Add(currentTile);         // add the current tile (target tile) to the final path list.
            currentTile = currentTile.parent;   // Assign the currentTile variable to the current tile's parent.
        }

        Vector3[] waypoints = SimplifyPath(finalPath);
        Array.Reverse(waypoints);    // Reverses the elements in the list so it's right side first.
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Tile> path) {

        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i - 1].xPos - path[i].xPos, path[i - 1].yPos - path[i].yPos);
            if (directionNew != directionOld) {
                waypoints.Add(path[i].position);    // If this doesn't work try path[i - 1];
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetManhattenDistance(Tile md_tileA, Tile md_tileB) {
        int ix = Mathf.Abs(md_tileA.xPos - md_tileB.xPos);  // 
        int iy = Mathf.Abs(md_tileA.yPos - md_tileB.yPos);  // Absolute values for the movement cost.

        return ix + iy;

        //if (ix > iy)
        //    return 14 * iy + 10 * (ix - iy);
        //return 14 * ix + 10 * (iy - ix); 
    }
}