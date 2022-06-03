using System;
using System.Collections.Generic;
using UnityEngine;
public class Piece {

    public Vector2Int gridIndex;

    public List<Vector2Int> movementVector;

    public int id;

    public Piece(Vector2Int _gridIndex, int _id) {
        this.gridIndex = _gridIndex;
        this.id = _id;

        this.movementVector = new List<Vector2Int>();
    }

}
