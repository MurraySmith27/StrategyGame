using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCityOnClick : MonoBehaviour
{
    public void createCity(){
        MouseMode.setMouseMode(MouseMode.mouseModes.PLACE_CITY);
    }
}
