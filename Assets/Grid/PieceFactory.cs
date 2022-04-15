

public class PieceFactory {

    public static Piece create(string type) {
        switch (type) {
            case PieceTypes.Pawn:
                return new Pawn(position, newPieceId);
                break;
            default: 
                break;
        }
    }
}