using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTileManager : MonoBehaviour, IClickable
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

    private GameObject instantiatedGrowthToken;
    private GameObject instantiatedProductionToken;

    public GameObject hoverOverCityTilePrefab;
    private GameObject hoverOverCityTile;

    public GameObject hoverOverCityTileBorderPrefab;
    public List<GameObject> hoverOverCityTileBorders;

    public GameObject hoverOverPawnPrefab;

    public GameObject hoverOverPawnArrowPrefab;
    private GameObject hoverOverPawn;

    private GameObject previewPlacePawn;

    private GameObject previewPawnArrow;

    public Camera cam;

    private bool isMouseOver = false;
    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //detect mouse events
        if (ShouldProcessMouseEvent()) {
            if (MouseEventUtils.IsClicked(
                Camera.main, 
                gameObject.GetComponent<Collider>(),
                LayerMask.GetMask("ResourceTile")
                )) OnClick();
            else {
                if (MouseEventUtils.IsMouseOver(
                    Camera.main, 
                    gameObject.GetComponent<Collider>(),
                    LayerMask.GetMask("ResourceTile")
                    )) {
                    if (!this.isMouseOver){
                        StartMouseOver();
                        this.isMouseOver = true;
                    }
                }
                else if (this.isMouseOver) {
                    this.isMouseOver = false;
                    EndMouseOver();
                }
            }
        }

        if (GlobalState.instance.mouseMode == mouseModes.CHOOSE_PAWN_DIRECTION && 
            GlobalState.instance.pawnToPlacePosition.x == gameObject.transform.position.x && 
            GlobalState.instance.pawnToPlacePosition.y == gameObject.transform.position.z &&
            previewPlacePawn == null) {       
            previewPlacePawn = Instantiate(hoverOverPawnPrefab, gameObject.transform);
        }
        else if (GlobalState.instance.mouseMode != mouseModes.CHOOSE_PAWN_DIRECTION && previewPlacePawn != null) {
            Destroy(previewPlacePawn);
            previewPlacePawn = null;
        }

        if (GlobalState.instance.mouseMode == mouseModes.DEFAULT) {
            DestroyHoverOverPrefabs();
        }
    }

    public void SetGrowth(int numGrowthPerTurn) {

        switch(numGrowthPerTurn){

            case 0:
                break;
            case 1:
                this.instantiatedGrowthToken = Instantiate(
                    growthTokenPrefab1,
                    gameObject.transform.position,
                    Quaternion.LookRotation(new Vector3(1, 1, 1), new Vector3(-1, 1, -1))
                );
                break;
            case 2:
                this.instantiatedGrowthToken = Instantiate(
                    growthTokenPrefab2,
                    gameObject.transform.position,
                    Quaternion.LookRotation(new Vector3(1, 1, 1), new Vector3(-1, 1, -1))
                );
                break;
            case 3:
                this.instantiatedGrowthToken = Instantiate(
                    growthTokenPrefab3,
                    gameObject.transform.position,
                    Quaternion.LookRotation(new Vector3(1, 1, 1), new Vector3(-1, 1, -1))
                );
                break;
            case 4:
                this.instantiatedGrowthToken = Instantiate(
                    growthTokenPrefab4,
                    gameObject.transform.position,
                    Quaternion.LookRotation(new Vector3(1, 1, 1), new Vector3(-1, 1, -1))
                );
                break;
            case 5:
                this.instantiatedGrowthToken = Instantiate(
                    growthTokenPrefab5,
                    gameObject.transform.position,
                    Quaternion.LookRotation(new Vector3(1, 1, 1), new Vector3(-1, 1, -1))
                );
                break;
            default:
                Debug.LogError("tried to instantiate resource tile with more than 5 growth.");
                break;   
        }
    }

    public void SetProduction(int numProductionPerTurn) {

        switch(numProductionPerTurn){

            case 0:
                break;
            case 1:
                this.instantiatedProductionToken = Instantiate(
                    productionTokenPrefab1,
                    gameObject.transform.position,
                    Quaternion.LookRotation(new Vector3(1, 1, 1), new Vector3(-1, 1, -1))
                );
                break;
            case 2:
                this.instantiatedProductionToken = Instantiate(
                    productionTokenPrefab2,
                    gameObject.transform.position,
                    Quaternion.LookRotation(new Vector3(1, 1, 1), new Vector3(-1, 1, -1))
                );
                break;
            case 3:
                this.instantiatedProductionToken = Instantiate(
                    productionTokenPrefab3,
                    gameObject.transform.position,
                    Quaternion.LookRotation(new Vector3(1, 1, 1), new Vector3(-1, 1, -1))
                );
                break;
            case 4:
                this.instantiatedProductionToken = Instantiate(
                    productionTokenPrefab4,
                    gameObject.transform.position,
                    Quaternion.LookRotation(new Vector3(1, 1, 1), new Vector3(-1, 1, -1))
                );
                break;
            case 5:
                this.instantiatedProductionToken = Instantiate(
                    productionTokenPrefab5,
                    gameObject.transform.position,
                    Quaternion.LookRotation(new Vector3(1, 1, 1), new Vector3(-1, 1, -1))
                );
                break;

            default:
                Debug.LogError("tried to instantiate resource tile with more than 5 production.");
                break;   
        }
    }

    private int GetPawnFacingDirection() {
        int pawnDirection;
        Vector3 thisPosition = gameObject.transform.position;
        Vector2 pawnPosition = GlobalState.instance.pawnToPlacePosition;
        if (thisPosition.x < pawnPosition.x) {
            if (-(thisPosition.z - pawnPosition.y) < (thisPosition.x - pawnPosition.x)) {
                //top quadrant.
                pawnDirection = 0;
            }
            else if ((thisPosition.z - pawnPosition.y) < (thisPosition.x - pawnPosition.x)) {
                //bottom quadrant.
                pawnDirection = 2;
            }
            else {
                //left quadrant
                pawnDirection = 3;
            }
        }
        else {
            if ((thisPosition.z - pawnPosition.y) > (thisPosition.x - pawnPosition.x)) {
                //top quadrant.
                pawnDirection = 0;
            }
            else if (-(thisPosition.z - pawnPosition.y) > (thisPosition.x - pawnPosition.x)) {
                //bottom quadrant.
                pawnDirection = 2;
            }
            else {
                //right quadrant
                pawnDirection = 1;
            }
        }
        return pawnDirection;
    }

    public bool ShouldProcessMouseEvent() {

        string[] validMouseModes = {
            mouseModes.DEFAULT.ToString(), 
            mouseModes.PLACE_CITY.ToString(), 
            mouseModes.PLACE_PAWN.ToString(), 
            mouseModes.CHOOSE_PAWN_DIRECTION.ToString() 
        };

        for (int i = 0; i < validMouseModes.Length; i++) {
            if (GlobalState.instance.mouseMode.ToString() == validMouseModes[i]) {
                return true;
            }
        }
        return false;
    }

    public void OnClick() {

        if (GlobalState.instance.mouseMode == mouseModes.PLACE_CITY) {
            PlayerManager.instance.AddCityForPlayer(
                GlobalState.instance.currentPlayer, 
                new Vector2Int(
                    (int)(gameObject.transform.position.x / GridManager.instance.cellWidth), 
                    (int)(gameObject.transform.position.z / GridManager.instance.cellWidth)
                )
            );
        }
        else if (GlobalState.instance.mouseMode == mouseModes.PLACE_PAWN) {
            if (PlayerManager.instance.CanPlacePawn(GlobalState.instance.currentPlayer)) {
                if (GridManager.instance.CanPlacePieceAt(new Vector2Int(
                            (int)(gameObject.transform.position.x / GridManager.instance.cellWidth), 
                            (int)(gameObject.transform.position.z / GridManager.instance.cellWidth)
                        ), 
                        PieceTypes.Pawn)) {
                    GlobalState.instance.pawnToPlacePosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
                    GlobalState.instance.setMouseMode(mouseModes.CHOOSE_PAWN_DIRECTION);
                    return;
                }
                else {
                    PopupMessage.CreatePopupMessage("Nope!", $"Invalid location for a Pawn!");
                }
            }
            else {
                PopupMessage.CreatePopupMessage("Nope!", $"Not enough production to place a Pawn! A Pawn requires {GameConstants.getCostFromPiece(PieceTypes.Pawn)} production.");
            }
        }
        else if (GlobalState.instance.mouseMode == mouseModes.CHOOSE_PAWN_DIRECTION) {
            if (GlobalState.instance.pawnToPlacePosition.x == -1 || 
                GlobalState.instance.pawnToPlacePosition.y == -1) {
                Debug.LogError("Error: in mousemode \"CHOOSE_PAWN_DIRECTION\" but global state has no registered position of a pawn to place.");
            }
            else {
                int pawnDirection = this.GetPawnFacingDirection();
                PlayerManager.instance.AddPieceForPlayer(
                    GlobalState.instance.currentPlayer, 
                    new Vector2Int(
                        (int)(GlobalState.instance.pawnToPlacePosition.x / GridManager.instance.cellWidth), 
                        (int)(GlobalState.instance.pawnToPlacePosition.y / GridManager.instance.cellWidth)
                    ),
                    PieceTypes.Pawn,
                    pawnDirection: pawnDirection
                );
            }
            GlobalState.instance.pawnToPlacePosition = new Vector2Int(-1, -1);
        }
        GlobalState.instance.setMouseMode(mouseModes.DEFAULT);
    }

    public void StartMouseOver() {
        if (GlobalState.instance.mouseMode == mouseModes.PLACE_CITY && hoverOverCityTile == null) {
            hoverOverCityTile = Instantiate(hoverOverCityTilePrefab, gameObject.transform);
            //also instantiate the borders.
            foreach (Vector2Int borderPosition in GridManager.instance.GetNewCityOwnedTiles(
                new Vector2Int(
                    (int)(gameObject.transform.position.x / GridManager.instance.cellWidth),
                    (int)(gameObject.transform.position.z / GridManager.instance.cellWidth)
                ))
            ) {
                hoverOverCityTileBorders.Add(Instantiate(
                    hoverOverCityTileBorderPrefab,
                    new Vector3(
                        borderPosition.x * GridManager.instance.cellWidth  + GridManager.instance.cellWidth / 2f,
                        gameObject.transform.position.y,
                        borderPosition.y * GridManager.instance.cellWidth + GridManager.instance.cellWidth / 2f
                    ),                    
                    gameObject.transform.rotation
                ));
            }
        }
        else if (GlobalState.instance.mouseMode != mouseModes.PLACE_CITY && hoverOverCityTile != null) {
            Destroy(hoverOverCityTile);
            foreach (GameObject instantiatedBorderPrefab in hoverOverCityTileBorders)
            {
                Destroy(instantiatedBorderPrefab);
            }
            hoverOverCityTileBorders.Clear();
            hoverOverCityTile = null;
        }
        if (GlobalState.instance.mouseMode == mouseModes.PLACE_PAWN && hoverOverPawn == null) {
            hoverOverPawn = Instantiate(hoverOverPawnPrefab, gameObject.transform);
        }
        else if (GlobalState.instance.mouseMode != mouseModes.PLACE_PAWN && hoverOverPawn != null) {
            Destroy(hoverOverPawn);
            hoverOverPawn = null;
        }
        if (GlobalState.instance.mouseMode == mouseModes.CHOOSE_PAWN_DIRECTION && 
            GlobalState.instance.pawnToPlacePosition.x != -1 &&
            GlobalState.instance.pawnToPlacePosition.y != -1) {
            int direction = GetPawnFacingDirection();
            int yRotation = 0;
            switch (direction) {
                case 0:
                    yRotation = 0;
                    break;
                case 1:
                    yRotation = 90;
                    break;
                case 2:
                    yRotation = 180;
                    break;
                case 3:
                    yRotation = -90;
                    break;
                default:
                    break;
            }
            previewPawnArrow = Instantiate(hoverOverPawnArrowPrefab, new Vector3(GlobalState.instance.pawnToPlacePosition.x, VisualConfig.tileInstantiateHeight, GlobalState.instance.pawnToPlacePosition.y), Quaternion.Euler(0, yRotation, 0), gameObject.transform);
        }
        else if (GlobalState.instance.mouseMode != mouseModes.CHOOSE_PAWN_DIRECTION && previewPawnArrow != null) {
            Destroy(previewPawnArrow);
            previewPawnArrow = null;
        } 
    }

    public void EndMouseOver() {
        DestroyHoverOverPrefabs();
    }

    private void DestroyHoverOverPrefabs() {
        if (hoverOverCityTile != null) {
            Destroy(hoverOverCityTile);
            hoverOverCityTile = null;
        }
        if (hoverOverPawn != null) {
            Destroy(hoverOverPawn);
            hoverOverPawn = null;
        }
        if (previewPawnArrow != null){
            Destroy(previewPawnArrow);
            previewPawnArrow = null;
        }

        foreach (GameObject instantiatedBorderPrefab in hoverOverCityTileBorders)
        {
            Destroy(instantiatedBorderPrefab);
        }
        hoverOverCityTileBorders.Clear();
    }

    public void OnDestroy()
    {
        DestroyHoverOverPrefabs();

        Destroy(this.instantiatedGrowthToken);
        Destroy(this.instantiatedProductionToken);
    }
}
