using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IRoundUpdateAction
{

    public int numStartingPlayers;

    //each arraylist entry stores the id of all the cities that belong to the player with key == id.
    private Dictionary<int, List<int>> playerToCities;

    private Dictionary<int, List<int>> playerToPieces;

    private Dictionary<int, List<int>> playerToPiecesMoved;

    //stores the current growth of each player.
    private Dictionary<int, int> playerToGrowth;

    //stores the current production of each player.
    private Dictionary<int, int> playerToProduction;

    public static PlayerManager instance;

    private int numPlayers = 0;

    //private Dictionary<int, City> cities;

    void Awake() {
        if (!instance)
            instance = this;  
        
        playerToCities = new Dictionary<int, List<int>>();
        playerToPieces = new Dictionary<int, List<int>>();
        playerToPiecesMoved = new Dictionary<int, List<int>>();
        playerToGrowth = new Dictionary<int, int>();
        playerToProduction = new Dictionary<int, int>();

        for (int i = 0; i < numStartingPlayers; i++) {
            AddPlayer();
        }
    }


    public void AddPlayer() {
        playerToCities[numPlayers] = new List<int>();
        playerToPieces[numPlayers] = new List<int>();
        playerToPiecesMoved[numPlayers] = new List<int>();
        playerToGrowth[numPlayers] = 0;
        playerToProduction[numPlayers] = 0;
        numPlayers++;
    }


    public bool CanPlaceCity(int playerIndex) {
        return playerToProduction[playerIndex] >= GameConstants.cityCost;
    }

    public bool CanPlacePawn(int playerIndex) {
        return playerToProduction[playerIndex] >= GameConstants.pawnCost;
    }

    public int GetPlayerFromCityId(int cityId) {
        for (int playerId = 0; playerId < numPlayers; playerId++) {
            if (playerToCities[playerId].IndexOf(cityId) != -1) {
                return playerId;
            }
        }
        return -1;
    }

    public bool PlayerCanMovePiece(int playerId, int pieceId) {
        return playerToPiecesMoved[playerId].IndexOf(pieceId) == -1;
    }

    public void AddCityForPlayer(int playerIndex, Vector2Int position) {

        Action addCity = () => {
            int newCityId = GridManager.instance.AddCity(position);
            if (newCityId != -1) {
                playerToCities[playerIndex].Add(newCityId);
            }
            else {
                Debug.LogError("Tried to place a city in an invalid place, but grid flagged it as valid! player: " + playerIndex + " position: " + position);
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
            else {
                PopupMessage.CreatePopupMessage(
                    "Nope!", 
                    $"Not enough production to place a city! A city requires {GameConstants.cityCost} production."
                );
            }
        }
    }

    public void RemoveCityForPlayer(int cityId) {

        foreach (KeyValuePair<int, List<int>> keyValuePair in this.playerToCities) {
            if (keyValuePair.Value.IndexOf(cityId) != -1) {
                keyValuePair.Value.Remove(cityId);
            }
        }
    }

    public int GetPlayerFromPieceId(int pieceId) {
        foreach (KeyValuePair<int, List<int>> keyValuePair in this.playerToPieces) {
            if (keyValuePair.Value.IndexOf(pieceId) != -1) {
                return keyValuePair.Key;
            }
        }

        return -1;
    }

    public void AddPieceForPlayer(int playerIndex, Vector2Int position, PieceTypes type, int pawnDirection = -1) {

        if (GridManager.instance.CanPlacePieceAt(position, type)) {
            if (this.CanPlacePawn(playerIndex)) {
                playerToProduction[playerIndex] -= GameConstants.pawnCost;
                
                int newPieceId = GridManager.instance.AddPiece(position, type, pawnDirection: pawnDirection);

                if (newPieceId != -1) {
                    playerToPieces[playerIndex].Add(newPieceId);
                }
                else {
                    Debug.LogError("Tried to place a piece in an invalid place, but grid flagged it as valid! player: " + playerIndex + " position: " + position + " piece type: " + type);
                    Debug.Break();
                }
            }
            else {
                PopupMessage.CreatePopupMessage(
                    "Nope!", 
                    $"Not enough production to place a {type.ToString()}! A {type.ToString()} requires {GameConstants.getCostFromPiece(type)} production."
                );
            }
        }
        else {
            PopupMessage.CreatePopupMessage(
                "Nope!", 
                $"Invalid location for a {type.ToString()}!"
            );
        }

    }

    public void RemovePieceForPlayer(int pieceId) {
        foreach (KeyValuePair<int, List<int>> keyValuePair in this.playerToPieces) {
            if (keyValuePair.Value.IndexOf(pieceId) != -1) {
                keyValuePair.Value.Remove(pieceId);
            }
        }
    }

    public void MarkPieceAsMoved(int pieceId) {
        int player = this.GetPlayerFromPieceId(pieceId);

        this.playerToPiecesMoved[player].Add(pieceId);
    }


    public void OnRoundStart() {
        for (int playerId = 0; playerId < numPlayers; playerId++) {
            int growthIncrement, prodIncrement;
            foreach(int cityId in this.playerToCities[playerId]){
                (growthIncrement, prodIncrement) = GridManager.instance.GetCityGrowthAndProd(cityId);
                this.playerToGrowth[playerId] += growthIncrement;
                this.playerToProduction[playerId] += prodIncrement;
            }

            this.playerToPiecesMoved[playerId].Clear();
        }
    }

    public int getPlayerGrowth(int playerIndex) {
        return this.playerToGrowth[playerIndex];
    }
    public int getPlayerProduction(int playerIndex) {
        return this.playerToProduction[playerIndex];
    }

    public int getNumPlayers() {
        return this.numPlayers;
    }
}
