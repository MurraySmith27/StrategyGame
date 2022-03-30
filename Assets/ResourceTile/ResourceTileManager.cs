using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTileManager : MonoBehaviour
{

    public GameObject growthTokenPrefab1;
    public GameObject growthTokenPrefab2;
    public GameObject growthTokenPrefab3;
    public GameObject growthTokenPrefab4;
    public GameObject growthTokenPrefab5;

    public GameObject productionTokenPrefab1;
    public GameObject productionTokenPrefab2;
    public GameObject productionTokenPrefab3;
    public GameObject productionTokenPrefab4;
    public GameObject productionTokenPrefab5;

    public GameObject hoverOverCityTilePrefab;
    private GameObject hoverOverCityTile;

    public GameObject hoverOverPawnPrefab;
    private GameObject hoverOverPawn;

    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetGrowth(int numGrowthPerTurn) {
        

        GameObject x;
        switch(numGrowthPerTurn){

            case 0:
                break;
            case 1:
                Instantiate(growthTokenPrefab1, gameObject.transform, false);
                
                break;
            case 2:
                Instantiate(growthTokenPrefab2, gameObject.transform, false);
                
                break;
            case 3:
                Instantiate(growthTokenPrefab3, gameObject.transform, false);
                
                break;
            case 4:
                Instantiate(growthTokenPrefab4, gameObject.transform, false);
                
                break;
            case 5:
                x = Instantiate(growthTokenPrefab5, gameObject.transform, false);
                break;

            default:
                Debug.Log("tried to instantiate resource tile with more than 5 growth.");
                break;   
        }
    }

    public void SetProduction(int numProductionPerTurn) {

        GameObject x;
        switch(numProductionPerTurn){

            case 0:
                break;
            case 1:
                Instantiate(productionTokenPrefab1, gameObject.transform, false);
                
                break;
            case 2:
                Instantiate(productionTokenPrefab2, gameObject.transform, false);
                
                break;
            case 3:
                Instantiate(productionTokenPrefab3, gameObject.transform, false);
                
                break;
            case 4:
                Instantiate(productionTokenPrefab4, gameObject.transform, false);
                
                break;
            case 5:
                x = Instantiate(productionTokenPrefab5, gameObject.transform, false);
                break;

            default:
                Debug.Log("tried to instantiate resource tile with more than 5 production.");
                break;   
        }
    }

    public void OnMouseDown() {
        if (GlobalState.instance.mouseMode == mouseModes.PLACE_CITY) {

            PlayerManager.instance.AddCityForPlayer(GlobalState.instance.currentPlayer, new Vector2Int((int)(gameObject.transform.position.x / GridManager.instance.cellWidth), (int)(gameObject.transform.position.z / GridManager.instance.cellWidth)));
        }
        else if (GlobalState.instance.mouseMode == mouseModes.PLACE_PAWN) {
            PlayerManager.instance.AddPieceForPlayer(GlobalState.instance.currentPlayer, new Vector2Int((int)(gameObject.transform.position.x / GridManager.instance.cellWidth), (int)(gameObject.transform.position.z / GridManager.instance.cellWidth)),
                PieceTypes.Pawn);
        }
        GlobalState.instance.setMouseMode(mouseModes.DEFAULT);
    }

    public void OnMouseOver() {
        Debug.Log("mousemode: " + GlobalState.instance.mouseMode);
        Debug.Log("mouseModes " + mouseModes.PLACE_CITY);
        if (GlobalState.instance.mouseMode == mouseModes.PLACE_CITY && hoverOverCityTile == null) {
            Debug.Log("Placing city at: " + (int)gameObject.transform.position.x + ", " + (int)gameObject.transform.position.y);
            hoverOverCityTile = Instantiate(hoverOverCityTilePrefab, gameObject.transform);
        }
        else if (GlobalState.instance.mouseMode != mouseModes.PLACE_CITY && hoverOverCityTile != null) {
            Destroy(hoverOverCityTile);
            hoverOverCityTile = null;
        }
        if (GlobalState.instance.mouseMode == mouseModes.PLACE_PAWN && hoverOverPawn == null) {
            hoverOverPawn = Instantiate(hoverOverPawnPrefab, gameObject.transform);
        }
        else if (GlobalState.instance.mouseMode != mouseModes.PLACE_PAWN && hoverOverPawn != null) {
            Destroy(hoverOverPawn);
            hoverOverPawn = null;
        }
    }

    public void OnMouseExit() {
        if (hoverOverCityTile != null) {
            Destroy(hoverOverCityTile);
            hoverOverCityTile = null;
        }
        if (hoverOverPawn != null) {
            Destroy(hoverOverPawn);
            hoverOverPawn = null;
        }
    }
}
