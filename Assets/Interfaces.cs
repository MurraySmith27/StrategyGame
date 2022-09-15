using UnityEngine;

public interface IRoundUpdateAction {

    public void OnRoundStart();
}

public interface IChangeTurnAction {

    public void OnChangeTurn(int currentPlayer);
}

public interface IGameStartAction
{
    public void OnGameStart();
}

public interface IGameOverAction
{
    public void OnGameOver(int winningPlayer);
}

public static class MouseEventUtils {

    public static bool IsMouseOver(Camera cam, Collider collider, LayerMask mask) {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
            if (collider == hit.collider) {
                return true;
            }
        }
        return false;
    }

    public static bool IsClicked(Camera cam, Collider collider, LayerMask mask) {
        return Input.GetMouseButtonDown(0) && IsMouseOver(cam, collider, mask);
    }
}

interface IClickable {
    public bool ShouldProcessMouseEvent();
}
