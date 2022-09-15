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

public class UIController : MonoBehaviour, IChangeTurnAction, IGameOverAction,
    IGameStartAction
{

    public UIMenus currentUI = UIMenus.MAIN;

    public Text playerLabelText;

    public Text gameOverScreenText;

    public GameObject mainMenuCanvas;

    public GameObject gameOverMenuCanvas;

    public GameObject citySelectCanvas;

    public GameObject pieceSelectCanvas;


    public void OnChangeTurn(int currentPlayer)
    {
        this.playerLabelText.text = "Player " + currentPlayer;
        this.SetActiveMenu(UIMenus.MAIN);
    }

    public void OnSelectCity(int cityId)
    {
        this.SetActiveMenu(UIMenus.CITYSELECT);
    }

    public void OnSelectPiece(int pieceId)
    {
        this.SetActiveMenu(UIMenus.PIECESELECT);
    }

    public void OnDeselect()
    {
        this.SetActiveMenu(UIMenus.MAIN);
    }


    public void SetActiveMenu(UIMenus newActiveMenu)
    {
       switch (newActiveMenu)
        {
            case UIMenus.MAIN:
                this.mainMenuCanvas.SetActive(true);
                this.citySelectCanvas.SetActive(false);
                this.pieceSelectCanvas.SetActive(false);
                this.gameOverMenuCanvas.SetActive(false);
                break;
            case UIMenus.CITYSELECT:
                this.mainMenuCanvas.SetActive(true);
                this.citySelectCanvas.SetActive(true);
                this.pieceSelectCanvas.SetActive(false);
                this.gameOverMenuCanvas.SetActive(false);
                break;
            case UIMenus.PIECESELECT:
                this.mainMenuCanvas.SetActive(true);
                this.citySelectCanvas.SetActive(false);
                this.pieceSelectCanvas.SetActive(true);
                this.gameOverMenuCanvas.SetActive(false);
                break;
            case UIMenus.GAMEOVER:
                this.mainMenuCanvas.SetActive(false);
                this.citySelectCanvas.SetActive(false);
                this.pieceSelectCanvas.SetActive(false);
                this.gameOverMenuCanvas.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void OnGameStart()
    {
        this.SetActiveMenu(UIMenus.MAIN);
    }

    public void OnGameOver(int winningPlayer)
    {
        this.SetActiveMenu(UIMenus.GAMEOVER);
        gameOverScreenText.text = $"Game Over! Winner: Player {winningPlayer}.";
    }
}
