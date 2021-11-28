using UnityEngine;


class City 
{
    public bool[,] ownedTiles;

    public Vector2Int position;

    public City(Vector2Int _position, Vector2Int _gridSize) {
        this.position = _position;

        this.ownedTiles = new bool[_gridSize.x, _gridSize.y];

        //the city starts out owning all tiles around it.
        for (int x = _position.x - 1; x < position.x + 2; x++) {
            for (int y = position.y - 1; y < position.y + 2; y++){
                this.ownedTiles[x, y] = true;
            }
        }
    }
}