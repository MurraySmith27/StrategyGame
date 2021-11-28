using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityTile : Tile
{

    public bool[,] ownedTiles;

    public CityTile(Vector3 _worldPos, Vector2Int _gridSize, Vector2Int _gridIndex): base(_worldPos, _gridSize) {

        this.ownedTiles = new bool[_gridSize.x, _gridSize.y];

        //the city starts out owning all tiles around it.
        for (int x = (int)_worldPos.x - 1; x < ((int)_worldPos.x) + 2; x++) {
            for (int y = (int)_worldPos.z - 1; y < ((int)_worldPos.z) + 2; y++){
                this.ownedTiles[x, y] = true;
            }
        }
    }
}
