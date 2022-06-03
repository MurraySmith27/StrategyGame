using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

    public GameObject potentialMovePrefab;

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
                
                //Don't show the move marker if it's an invalid move.
                if (!GridManager.instance.CanMovePieceTo(piece.id, piecePos + movement)) {
                    continue;
                }

                GameObject potentialMoveMarker = Instantiate(
                    potentialMovePrefab, 
                    new Vector3(
                        cellWidth * (piecePos.x + movement.x) + cellWidth / 2f, 
                        0, 
                        cellWidth * (piecePos.y + movement.y) + cellWidth / 2f
                    ), 
                    Quaternion.identity, 
                    gameObject.transform
                );

                PotentialMoveMarker moveMarkerComponent = potentialMoveMarker.GetComponent<PotentialMoveMarker>();

                moveMarkerComponent.position = piecePos + movement;
                moveMarkerComponent.underlyingPieceId = piece.id;

                instantiatedPotentialMovePrefabs.Add(potentialMoveMarker);
            }
        }
        
        //it's a city
        CityTile city = GridManager.instance.GetCityTileFromId(objectId);

        //TODO: City selection.
    }

    public void Update() {

        if (Input.GetMouseButtonDown(0) && !selectedThisFrame) {
            this.DeselectAll();
        }

        this.selectedThisFrame = false;
    }

}