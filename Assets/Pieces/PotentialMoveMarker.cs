using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialMoveMarker : MonoBehaviour
{
    
    public Vector2Int position;
    public int underlyingPieceId = -1;

    public void OnMouseDown() {
        if (position == null || underlyingPieceId == -1) {
            Debug.LogError("Tried to make a piece move but the marker doesn't know it's own position or id.");
        }
        else {
            GridManager.instance.MovePiece(underlyingPieceId, position);
        }
    }
}
