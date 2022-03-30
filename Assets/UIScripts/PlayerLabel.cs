using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLabel : MonoBehaviour, IChangeTurnAction
{   
    private Text text;
    public void Awake() {
        text = gameObject.GetComponent<Text>();
    }

    public void OnChangeTurn(int currentPlayer) {
        text.text = "Player " + currentPlayer;
    }
}
