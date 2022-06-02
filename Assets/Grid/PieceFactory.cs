using UnityEngine;

public class PieceFactory {

    public static Piece create(PieceTypes type, Vector2Int position, int newPieceId) {
        switch (type) {
            case PieceTypes.Pawn:
                return new Pawn(position, newPieceId);
            default: 
                return new Piece(position, newPieceId);
        }
    }
}