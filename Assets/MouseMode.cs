using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMode : MonoBehaviour
{

    public enum mouseModes {
        DEFAULT,
        PLACE_CITY
    }

    public static mouseModes mouseMode = mouseModes.DEFAULT;

    public static void setMouseMode(mouseModes newMode) {
        MouseMode.mouseMode = newMode;
    }

}
