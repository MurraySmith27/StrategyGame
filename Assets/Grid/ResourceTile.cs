
using UnityEngine;

public class ResourceTile : Tile
{
    public int growthPerTurn;
    public int productionPerTurn;

    public ResourceTile(Vector2Int _gridIndex, int _growthPerTurn, int _productionPerTurn)
    : base(_gridIndex) {
        this.growthPerTurn = _growthPerTurn;
        this.productionPerTurn = _productionPerTurn;
    }
    
}
