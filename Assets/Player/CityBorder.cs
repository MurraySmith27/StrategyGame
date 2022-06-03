using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBorder : MonoBehaviour
{

    public Material borderMaterial = null;
    // Start is called before the first frame update
    public void AddPlayerColor(int playerNum)
    {
        //create a new material with the specified color and apply it to all child objects.
        if (borderMaterial) {
            Material newMaterial = new Material(borderMaterial);
            //this relies on the fact that all cities created during a players turn belong to that player.
            newMaterial.color = GlobalState.instance.playerColors[playerNum];
            foreach(Transform childTransform in gameObject.transform) {
                childTransform.gameObject.GetComponent<MeshRenderer>().material = newMaterial;
            }
        }
        else {
            Debug.LogError("No material attached to BorderInit script! The city border will have no material.");
        }
    }
}
