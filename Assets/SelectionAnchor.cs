using System;
using System.Collections;
using UnityEngine;

public class SelectionAnchor : MonoBehaviour {

    public int objectId = -1;

    public void Update() {
        if (MouseEventUtils.IsClicked(
            Camera.main, 
            gameObject.GetComponent<Collider>(),
            LayerMask.GetMask(new string[]{"City", "Piece"})
            ) && ShouldProcessMouseEvent()) OnClick();
    }

    public void OnClick() {

        if (objectId == -1) {
            Debug.LogError("Trying to select an object who's id hasn't been registered on the selection anchor!");
        }

        SelectionManager.instance.SelectObject(this.objectId);
        GlobalState.instance.setMouseMode(mouseModes.SELECTING);
    }

    public bool ShouldProcessMouseEvent() {

        string[] validMouseModes = {
            mouseModes.DEFAULT.ToString()
        };

        for (int i = 0; i < validMouseModes.Length; i++) {
            if (GlobalState.instance.mouseMode.ToString() == validMouseModes[i]) {
                return true;
            }
        }
        return false;
    }

}