using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PieceTypes {
    Pawn,
}

public class GridManager : MonoBehaviour, IGameOverAction
{
    
    public static GridManager instance;

    public Vector2Int gridSize;
    public float cellWidth = 10f;

    public Grid grid;

    public GameObject resourceTilePrefab;

    public GameObject cityTilePrefab;

    public GameObject borderPrefab;

    public GameObject pawnPrefab;

    public GameObject forestTilePrefab;

    private GridDebug gridDebug;

    public int averageResourceCount = 2;

    private ArrayList borderPrefabs;

    private bool gridChanged = false;

    private GameObject[,] instantiatedGridPrefabs;

    private GameObject[,] instantiatedForestPrefabs;

    private GameObject[,] instantiatedPiecePrefabs;


    //---- Begin GridManager API functions ----

    public void OnGameOver(int winningPlayer)
    {
        //TODO game over logic (clear the board).
    }

    public (int, int) GetCityGrowthAndProd(int cityId) {
        return this.grid.GetCityGrowthAndProd(cityId);
    }

    public bool CanPlaceCityAt(Vector2Int position) {
        
        return this.grid.CanPlaceCity(position);
    }

    public bool CanPlacePieceAt(Vector2Int position, PieceTypes type) {
        return this.grid.CanPlacePieceAt(position, type);
    }

    //Create a new city. Return the id of the new city, or -1 if it could not be created.
    public int AddCity(Vector2Int position) {
        int newCityId = this.grid.AddCity(position);
        
        //trigger an update if successful.
        this.gridChanged = this.gridChanged || newCityId != -1;

        return newCityId;
    }

    //Create a new piece. Return the id of the piece or -1 if the piece could not be created.
    public int AddPiece(Vector2Int position, PieceTypes type, int pawnDirection = -1) {
        int newPieceId = this.grid.AddPiece(position, type, pawnDirection: pawnDirection);
        
        //trigger an update if successful.
        this.gridChanged = this.gridChanged || newPieceId != -1;

        return newPieceId;
    }

    public bool CanMovePieceTo(int pieceId, Vector2Int positionToMoveTo) {
        City city = this.grid.GetCityAt(positionToMoveTo.x, positionToMoveTo.y);
        Piece piece = this.grid.GetPieceAt(positionToMoveTo.x,positionToMoveTo.y);

        return piece == null && city == null;
    }

    public bool CanPieceAttackAt(int pieceId, Vector2Int positionToAttackAt) {

        int player = PlayerManager.instance.GetPlayerFromPieceId(pieceId);

        City city = this.grid.GetCityAt(positionToAttackAt.x, positionToAttackAt.y);

        if (city != null && PlayerManager.instance.GetPlayerFromCityId(city.id) != player) {
            return true;
        }

        Piece piece = this.grid.GetPieceAt(positionToAttackAt.x, positionToAttackAt.y);

        if (piece != null && PlayerManager.instance.GetPlayerFromPieceId(piece.id) != player) {
            return true;
        }

        return false;
    }

    public bool WillAttackerSurvive(int attackerId, Vector2Int positionToAttackAt) {
        //attacker should survive if he hits the enemy for their last hit point `
        Piece attackerPiece = this.GetPieceFromId(attackerId);

        if (attackerPiece != null) {
            //case 1: defender is a piece
            Piece defenderPiece = this.grid.GetPieceAt(positionToAttackAt.x, positionToAttackAt.y);

            if (defenderPiece != null) {
                return true;
            }
            else {
                //case 2: defender is a city
                City defenderCity = this.grid.GetCityAt(positionToAttackAt.x, positionToAttackAt.y);
                if (defenderCity != null) {
                    if (defenderCity.currentHealth <= attackerPiece.damageOutput) return true;
                    else return false;
                }
                else {
                    Debug.LogError($"Could not find a defender at position ({positionToAttackAt.x}, {positionToAttackAt.y})." 
                                    + " This means that this action was marked as an attack despite not being one.");
                    Debug.Break();
                }
            }
        }
        else {
            Debug.LogError($"Could not find piece with id {attackerId} when calling Piece.WillAttackerSurvive");
            Debug.Break();
        }

        return false;
    }

