using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour {

    TileGrid grid;  // Reference to the grid
    public Transform startPosition, targetPosition; // Start and target position

    void Awake()
    {
        grid = GetComponent<TileGrid>();    // Assigning the grid.
    }

    private void Update() {
        CalculatePath(startPosition.position, targetPosition.position); // Calling the actual pathfinding class.
    }

    void CalculatePath(Vector3 cp_start, Vector3 cp_target) {
        Tile startTile = grid.TileFromWorldPosition(cp_start);  // Assigning the start position.
        Tile targetTile = grid.TileFromWorldPosition(cp_target); // Assigning the target position.

        List<Tile> openList = new List<Tile>();     // List to check for neighbours.
        HashSet<Tile> closedList = new HashSet<Tile>();   // HashSet for checked neighbours. 
                                                          // (HashSet for better performance).

        openList.Add(startTile);    // Start with the first tile (Start Position).
        while(openList.Count > 0) {
            Tile currentTile = openList[0];
            for (int i = 0; i < openList.Count; i++) {
                // Check if the current node's f-cost is lower or the same as 
                if (openList[i].f < currentTile.f || openList[i].f == currentTile.f && openList[i].h < currentTile.h) {
                    currentTile = openList[i];
                }
            }
            openList.Remove(currentTile);   // If the tile has been checked, remove it from the openList.
            closedList.Add(currentTile);    // And add it to the closedList, so it won't be checked again.

            if (currentTile == targetTile) {            // If the current tile is the target tile
                GetFinalPath(startTile, targetTile);    // Backtrace the parents to calculate the actual path.
                break;
            }

            foreach (Tile neighbour in grid.GetNeighbourTiles(currentTile))
            {
                if (!neighbour.isWall || closedList.Contains(neighbour)) {
                    continue;   // Ignore the neighbour it's a wall.
               
                }

                int moveCost = currentTile.g + GetManhattenDistance(currentTile, neighbour);
                if (!openList.Contains(neighbour) || moveCost < neighbour.f) {
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

    void GetFinalPath(Tile fp_startTile, Tile fp_targetTile) {
        List<Tile> finalPath = new List<Tile>();    // List for the path.
        Tile currentTile = fp_targetTile;           // convert parameter to local variable.

        while (currentTile != fp_startTile) {
            finalPath.Add(currentTile);         // add the current tile (target tile) to the final path list.
            currentTile = currentTile.parent;   // Assign the currentTile variable to the current tile's parent.
        }

        finalPath.Reverse();    // Reverses the elements in the list so it's right side first.

        grid.path = finalPath;  // Copies the final path list to the path list of the grid.
        Debug.Log("Path calculated");
    }

    int GetManhattenDistance(Tile md_tileA, Tile md_tileB) {
        int ix = Mathf.Abs(md_tileA.xPos - md_tileB.xPos);  // 
        int iy = Mathf.Abs(md_tileA.xPos - md_tileB.xPos);  // Absolute values for the movement cost.

        return ix + iy;
    }
}

public class Tile {
    public int xPos, yPos;      // Tile x and y positions.

    public bool isWall;         // Is the tile a wall?
    public Vector3 position;    // position in vector 3 space.
    public Tile parent;         // Tiles parent for backtracing.

    public int g, h;            // G and H cost for the tile. G = distance travelled.. H = estimate distance to target.
    public int f { get { return g + h; } }  // F cost for the tile calculated as shown. 

    public Tile(bool T_isWall, Vector3 T_position, int T_xPos, int T_yPos) {
        // Set all parameters to local variables.
        isWall = T_isWall;
        position = T_position;
        xPos = T_xPos;
        yPos = T_yPos;
    }
}