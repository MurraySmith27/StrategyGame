using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialAttackMarker : PieceActionMarker
{

    public void Update() {
        if (
            MouseEventUtils.IsClicked(
                Camera.main, 
                gameObject.GetComponent<Collider>(),
                LayerMask.GetMask("PotentialMoveMarker")
            ) && ShouldProcessMouseEvent()) OnClick();
    }
    public void OnClick() {
        
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

    public bool ShouldProcessMouseEvent() {

        string[] validMouseModes = {
            mouseModes.SELECTING.ToString()
        };

        for (int i = 0; i < validMouseModes.Length; i++) {
            if (GlobalState.instance.mouseMode.ToString() == validMouseModes[i]) {
                return true;
            }
        }
        return false;
    }
}
