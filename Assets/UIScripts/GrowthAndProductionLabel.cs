using UnityEngine;
using UnityEngine.UI;

public class GrowthAndProductionLabel : MonoBehaviour
{   
    private Text text;
    public void Awake() {
        text = gameObject.GetComponent<Text>();
    }

    public void Update() {
        int currentPlayer = GlobalState.instance.currentPlayer;
        text.text = "Growth: " + PlayerManager.instance.getPlayerGrowth(currentPlayer) + " Production: " + PlayerManager.instance.getPlayerProduction(currentPlayer); 
    }
}
