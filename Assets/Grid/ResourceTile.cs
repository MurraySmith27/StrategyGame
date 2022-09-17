
using UnityEngine;

public enum ResourceTileTypes
{
    Normal,
    Forest
}

public class ResourceTile : Tile
{
    public int growthPerTurn;
    public int productionPerTurn;

    public ResourceTileTypes type;

    public ResourceTile(Vector2Int _gridIndex, int _growthPerTurn, int _productionPerTurn, ResourceTileTypes type)
    : base(_gridIndex) {
        this.growthPerTurn = _growthPerTurn;
        this.productionPerTurn = _productionPerTurn;
        this.type = type;
    }
    
}
