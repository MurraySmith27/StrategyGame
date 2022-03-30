using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateItemOnClick : MonoBehaviour
{   
    //this gets called by the UI button object (see Canvas -> CreateCityButton)
    public void createCity(){
        GlobalState.instance.setMouseMode(mouseModes.PLACE_CITY);
    }


    public void createPawn() {
        GlobalState.instance.setMouseMode(mouseModes.PLACE_PAWN);
    }
}
