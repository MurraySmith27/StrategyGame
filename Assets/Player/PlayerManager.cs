using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour 
{

    public int numStartingPlayers;

    //each arraylist entry stores the id of all the cities that belong to the player with key == id.
    private Dictionary<int, ArrayList> playerToCities;

    private Dictionary<int, ArrayList> playerToPieces;

    //stores the current growth of each player.
    private Dictionary<int, int> playerToGrowth;

    //stores the current production of each player.
    private Dictionary<int, int> playerToProduction;

    public static PlayerManager instance;

    private int numPlayers = 0;

    //private Dictionary<int, City> cities;

    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
            instance = this;        
        playerToCities = new Dictionary<int, ArrayList>();
        playerToPieces = new Dictionary<int, ArrayList>();
        playerToGrowth = new Dictionary<int, int>();
        playerToProduction = new Dictionary<int, int>();

        for (int i = 0; i < numStartingPlayers; i++) {
            AddPlayer();
        }
    }


    public void AddPlayer() {
        playerToCities[numPlayers] = new ArrayList();
        playerToPieces[numPlayers] = new ArrayList();
        playerToGrowth[numPlayers] = 0;
        playerToProduction[numPlayers] = 0;
        numPlayers++;
    }


    public bool CanPlaceCity(int playerIndex) {
        return playerToProduction[playerIndex] > GameConstants.cityCost;
    }

    public bool CanPlacePiece(int playerIndex) {
        return playerToProduction[playerIndex] > GameConstants.pawnCost;
    }

    public void AddCityForPlayer(int playerIndex, Vector2Int position) {

        Action addCity = () => {
            int newCityId = GridManager.instance.AddCity(position);
            if (newCityId != -1) {
                playerToCities[playerIndex].Add(newCityId);
            }
            else {
                Debug.LogError("Tried to place a city in an invalid place! player: " + playerIndex + " position: " + position);
                Debug.Break();
            }
        };

        //first city is free.
        if (GridManager.instance.CanPlaceCityAt(position)) {

            if (playerToCities[playerIndex].Count == 0){
                addCity();
            }
            else if (this.CanPlaceCity(playerIndex)) {

                //deduct the cost
                playerToProduction[playerIndex] -= GameConstants.cityCost;
                addCity();
            }

        }
    }

    public void AddPieceForPlayer(int playerIndex, Vector2Int position, PieceTypes type) {
        
        if (this.CanPlacePiece(playerIndex) && GridManager.instance.CanPlacePieceAt(position, type)) {

            playerToProduction[playerIndex] -= GameConstants.pawnCost;

            int newPieceId = GridManager.instance.AddPiece(position, type);
            if (newPieceId != -1) {
                playerToPieces[playerIndex].Add(newPieceId);
            }
            else {
                Debug.LogError("Tried to place a piece in an invalid place! player: " + playerIndex + " position: " + position + " piece type: " + type);
                Debug.Break();
            }
        }
    }


    public void OnRoundStart() {
        for (int playerId = 0; playerId < numPlayers; playerId++) {
            int growthIncrement, prodIncrement;
            foreach(int cityId in playerToCities[playerId]){
                (growthIncrement, prodIncrement) = GridManager.instance.GetCityGrowthAndProd(playerId);
                playerToGrowth[playerId] += growthIncrement;
                playerToProduction[playerId] += prodIncrement;
            }
        }
    }

    public int getNumPlayers() {
        return this.numPlayers;
    }
}
