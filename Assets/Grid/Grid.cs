using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid
{

    private int idGenerator;

    private int numPieces;

    public Tile[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }

    public float cellWidth { get; private set; }

    public bool isActive;

    //key: city id value: CityTile Object
    private Dictionary<int, CityTile> cities;

    private Dictionary<int, Piece> pieces;

    public Grid(Vector2Int _gridSize, float _cellWidth) {

        this.gridSize = _gridSize;
        this.cellWidth = _cellWidth;
        this.isActive = false;

        this.cities = new Dictionary<int, CityTile>();
        this.pieces = new Dictionary<int, Piece>();
        this.grid = new Tile[this.gridSize.x, this.gridSize.y];
    }


    //get the total growth and production of all owned tiles for city with cityId 
    //  return: (growth, production)
    public (int, int) GetCityGrowthAndProd(int cityId) {
        CityTile city;
        
        if (this.cities.ContainsKey(cityId) && (city = this.cities[cityId]) != null) {
            int totalGrowth = 0;
            int totalProd = 0;
            for (int x = 0; x < this.gridSize.x; x++) {
                for (int y = 0; y < this.gridSize.y; y++) {
                    if (city.ownedTiles[x,y] && this.GetTileAt(x, y) is ResourceTile) {
                        totalGrowth += (this.GetTileAt(x, y) as ResourceTile).growthPerTurn;
                        totalProd += (this.GetTileAt(x, y) as ResourceTile).productionPerTurn;
                    }
                }   
            }
            return (totalGrowth, totalProd);
        }
        else {
            return (-1, -1);
        }
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
                this.grid[i,j] = new ResourceTile(new Vector2Int(i,j), numGrowthPerTurn, numProductionPerTurn);
            }
        }
    }

    public bool CanPlaceCity(Vector2Int position) {

        //return false if x is by a corner
        if (position.x < 1 || position.y < 1 || position.x >= this.gridSize.x - 1 || position.y >= this.gridSize.y - 1) {
            return false;
        }

        //iterate through all tiles to make sure they're all resource tiles.
        for (int x = position.x - 1; x < position.x + 2; x++) {
            for (int y = position.y - 1; y < position.y + 2; y++) {
                if (!(this.grid[x, y] is ResourceTile) || this.grid[x, y] is CityTile) {
                    return false;
                }
            }
        }

        //check if we're overlapping borders with a city.
        foreach (KeyValuePair<int, CityTile> keyValuePair in this.cities) {
            for (int x = position.x - 1; x < position.x + 2; x++) {
                for (int y = position.y - 1; y < position.y + 2; y++) {
                    if ((keyValuePair.Value as CityTile).ownedTiles[x, y]) {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public bool CanPlacePieceAt(Vector2Int position, PieceTypes type) {

        //return false if x is by a corner
        if (position.x < 1 || position.y < 1 || position.x >= this.gridSize.x - 1 || position.y >= this.gridSize.y - 1) {
            return false;
        }

        Piece piece = GetPieceAt(position.x, position.y);
        return this.grid[position.x, position.y] is ResourceTile && piece == null;
    }

    public int AddPiece(Vector2Int position, PieceTypes type, int pawnDirection = -1) {
        int newPieceId = this.idGenerator++;

        this.numPieces++;
        
        Piece newPiece = PieceFactory.create(type, position, newPieceId, pawnDirection: pawnDirection);;
        
        if (newPiece != null) {
            this.pieces[newPieceId] = newPiece;
            return newPieceId;
        }
        else {
            Debug.LogError("Tried to instantiate a default Piece from Grid.cs for some reason. Give it a type first.");
            Debug.Break();
            return -1;
        }
    }

    public void DestroyPiece(int pieceId) {
        this.pieces.Remove(pieceId);
    }

    public void MovePiece(int pieceId, Vector2Int positionToMoveTo) {
        if (this.pieces.ContainsKey(pieceId)) {
            this.pieces[pieceId].gridIndex = positionToMoveTo;
        }
    }

    public Tile GetTileAt(int x, int y) {
        if (x < 0 || this.gridSize.x <= x || y < 0 || this.gridSize.y <= y) {
            return null;
        }
        else {
            return this.grid[x, y];
        }
    }

    public Piece GetPieceAt(int x, int y) {
        foreach (KeyValuePair<int, Piece> keyValuePair in this.pieces) {
            if (keyValuePair.Value.gridIndex.x == x && keyValuePair.Value.gridIndex.y == y) {
                return keyValuePair.Value;
            }
        }
        return null;
    }

    //adds a city to the board at the specified location. if a city cannot be built on this tile, return false and do not add the city.
    public int AddCity(Vector2Int position) {

        int newCityId = this.idGenerator++;
        CityTile city = new CityTile(this.gridSize, position, newCityId);

        this.cities[newCityId] = city;

        this.grid[position.x, position.y] = city;
        return newCityId;
    }

    public void DestroyCity(int cityId) {
        this.cities.Remove(cityId);
    }

    public void CreateGrid() {
        this.isActive = true;
    }
}
