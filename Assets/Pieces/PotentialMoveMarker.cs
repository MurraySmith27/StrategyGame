using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialMoveMarker : PieceActionMarker, IClickable
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
            Debug.LogError("Tried to make a piece move but the marker doesn't know it's own position or id.");
        }
        else {
            GridManager.instance.MovePiece(underlyingPieceId, position);
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
