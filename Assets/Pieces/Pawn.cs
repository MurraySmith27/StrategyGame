using System;
using System.Collections.Generic;
using UnityEngine;


public class Pawn : Piece {

    public int direction = 0; //0: north, 1: east, 2: south, 3: west

    public Pawn(Vector2Int _gridIndex, int _id, int _direction): base(_gridIndex, _id) {
        
        this.maxHealth = 1;

        this.currentHealth = 1;

        this.damageOutput = 1;
        
        this.direction = _direction;
        
        if (this.movementVector == null)
            this.movementVector = new List<Vector2Int>();

        switch (direction) {
            case 0: 
                //North
                this.movementVector.Add(new Vector2Int(-1, 1));
                this.movementVector.Add(new Vector2Int(1, 1));
                break;
            case 1:
                //East
                this.movementVector.Add(new Vector2Int(1, 1));
                this.movementVector.Add(new Vector2Int(1, -1));
                break;
            case 2: 
                //South
                this.movementVector.Add(new Vector2Int(1, -1));
                this.movementVector.Add(new Vector2Int(-1, -1));
                break;
            case 3: 
                //West
                this.movementVector.Add(new Vector2Int(-1, -1));
                this.movementVector.Add(new Vector2Int(-1, 1));
                break;
            default:
                break;
        }
    }

}