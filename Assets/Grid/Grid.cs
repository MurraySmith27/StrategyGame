using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid
{

    private int idGenerator;

    private int numPieces;

    public ResourceTile[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }

    public float cellWidth { get; private set; }

    public bool isActive;

    //key: city id value: CityTile Object
    private Dictionary<int, City> cities;

    private Dictionary<int, Piece> pieces;

    public Grid(Vector2Int _gridSize, float _cellWidth) {

        this.gridSize = _gridSize;
        this.cellWidth = _cellWidth;
        this.isActive = false;

        this.cities = new Dictionary<int, City>();
        this.pieces = new Dictionary<int, Piece>();
        this.grid = new ResourceTile[this.gridSize.x, this.gridSize.y];
    }


    //get the total growth and production of all owned tiles for city with cityId 
    //  return: (growth, production)
    public (int, int) GetCityGrowthAndProd(int cityId) {
        City city;
        
        if (this.cities.ContainsKey(cityId) && (city = this.cities[cityId]) != null) {
            int totalGrowth = 0;
            int totalProd = 0;
            for (int x = 0; x < this.gridSize.x; x++) {
                for (int y = 0; y < this.gridSize.y; y++) {
                    if (city.ownedTiles[x,y]) {
                        ResourceTile tile = this.GetTileAt(x, y);
                        totalGrowth += tile.growthPerTurn;
                        totalProd += tile.productionPerTurn;
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
                int numGrowthPerTurn = 1;
                int numProductionPerTurn = 1;
                ResourceTileTypes type = ResourceTileTypes.Normal;

                //add an extra random resource on random chance
                if (Random.Range(0, GameConstants.forestProbabilty) == 0) {
                    numGrowthPerTurn++;
                    type = ResourceTileTypes.Forest;
                }
                this.grid[i,j] = new ResourceTile(new Vector2Int(i,j), numGrowthPerTurn, numProductionPerTurn, type);
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
                if (this.GetCityAt(x, y) != null) {
                    return false;
                }
            }
        }

        //check if we're overlapping borders with a city.
        foreach (KeyValuePair<int, City> keyValuePair in this.cities) {
            for (int x = position.x - 1; x < position.x + 2; x++) {
                for (int y = position.y - 1; y < position.y + 2; y++) {
                    if (keyValuePair.Value.ownedTiles[x, y]) {
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

        return this.GetCityAt(position.x, position.y) == null && 
                this.GetPieceAt(position.x, position.y) == null;
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
        }
        return -1;
    }

    public void DestroyPiece(int pieceId) {
        this.pieces.Remove(pieceId);
    }

    public void MovePiece(int pieceId, Vector2Int positionToMoveTo) {
        if (this.pieces.ContainsKey(pieceId)) {
            this.pieces[pieceId].gridIndex = positionToMoveTo;
        }
    }

    public ResourceTile GetTileAt(int x, int y) {
        if (x < 0 || this.gridSize.x <= x || y < 0 || this.gridSize.y <= y) {
            return null;
        }
        else {
            return this.grid[x, y];
        }
    }

    public City GetCityAt(int x, int y) {
        foreach (KeyValuePair<int, City> keyValuePair in this.cities) {
            if (keyValuePair.Value.position.x == x && keyValuePair.Value.position.y == y) {
                return keyValuePair.Value;
            }
        }
        return null;
    }

    public Piece GetPieceAt(int x, int y) {
        foreach (KeyValuePair<int, Piece> keyValuePair in this.pieces) {
            if (keyValuePair.Value.gridIndex.x == x && keyValuePair.Value.gridIndex.y == y) {
                return keyValuePair.Value;
            }
        }
        return null;
    }

    public List<Vector2Int> GetNewCityOwnedTiles(Vector2Int cityPos)
    {
        List<Vector2Int> borderPositions = new List<Vector2Int>(4);
        borderPositions.Add(new Vector2Int(cityPos.x - 1, cityPos.y));
        borderPositions.Add(new Vector2Int(cityPos.x + 1, cityPos.y));
        borderPositions.Add(new Vector2Int(cityPos.x, cityPos.y - 1));
        borderPositions.Add(new Vector2Int(cityPos.x, cityPos.y + 1));

        return borderPositions;
    }

    //adds a city to the board at the specified location. if a city cannot be built on this tile, return false and do not add the city.
    public int AddCity(Vector2Int position) {

        int newCityId = this.idGenerator++;
        City city = new City(this.gridSize, position, newCityId);

        this.cities[newCityId] = city;

        foreach (Vector2Int borderPos in this.GetNewCityOwnedTiles(position))
        {
            city.ownedTiles[borderPos.x, borderPos.y] = true;
        }
        
        return newCityId;
    }

    public void DestroyCity(int cityId) {
        this.cities.Remove(cityId);
    }

    public void CreateGrid() {
        this.isActive = true;
    }
}
