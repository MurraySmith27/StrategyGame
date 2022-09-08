using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum UIMenus
{
    MAIN,
    GAMEOVER,
    CITYSELECT,
    PIECESELECT
}

public class UIController : MonoBehaviour, IChangeTurnAction, IGameOverAction
{

    public UIMenus currentUI = UIMenus.MAIN;

    public Dictionary<UIMenus, GameObject> menus;

    public Text playerLabelText;

    public Text gameOverScreenText;

    public GameObject mainMenuCanvas;

    public GameObject gameOverMenuCanvas;

    public void Awake()
    {
        this.menus = new Dictionary<UIMenus, GameObject>();

        this.menus[UIMenus.MAIN] = mainMenuCanvas;

        this.menus[UIMenus.GAMEOVER] = gameOverMenuCanvas;

        //TODO: Add other menus to dict.
    }

    public void OnChangeTurn(int currentPlayer)
    {
        playerLabelText.text = "Player " + currentPlayer;
    }


    public void SetActiveMenu(UIMenus newActiveMenu)
    {
        this.menus[this.currentUI].SetActive(false);

        this.currentUI = newActiveMenu;

        this.menus[newActiveMenu].SetActive(true);
    }


    public void OnGameOver(int winningPlayer)
    {

        this.SetActiveMenu(UIMenus.GAMEOVER);
        gameOverScreenText.text = $"Game Over! Winner: Player {winningPlayer}.";
    }
}
