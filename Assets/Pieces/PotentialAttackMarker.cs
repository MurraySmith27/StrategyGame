using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialAttackMarker : PieceActionMarker
{

    public void OnMouseDown() {
        if (position == null || underlyingPieceId == -1) {
            Debug.LogError("Tried to make a piece attack but the marker doesn't know it's own position or id.");
        }
        else {
            bool attackerSurvives = GridManager.instance.WillAttackerSurvive(underlyingPieceId, position);

            GridManager.instance.ProcessAttack(underlyingPieceId, position);
            if (attackerSurvives) {
                GridManager.instance.MovePiece(underlyingPieceId, position);
            }
            else {
                GridManager.instance.DestroyPiece(underlyingPieceId);
            }

            GlobalState.instance.setMouseMode(mouseModes.DEFAULT);
        }
    }
}
