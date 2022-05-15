using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PieceTypes {
    Pawn,
}

public class GridManager : MonoBehaviour
{
    
    public static GridManager instance;

    public Vector2Int gridSize;
    public float cellWidth = 10f;

    public Grid grid;

    public GameObject resourceTilePrefab;

    public GameObject cityTilePrefab;

    public GameObject borderPrefab;

    public GameObject pawnPrefab;

    private GridDebug gridDebug;

    public int averageResourceCount = 2;

    private ArrayList borderPrefabs;

    private bool gridChanged = false;

    private GameObject[,] instantiatedGridPrefabs;

    private GameObject[,] instantiatedPiecePrefabs;


    //---- Begin GridManager API functions ----

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
    public int AddPiece(Vector2Int position, PieceTypes type) {
        int newPieceId = this.grid.AddPiece(position, type);
        
        //trigger an update if successful.
        this.gridChanged = this.gridChanged || newPieceId != -1;

        return newPieceId;
    }

    //---- End GridManager API functions ----

    void Awake() {
        if (!instance) instance = this;
    }

    private void CreateTiles() {
        //this is all resource tiles for now
        this.grid.PopulateWithResourceTiles(averageResourceCount);
    }

    public void InstantiateResourcePrefab(int x, int y) {
        this.instantiatedGridPrefabs[x, y] = Instantiate(resourceTilePrefab, new Vector3(this.cellWidth * x + this.cellWidth / 2f, 0, this.cellWidth * y + this.cellWidth / 2f), Quaternion.identity, gameObject.transform);
        //SendMessage just calls these methods in the ResourceTile class.
        this.instantiatedGridPrefabs[x, y].transform.GetChild(0).SendMessage("SetGrowth", (this.grid.GetTileAt(x,y) as ResourceTile).growthPerTurn);
        this.instantiatedGridPrefabs[x, y].transform.GetChild(0).SendMessage("SetProduction", (this.grid.GetTileAt(x,y) as ResourceTile).productionPerTurn);
    }
    public void InstantiateCityPrefab(int x, int y, int player) {
        this.instantiatedGridPrefabs[x, y] = Instantiate(cityTilePrefab, new Vector3(this.cellWidth * x + this.cellWidth / 2f, 0, this.cellWidth * y + this.cellWidth / 2f), Quaternion.identity, gameObject.transform);
        //go through all owned city tiles and instantiate the border prefab.
        for (int x2 = 0; x2 < this.grid.gridSize.x; x2++){
            for (int y2 = 0; y2 < this.grid.gridSize.y; y2++){
                if (this.grid.GetTileAt(x,y) is CityTile && (this.grid.GetTileAt(x, y) as CityTile).ownedTiles[x2, y2]) {
                    GameObject newBorder = Instantiate(borderPrefab, new Vector3(this.cellWidth * x2 + this.cellWidth / 2f, 0, this.cellWidth * y2 + this.cellWidth / 2f), Quaternion.identity, gameObject.transform);
                    newBorder.GetComponent<CityBorder>().AddPlayerColor(player);
                    this.borderPrefabs.Add(newBorder);
                }
            }
        }
    }

    public void InstantiatePawnPrefab(int x, int y) {
        this.instantiatedPiecePrefabs[x, y] = Instantiate(pawnPrefab, new Vector3(this.cellWidth * x + this.cellWidth / 2f, 0, 
                                                          this.cellWidth * y + this.cellWidth / 2f), Quaternion.identity, gameObject.transform);
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
        for (int x = 0; x < this.grid.gridSize.x; x++){
            for (int y = 0; y < this.grid.gridSize.y; y++) {
                if (this.grid.GetTileAt(x, y) is ResourceTile) {
                    InstantiateResourcePrefab(x, y);
                    
                }
                else if (this.grid.GetTileAt(x, y) is CityTile) {
                    int playerId = PlayerManager.instance.getPlayerFromCityId(((CityTile)this.grid.GetTileAt(x, y)).id);
                    if (playerId != -1) {
                        InstantiateCityPrefab(x, y, playerId);
                    }
                    else {
                        Debug.LogError("Trying to find a player id for a city that is not registered with PlayerManager from GridManager.");
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
                if (this.grid.GetTileAt(x, y) is ResourceTile) {
                    InstantiateResourcePrefab(x,y);
                }
                else if (this.grid.GetTileAt(x, y) is CityTile) {
                    int playerId = PlayerManager.instance.getPlayerFromCityId(((CityTile)this.grid.GetTileAt(x, y)).id);
                    if (playerId != -1) {
                        InstantiateCityPrefab(x, y, playerId);
                    }
                    else {
                        Debug.LogError("Trying to find a player id for a city that is not registered with PlayerManager from GridManager.");
                    }
                }

                //then, update piece prefabs
                Destroy(this.instantiatedPiecePrefabs[x, y]);
                Piece currentPiece = null;
                if ((currentPiece = this.grid.getPieceAt(x, y)) != null) {
                    if (currentPiece is Pawn) {
                        InstantiatePawnPrefab(x, y);
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
