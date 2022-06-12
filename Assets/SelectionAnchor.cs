using System;
using System.Collections;
using UnityEngine;

public class SelectionAnchor : MonoBehaviour {

    public int objectId = -1;

    public void OnMouseDown() {
        if (objectId == -1) {
            Debug.LogError("Trying to select an object who's id hasn't been registered on the selection anchor!");
        }

        SelectionManager.instance.SelectObject(this.objectId);
        GlobalState.instance.setMouseMode(mouseModes.SELECTING);
    }

}