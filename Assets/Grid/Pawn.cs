using System;
using System.Collections.Generic;
using UnityEngine;


public class Pawn : Piece {

    public int id;
    public Pawn(Vector2Int _gridIndex, int _id): base(_gridIndex) {
        this.id = _id;
    }


    public override (int, int)[] getMovements() {
        (int, int)[] movements = {};
        return movements;
    }

}