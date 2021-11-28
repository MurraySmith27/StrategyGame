
using UnityEngine;

public class ResourceTile : Tile
{
    public int growthPerTurn;
    public int productionPerTurn;

    public ResourceTile(Vector3 _worldPos, Vector2Int _gridIndex, int _growthPerTurn, int _productionPerTurn)
    : base(_worldPos, _gridIndex) {
        this.growthPerTurn = _growthPerTurn;
        this.productionPerTurn = _productionPerTurn;
    }
    
}
