using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLineController : MonoBehaviour
{
    private LineRenderer lr;

    private void Awake()
    {
        lr = gameObject.GetComponent<LineRenderer>();
    }

    public void SetGrid(Vector3 startPos, float tileSize, Vector2 gridSize)
    {

    }
}
