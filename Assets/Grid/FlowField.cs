using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField : MonoBehaviour
{

    public enum mouseModes {
        DEFAULT,
        PLACE_CITY
    }
    
    public static FlowField instance;

    public Vector2Int gridSize;
    public float cellWidth = 1f;

    public Grid grid;

    public GameObject resourceTilePrefab;

    public GameObject cityTilePrefab;

    public GameObject borderPrefab;
    private GridDebug gridDebug;

    public int averageResourceCount = 2;

    private ArrayList borderPrefabs;
    public static mouseModes mouseMode = mouseModes.DEFAULT;

    private bool gridChanged = false;

    private GameObject[,] instantiatedGridPrefabs;


    void Start() {
        if (!instance)
            instance = this;

    }


    public static void setMouseMode(mouseModes newMode) {
        FlowField.mouseMode = newMode;
    }

    private void CreateTiles() {
        //this is all resource tiles for now
        this.grid.PopulateWithResourceTiles(averageResourceCount);
    }


    public void AddCity(Vector2Int position) {
        this.gridChanged = this.grid.AddCity(position);
    }



    public void InstantiateResourcePrefabs(int x, int y) {
        this.instantiatedGridPrefabs[x, y] = Instantiate(resourceTilePrefab, new Vector3(x, 0, y), Quaternion.identity, gameObject.transform);
        this.instantiatedGridPrefabs[x, y].transform.GetChild(0).SendMessage("SetGrowth", (this.grid.grid[x,y] as ResourceTile).growthPerTurn);
        this.instantiatedGridPrefabs[x, y].transform.GetChild(0).SendMessage("SetProduction", (this.grid.grid[x,y] as ResourceTile).productionPerTurn);
    }
    public void InstantiateCityPrefabs(int x, int y) {
        this.instantiatedGridPrefabs[x, y] = Instantiate(cityTilePrefab, new Vector3(x, 0, y), Quaternion.identity, gameObject.transform);
        //go through all owned city tiles and instantiate the border prefab.
        Debug.Log("owned tiles" + (this.grid.grid[x, y] as CityTile).ownedTiles);
        for (int x2 = 0; x2 < this.grid.gridSize.x; x2++){
            for (int y2 = 0; y2 < this.grid.gridSize.y; y2++){
                if ((this.grid.grid[x, y] as CityTile).ownedTiles[x2, y2]) {
                    Debug.Log("found an owned tile at " + x2 + "," + y2);
                    this.borderPrefabs.Add(Instantiate(borderPrefab, new Vector3(x2, 0, y2), Quaternion.identity, gameObject.transform));
                }
            }
        }
    }

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
                if (this.grid.grid[x, y] is ResourceTile) {
                    InstantiateResourcePrefabs(x,y);
                    
                }
                else if (this.grid.grid[x, y] is CityTile) {
                    InstantiateCityPrefabs(x,y);
                }
            }
        }
    }

    private void UpdatePrefabs() {

        Debug.Log("updating prefabs");
        for (int x = 0; x < this.grid.gridSize.x; x++){
            for (int y = 0; y < this.grid.gridSize.y; y++) {
                //remove existing tiles
                Destroy(this.instantiatedGridPrefabs[x, y]);
                if (this.grid.grid[x, y] is ResourceTile) {
                    InstantiateResourcePrefabs(x,y);
                }
                else if (this.grid.grid[x, y] is CityTile) {
                    InstantiateCityPrefabs(x, y);
                }
            }
        }
    }

    private void Awake() {
        InitializeGrid();
    }

    private void Update() {
        if (this.gridChanged) {
            UpdatePrefabs();
            this.gridChanged = false;
        }
    }
}
