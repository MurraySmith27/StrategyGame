using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityTile : Tile
{
    
    public int id;
    public bool[,] ownedTiles;

    public CityTile(Vector2Int _gridSize, Vector2Int _gridIndex, int id): base(_gridSize) {

        this.ownedTiles = new bool[_gridSize.x, _gridSize.y];
        
        this.id = id;
        //populate with false
        for (int x = 0; x < _gridSize.x; x++) {
            for (int y = 0; y < _gridSize.y; y++){
                this.ownedTiles[x, y] = false;
            }
        }
        //the city starts out owning all tiles around it.
        for (int x = _gridIndex.x - 1; x < _gridIndex.x + 2; x++) {
            for (int y = _gridIndex.y - 1; y < _gridIndex.y + 2; y++) {
                this.ownedTiles[x, y] = true;
            }
        }
    }
}