    public void ProcessAttack(int attackerId, Vector2Int positionToAttackAt) {
        Piece attackerPiece = this.GetPieceFromId(attackerId);
        int attackingPlayer = PlayerManager.instance.GetPlayerFromPieceId(attackerId);

        if (attackerPiece != null) {
            //case 1: defender is a piece
            Piece defenderPiece = this.grid.GetPieceAt(positionToAttackAt.x, positionToAttackAt.y);

            if (defenderPiece != null) {
                this.DestroyPiece(defenderPiece.id);
            }
            else {
                //case 2: defender is a city
                City defenderCity = this.grid.GetCityAt(positionToAttackAt.x, positionToAttackAt.y);
                if (defenderCity != null) {
                    defenderCity.currentHealth -= attackerPiece.damageOutput;
                    if (defenderCity.currentHealth <= 0) {
                        int defendingPlayer = PlayerManager.instance.GetPlayerFromCityId(defenderCity.id);
                        this.DestroyCity(defenderCity.id);
                        //now that city is destroyed, check if defending player has any cities left.
                        if (PlayerManager.instance.GetNumCitiesForPlayer(defendingPlayer) == 0)
                        {
                            //game over!
                            GlobalState.instance.EndGame(attackingPlayer);
                            return;
                        }
                    }
                }
                else {
                    Debug.LogError($"Could not find a defender at position ({positionToAttackAt.x}, {positionToAttackAt.y})." 
                                    + " This means that this action was marked as an attack despite not being one.");
                    Debug.Break();
                }
            }
        }
        else {
            Debug.LogError($"Could not find piece with id {attackerId} when calling Piece.WillAttackerSurvive");
            Debug.Break();
        }
    }

    public void MovePiece(int pieceId, Vector2Int positionToMoveTo) {
        if (!CanMovePieceTo(pieceId, positionToMoveTo)) {
            return;
        }

        this.grid.MovePiece(pieceId, positionToMoveTo);
        this.gridChanged = true;
    }

    public void DestroyPiece(int pieceId) {
        this.grid.DestroyPiece(pieceId);
        PlayerManager.instance.RemovePieceForPlayer(pieceId);
        this.gridChanged = true;
    }

    public void DestroyCity(int cityId) {
        this.grid.DestroyCity(cityId);
        PlayerManager.instance.RemoveCityForPlayer(cityId);
        this.gridChanged = true;
    }

    public Piece GetPieceFromId(int pieceId) {

        for (int x = 0; x < this.gridSize.x; x++) {
            for (int y = 0; y < this.gridSize.y; y++) {
                Piece pieceAt = this.grid.GetPieceAt(x, y);
                if (pieceAt != null && pieceAt.id == pieceId) {
                    return pieceAt;
                }
            }
        }

        return null;
    }

    public Vector2Int GetPiecePositionFromId(int pieceId) {

        for (int x = 0; x < this.gridSize.x; x++) {
            for (int y = 0; y < this.gridSize.y; y++) {
                Piece pieceAt = this.grid.GetPieceAt(x, y);
                if (pieceAt != null && pieceAt.id == pieceId) {
                    return new Vector2Int(x, y);
                }
            }
        }

        return new Vector2Int();
    }

    public City GetCityTileFromId(int cityId) {

        for (int x = 0; x < this.gridSize.x; x++) {
            for (int y = 0; y < this.gridSize.y; y++) {
                City cityAt = this.grid.GetCityAt(x, y);
                if (cityAt != null && cityAt.id == cityId) {
                    return cityAt;
                }
            }
        }

        return null;
    }

