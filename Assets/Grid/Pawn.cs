using UnityEngine;

public class Pawn : Piece {

    public int id;
    public Pawn(Vector2Int _gridIndex, int _id): base(_gridIndex) {
        this.id = _id;
    }


    public override Array<(int, int)> getMovements() {

    }

}