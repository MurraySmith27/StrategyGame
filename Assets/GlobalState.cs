using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum mouseModes {
    DEFAULT,
    PLACE_CITY,
    PLACE_PAWN
}


public class GlobalState : MonoBehaviour
{

    public int currentPlayer = 0;

    public mouseModes mouseMode = mouseModes.DEFAULT;

    public static GlobalState instance;


    public void Awake() {
        if (!instance)
            instance = this;

        mouseMode = mouseModes.DEFAULT;
        currentPlayer = currentPlayer;
    }

    public void Start() {
        BroadcastMessage("OnRoundStart");
        BroadcastMessage("OnChangeTurn", currentPlayer);
    }

    public void changeTurn() {
        if (currentPlayer == PlayerManager.instance.getNumPlayers() - 1) {
            setCurrentPlayer(0);
            BroadcastMessage("OnRoundStart");
        }
        else {
            setCurrentPlayer(currentPlayer + 1);
        }
        BroadcastMessage("OnChangeTurn", currentPlayer);
    }

    public void setCurrentPlayer(int newPlayer) {
        currentPlayer = newPlayer;
    }

    public void setMouseMode(mouseModes newMode) {
        mouseMode = newMode;
    }

}