    public Vector2Int GetCityTilePositionFromId(int cityId) {

        for (int x = 0; x < this.gridSize.x; x++) {
            for (int y = 0; y < this.gridSize.y; y++) {
                City cityAt = this.grid.GetCityAt(x, y);
                if (cityAt != null && cityAt is City && cityAt.id == cityId) {
                    return new Vector2Int(x, y);
                }
            }
        }

        return new Vector2Int();
    }

    public List<Vector2Int> GetNewCityOwnedTiles(Vector2Int cityPosition)
    {
        return this.grid.GetNewCityOwnedTiles(cityPosition);
    }

    //---- End GridManager API functions ----

    void Awake() {
        if (!instance) instance = this;
    }

    private void CreateTiles() {
        //this is all resource tiles for now
        this.grid.PopulateWithResourceTiles(averageResourceCount);
    }

    public void InstantiateResourcePrefab(int x, int y, ResourceTileTypes type) {
        Vector3 pos = new Vector3(this.cellWidth * x + this.cellWidth / 2f, VisualConfig.tileInstantiateHeight, this.cellWidth * y + this.cellWidth / 2f);
        this.instantiatedGridPrefabs[x, y] = Instantiate(resourceTilePrefab, pos, Quaternion.identity, gameObject.transform);
        switch (type)
        {
            case ResourceTileTypes.Forest:
                this.instantiatedForestPrefabs[x, y] = Instantiate(forestTilePrefab, pos, Quaternion.identity, gameObject.transform);
                break;
            default:
                break;
        }
        //SendMessage just calls these methods in the ResourceTile class.
        this.instantiatedGridPrefabs[x, y].transform.GetChild(0).SendMessage("SetGrowth", this.grid.GetTileAt(x,y).growthPerTurn);
        this.instantiatedGridPrefabs[x, y].transform.GetChild(0).SendMessage("SetProduction", this.grid.GetTileAt(x,y).productionPerTurn);
    }

    public void InstantiateCityPrefab(int x, int y, int player, int cityId) {
        GameObject instantiatedCity = Instantiate(cityTilePrefab, new Vector3(this.cellWidth * x + this.cellWidth / 2f, VisualConfig.tileInstantiateHeight, this.cellWidth * y + this.cellWidth / 2f), Quaternion.identity, gameObject.transform);
        instantiatedCity.GetComponent<SelectionAnchor>().objectId = cityId;
        this.instantiatedGridPrefabs[x, y] = instantiatedCity;

        //go through all owned city tiles and instantiate the border prefab.
        for (int x2 = 0; x2 < this.grid.gridSize.x; x2++){
            for (int y2 = 0; y2 < this.grid.gridSize.y; y2++){
                City city = this.grid.GetCityAt(x, y);
                if (city != null && city.ownedTiles[x2, y2]) {
                    GameObject newBorder = Instantiate(borderPrefab, new Vector3(this.cellWidth * x2 + this.cellWidth / 2f, VisualConfig.tileInstantiateHeight, this.cellWidth * y2 + this.cellWidth / 2f), Quaternion.identity, gameObject.transform);
                    newBorder.GetComponent<CityBorder>().AddPlayerColor(player);
                    this.borderPrefabs.Add(newBorder);
                }
            }
        }
    }

    private GameObject GetPiecePrefabFromType(PieceTypes type) {
        switch (type) {
            case PieceTypes.Pawn:
                return pawnPrefab;
            default:
                return null;
        }
    }

