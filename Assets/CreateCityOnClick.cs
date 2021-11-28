using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCityOnClick : MonoBehaviour
{
    public void createCity(){
        FlowField.setMouseMode(FlowField.mouseModes.PLACE_CITY);
    }
}
