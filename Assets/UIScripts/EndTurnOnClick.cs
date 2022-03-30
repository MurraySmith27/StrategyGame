using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnOnClick : MonoBehaviour
{   
    public void EndTurn() {
        GlobalState.instance.changeTurn();
    }
}
