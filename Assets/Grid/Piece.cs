using UnityEngine;

public class Piece {

    public Vector2Int gridIndex;

    public Piece(Vector2Int _gridIndex) {
        this.gridIndex = _gridIndex;
    }


    public virtual (int, int) getMovementPaths;

}
