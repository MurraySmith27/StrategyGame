using UnityEngine;

public class PieceFactory {

    public static Piece create(PieceTypes type, Vector2Int position, int newPieceId, int pawnDirection = 0) {
        switch (type) {
            case PieceTypes.Pawn:
                return new Pawn(position, newPieceId, pawnDirection);
            default: 
                return null;
        }
    }
}