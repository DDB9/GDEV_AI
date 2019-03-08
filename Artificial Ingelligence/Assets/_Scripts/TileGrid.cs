using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public Transform StartPosition;
    public LayerMask wallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float distance;

    Tile[,] grid;
    public List<Tile> path;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start() {
        nodeDiameter = nodeRadius * 2; // Size of the cubes drawn as the grid.
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);   // X size of the Grid.
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);   // Y size of the Grid.

        CreateGrid();   // Create the grid.
    }
 
    void CreateGrid() {
        grid = new Tile[gridSizeX, gridSizeY];
        // Finding the bottom-left corner to draw the gizmo cubes on.
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++) {       // Converting the grid to worldpoints.
            for (int y = 0; y < gridSizeY; y++){
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool isWall = false;
                if (Physics.CheckSphere(worldPoint, nodeRadius, wallMask)) {    // checks for colliders on the wall layermask.
                    isWall = true;  //if so, set to true.
                }

                grid[x, y] = new Tile(isWall, worldPoint, x, y);  // Creating the tile as wall or not.
            }

        }
    }

    public Tile TileFromWorldPosition(Vector3 t_worldPosition) {
        float xPoint = ((t_worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x); // looks for the closest tile relative to the actual position.
        float yPoint = ((t_worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y); // same here, but for the Y position.

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[x, y];  // Returns the actual grid with x and y positions.
    }

    public List<Tile> GetNeighbourTiles(Tile n_tile) {
        List<Tile> neighbourTiles = new List<Tile>();
        int xCheck, yCheck;

        // First check the neighbour on the top...
        xCheck = n_tile.xPos;           // Same X position.
        yCheck = n_tile.yPos + 1;       // And one upwards.
        if (xCheck >= 0 && yCheck < gridSizeY) {
            neighbourTiles.Add(grid[xCheck, yCheck]); // Add the tile to the neighbourhood.
        }

        // Second, check the neighbour to the left...
        xCheck = n_tile.xPos - 1;   // One to the left.
        yCheck = n_tile.yPos;       // And same Y position.
        if (xCheck >= 0 && yCheck < gridSizeY)
        {
            neighbourTiles.Add(grid[xCheck, yCheck]); // Add the tile to the neighbourhood.
        }

        // Third, check the neighbour at the bottom...
        xCheck = n_tile.xPos;           // Same X position.
        yCheck = n_tile.yPos - 1;       // And one to downwards.
        if (xCheck >= 0 && yCheck < gridSizeY)
        {
             neighbourTiles.Add(grid[xCheck, yCheck]); // Add the tile to the neighbourhood.
        }

        // Lastly, check the neighbour to the right.
        xCheck = n_tile.xPos + 1;   // One to the right.
        yCheck = n_tile.yPos;       // And same Y position.
        if (xCheck >= 0 && yCheck < gridSizeY)
        {
            neighbourTiles.Add(grid[xCheck, yCheck]); // Add the tile to the neighbourhood.
        }

        return neighbourTiles;
    }

    private void OnDrawGizmos() { // Draws Gizmos in the scene view for clarification.
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null) {
            foreach (Tile tile in grid) {           // Paints the cubes drawn as the grid specific colors;
                if (!tile.isWall) {
                    Gizmos.color = Color.white;     // If the tile is not a wall, paint it white.
                }
                else {
                    Gizmos.color = Color.yellow;    // Else if the tile IS a wall, paint it yellow.   
                }

                if (path != null) {
                    if (path.Contains(tile)) {
                        Gizmos.color = Color.red;     // the calculated path will be painted red.
                    }
                }

                // Draws the grid with cubes.
                Gizmos.DrawCube(tile.position, Vector3.one * (nodeDiameter - distance));
            }
        }
    }
}
