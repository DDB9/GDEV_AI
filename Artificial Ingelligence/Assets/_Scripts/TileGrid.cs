using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour {

    public bool displayGridGizmos;
    public LayerMask wallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float distance;

    public Tile[,] Grid { get; private set; }

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake() {
        nodeDiameter = nodeRadius * 2; // Size of the cubes drawn as the grid.
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);   // X size of the Grid.
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);   // Y size of the Grid.

        CreateGrid();   // Create the grid.
    }


    public void CreateGrid() {
        Grid = new Tile[gridSizeX, gridSizeY];

        // Finding the bottom-left corner to draw the gizmo cubes on.
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++) {       // Converting the grid to worldpoints.
            for (int y = 0; y < gridSizeY; y++){
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool isWall = false;
                if (Physics.CheckSphere(worldPoint, nodeRadius, wallMask)) {    // checks for colliders on the wall layermask.
                    isWall = true;  //if so, set to true.
                }

                Grid[x, y] = new Tile(isWall, worldPoint, x, y);  // Creating the tile as wall or not.
            }

        }
    }

    public void Reset() {
        foreach(var tile in Grid) {
            tile.g = 0;
            tile.h = 0;
            tile.parent = null;
        }
    }

    public Tile TileFromWorldPosition(Vector3 t_worldPosition) {
        float xPoint = ((t_worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x); // looks for the closest tile relative to the actual position.
        float yPoint = ((t_worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y); // same here, but for the Y position.

        xPoint = Mathf.Clamp01(xPoint); //
        yPoint = Mathf.Clamp01(yPoint); // Clamp between 0 and 1;

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint); //
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint); // Round the final answer down to an integer. 

        return Grid[x, y];  // Returns the tile converted from the world position.
    }

    public List<Tile> GetNeighbourTiles(Tile n_tile) {
        List<Tile> neighbourTiles = new List<Tile>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) continue;

                int xCheck = n_tile.xPos + x;
                int yCheck = n_tile.yPos + y;

                if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY) {
                    neighbourTiles.Add(Grid[xCheck, yCheck]);
                }
            }
        }
        return neighbourTiles;
    }

    // Draws Gizmos in the scene view for clarification.
    private void OnDrawGizmos() { 
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (Grid != null && displayGridGizmos) {
            foreach (Tile tile in Grid) {                                                // Paints the cubes drawn as the grid specific colors;
                Gizmos.color = (!tile.isWall)?Color.white:Color.yellow;                  // Colors the gizmo cubes.
                Gizmos.DrawCube(tile.position, Vector3.one * (nodeDiameter - distance)); // Draws the grid with cubes.
            }
        }
    }
}
