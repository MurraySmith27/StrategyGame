using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

    public GameObject potentialMovePrefab;

    public GameObject potentialAttackPrefab;

    private List<GameObject> instantiatedPotentialMovePrefabs;

    public static SelectionManager instance;

    private bool selectedThisFrame = false;

    public void Awake() {
        if (!SelectionManager.instance)
            SelectionManager.instance = this;
        
        instantiatedPotentialMovePrefabs = new List<GameObject>();
    }


    public void DeselectAll() {
        foreach (GameObject instantiatedPotentialMovePrefab in this.instantiatedPotentialMovePrefabs) {
            
            Destroy(instantiatedPotentialMovePrefab);
        }

        instantiatedPotentialMovePrefabs.Clear();
    }

    public void SelectObject(int objectId) {

        this.selectedThisFrame = true;

        this.DeselectAll();

        Piece piece = GridManager.instance.GetPieceFromId(objectId);

        if (piece != null) {
            //draw the projected movements.
            Vector2Int piecePos = GridManager.instance.GetPiecePositionFromId(objectId);

            float cellWidth = GridManager.instance.cellWidth;

            foreach (Vector2Int movement in piece.movementVector) {
                
                bool canMoveAt = GridManager.instance.CanMovePieceTo(piece.id, piecePos + movement);
                bool canAttackAt = GridManager.instance.CanPieceAttackAt(piece.id, piecePos + movement);

                //Don't show the move marker if it's an invalid move.
                if (!canMoveAt && !canAttackAt) {
                    continue;
                }

                GameObject pieceActionMarkerGO = Instantiate(
                        canMoveAt ? potentialMovePrefab : canAttackAt ? potentialAttackPrefab : null,
                        new Vector3(
                            cellWidth * (piecePos.x + movement.x) + cellWidth / 2f,
                            0,
                            cellWidth * (piecePos.y + movement.y) + cellWidth / 2f
                        ),
                        Quaternion.identity,
                        gameObject.transform
                    );

                PieceActionMarker pieceActionMarker = pieceActionMarkerGO.GetComponent<PieceActionMarker>();

                pieceActionMarker.position = piecePos + movement;
                pieceActionMarker.underlyingPieceId = piece.id;

                instantiatedPotentialMovePrefabs.Add(pieceActionMarkerGO);
            }
        }
        
        //it's a city
        City city = GridManager.instance.GetCityTileFromId(objectId);

        //TODO: City selection.
    }

    public void LateUpdate() {

        if (GlobalState.instance.mouseMode != mouseModes.SELECTING) {
            this.DeselectAll();
        }
        else if (Input.GetMouseButtonDown(0) && !selectedThisFrame) {
            this.DeselectAll();
            GlobalState.instance.setMouseMode(mouseModes.DEFAULT);
        }
        

        this.selectedThisFrame = false;
    }

}