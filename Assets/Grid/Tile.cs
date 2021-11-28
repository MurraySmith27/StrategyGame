using UnityEngine;

public class Tile
{
    public Vector3 worldPos;
    public Vector2Int gridIndex;

    public Tile(Vector3 _worldPos, Vector2Int _gridIndex) {

        this.worldPos = _worldPos;
        this.gridIndex = _gridIndex;
        
    }


}