    public void InstantiatePiecePrefab(int x, int y, int pieceId, PieceTypes type) {

        GameObject prefab = this.GetPiecePrefabFromType(type);

        if (prefab == null) {
            Debug.LogError($"Cannot find a prefab associated with type {type.ToString()}");
            Debug.Break();
        }

        GameObject instantiatedPiece = Instantiate(prefab, new Vector3(this.cellWidth * x + this.cellWidth / 2f, VisualConfig.tileInstantiateHeight, 
                                                  this.cellWidth * y + this.cellWidth / 2f), Quaternion.identity, gameObject.transform);

        instantiatedPiece.GetComponent<SelectionAnchor>().objectId = pieceId;

        int playerId;

        if ((playerId = PlayerManager.instance.GetPlayerFromPieceId(pieceId)) == -1) {
            Debug.LogError($"Cannot find a player who owns the piece with id {pieceId}.");
            Debug.Break();
        }

        instantiatedPiece.GetComponent<MeshRenderer>().material.SetColor("_Color", 
            GlobalState.instance.playerColors[playerId]);


        this.instantiatedPiecePrefabs[x, y] = instantiatedPiece;

    }

    /**
     * Iniitalizes the grid by creating the grid object, randomly generating the tiles, and instantiating their prefabs.
     */
    private void InitializeGrid() 
    {

        this.borderPrefabs = new ArrayList();
        this.grid = new Grid(this.gridSize, this.cellWidth);

        if (Debug.isDebugBuild) {
            this.gridDebug = gameObject.AddComponent<GridDebug>();
            this.gridDebug.AddGrid(this.grid);
        }

        this.grid.CreateGrid();
        
        CreateTiles();

        this.instantiatedGridPrefabs = new GameObject[this.grid.gridSize.x, this.grid.gridSize.y];
        this.instantiatedForestPrefabs = new GameObject[this.grid.gridSize.x, this.grid.gridSize.y];
        for (int x = 0; x < this.grid.gridSize.x; x++){
            for (int y = 0; y < this.grid.gridSize.y; y++) {
                City city = this.grid.GetCityAt(x, y);
                ResourceTile tile = this.grid.GetTileAt(x, y);
                if (city == null) {
                    InstantiateResourcePrefab(x, y, tile.type);
                    
                }
                else {
                    int playerId = PlayerManager.instance.GetPlayerFromCityId(city.id);
                    if (playerId != -1) {
                        InstantiateCityPrefab(x, y, playerId, city.id);
                    }
                    else {
                        Debug.LogError("Trying to find a player id for a city that is not registered with PlayerManager from GridManager.");
                        Debug.Break();
                    }
                }
            }
        }

        this.instantiatedPiecePrefabs = new GameObject[this.grid.gridSize.x, this.grid.gridSize.y];
    }

    private void UpdatePrefabs() {

        //TODO: Speed this up by not destroying unless the tile has changed (only if performance is a concern).
        for (int x = 0; x < this.grid.gridSize.x; x++){
            for (int y = 0; y < this.grid.gridSize.y; y++) {
                //remove existing tiles
                //First, update tile prefabs
                Destroy(this.instantiatedGridPrefabs[x, y]);
                Destroy(this.instantiatedForestPrefabs[x, y]);
                ResourceTile tile = this.grid.GetTileAt(x, y);
                City city = this.grid.GetCityAt(x, y);
                if (city == null) {
                    InstantiateResourcePrefab(x,y, tile.type);
                }
                else {
                    int playerId = PlayerManager.instance.GetPlayerFromCityId(city.id);
                    if (playerId != -1) {
                        InstantiateCityPrefab(x, y, playerId, city.id);
                    }
                    else {
                        Debug.LogError("Trying to find a player id for a city that is not registered with PlayerManager from GridManager.");
                        Debug.Break();
                    }
                }

                //then, update piece prefabs
                Destroy(this.instantiatedPiecePrefabs[x, y]);
                Piece currentPiece = null;
                if ((currentPiece = this.grid.GetPieceAt(x, y)) != null) {
                    if (currentPiece is Pawn) {
                        InstantiatePiecePrefab(x, y, currentPiece.id, PieceTypes.Pawn);
                    }
                }
            }
        }
    }

    private void Start() {
        InitializeGrid();
    }

    private void Update() {
        //respond to changes in the grid (e.g. AddCity)
        if (this.gridChanged) {
            UpdatePrefabs();
            this.gridChanged = false;
        }
    }
}
