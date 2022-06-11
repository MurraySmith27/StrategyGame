using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum mouseModes {
    DEFAULT,
    PLACE_CITY,
    PLACE_PAWN,
    CHOOSE_PAWN_DIRECTION
}

public class GlobalState : MonoBehaviour
{

    public int currentPlayer = 0;

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
