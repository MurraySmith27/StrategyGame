using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City
{
    
    public int id;
    public bool[,] ownedTiles;

    public Vector2Int position;
    public int maxHealth;
    
    public int currentHealth;

    public City(Vector2Int _gridSize, Vector2Int _gridIndex, int id) {

        this.maxHealth = 3;

        this.currentHealth = this.maxHealth;

        this.position = _gridIndex;

        this.ownedTiles = new bool[_gridSize.x, _gridSize.y];
        
        this.id = id;
    }
}
