using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
