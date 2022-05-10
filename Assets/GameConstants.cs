public class GameConstants
{
    public static int pawnCost = 1;
    public static int cityCost = 5;

    public static int getCostFromPiece(PieceTypes type) {
        switch (type) {
            case PieceTypes.Pawn:
                return pawnCost;
            default:
                break;
        }
        return 0;
    }
}
