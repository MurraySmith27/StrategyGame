using UnityEngine;
using System.Collections;

public class Grid
{
    public Tile[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }

    public float cellWidth { get; private set; }

    public bool isActive;

    private ArrayList cities;

    public Grid(Vector2Int _gridSize, float _cellWidth) {

        this.gridSize = _gridSize;
        this.cellWidth = _cellWidth;
        this.isActive = false;

        this.cities = new ArrayList();
        this.grid = new Tile[this.gridSize.x, this.gridSize.y];
    }

    //populates the entire grid with random resource tiles
    public void PopulateWithResourceTiles(int averageResourceCount) {
        

        for (int i = 0; i < this.gridSize.x; i++) {
            for (int j = 0; j < this.gridSize.y; j++) {
                int numGrowthPerTurn = Random.Range(0, averageResourceCount + 1);
                int numProductionPerTurn = averageResourceCount - numGrowthPerTurn;

                //add an extra random resource on random chance
                if (Random.Range(0,6) == 5) {
                    if (Random.Range(0, 2) == 0) {
                        numGrowthPerTurn++;
                    }
                    else {
                        numProductionPerTurn++;
                    }
                }

                Debug.Log("node [" + i + ", " + j + "] growth: " + numGrowthPerTurn + " production: " + numProductionPerTurn);
                Vector3 worldPos = new Vector3(this.cellWidth * i + this.cellWidth / 2f, 0, this.cellWidth * j + this.cellWidth / 2f);
                this.grid[i,j] = new ResourceTile(worldPos, new Vector2Int(i,j), numGrowthPerTurn, numProductionPerTurn);
            }
        }
    }

    //adds a city to the board at the specified location. if a city cannot be built on this tile, return false and do not add the city.
    public bool AddCity(Vector2Int position) {

        Debug.Log("Adding city at " + position.x + ", " + position.y);
        Vector3 worldPos = new Vector3(this.cellWidth * position.x + this.cellWidth / 2f, 0, this.cellWidth * position.y + this.cellWidth / 2f);


        //return false if x is by a corner
        if (position.x < 1 || position.y < 1 || position.x > this.gridSize.x - 1 || position.y > this.gridSize.y - 1) {
            return false;
        }

        //iterate through all tiles to make sure they're all resource tiles.
        for (int x = position.x - 1; x < position.x + 2; x++) {
            for (int y = position.y - 1; y < position.y + 2; y++) {
                if (!(this.grid[x, y] is ResourceTile) || this.grid[x, y] is CityTile) {
                    Debug.Log("invalid city placement!");
                    return false;
                }
            }
        }

        //check if we're overlapping borders with a city.
        for (int i = 0; i < this.cities.Count; i++) {
            for (int x = position.x - 1; x < position.x + 2; x++) {
                for (int y = position.y - 1; y < position.y + 2; y++) {
                    if ((this.cities[i] as CityTile).ownedTiles[x, y]) {
                        return false;
                    }
                }
            }
        }

        CityTile city = new CityTile(worldPos, this.gridSize, position);

        this.cities.Add(city);

        this.grid[position.x, position.y] = city;
        return true;
    }

    public void CreateGrid() {
        this.isActive = true;
    }
}
