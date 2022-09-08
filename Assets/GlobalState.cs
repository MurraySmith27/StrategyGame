using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum mouseModes {
    DEFAULT,
    PLACE_CITY,
    PLACE_PAWN,
    CHOOSE_PAWN_DIRECTION,
    SELECTING
}

public class GlobalState : MonoBehaviour
{

    public int currentPlayer = 0;

    public int roundNumber = 0;

    public mouseModes mouseMode = mouseModes.DEFAULT;

    public static GlobalState instance;

    public Color[] playerColors = new Color[1];

    public Vector2 pawnToPlacePosition = new Vector2(-1f, -1f);

    public void Awake() {
        if (!instance)
            instance = this;

        mouseMode = mouseModes.DEFAULT;

    }

    public void Start() {
        BroadcastMessage("OnRoundStart");
        BroadcastMessage("OnChangeTurn", currentPlayer);
    }

    public void EndGame(int winningPlayer)
    {
        BroadcastMessage("OnGameOver", winningPlayer);
    }

    private bool CanChangeTurn(out string errorMessage)
    {

        errorMessage = "";

        if (roundNumber == 0 &&
            PlayerManager.instance.GetNumCitiesForPlayer(this.currentPlayer) == 0)
        {
            errorMessage = "Place a city before you end your first turn!";
            return false;
        }

        return true;
    }

    public void changeTurn() {
        string errorMessage;
        if (CanChangeTurn(out errorMessage))
        {


            if (currentPlayer == PlayerManager.instance.getNumPlayers() - 1)
            {
                setCurrentPlayer(0);
                BroadcastMessage("OnRoundStart");
            }
            else
            {
                setCurrentPlayer(currentPlayer + 1);
            }
            BroadcastMessage("OnChangeTurn", currentPlayer);
        }
        else
        {
            PopupMessage.CreatePopupMessage("Nope!", errorMessage);
        }
    }

    public void Update() {
        if (Input.GetMouseButtonDown(1)) {
            GlobalState.instance.setMouseMode(mouseModes.DEFAULT);
        }
    }

    public void setCurrentPlayer(int newPlayer) {
        currentPlayer = newPlayer;
    }

    public void setMouseMode(mouseModes newMode) {
        mouseMode = newMode;
    }

}
