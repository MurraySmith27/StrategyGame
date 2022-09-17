using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLines : MonoBehaviour
{
    LineRenderer lr;
    private void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();

        CreateGridLines();
    }

    public void CreateGridLines()
    {
        Vector2Int gridSize = GridManager.instance.gridSize;
        float cellWidth = GridManager.instance.cellWidth;
        Vector3[] positions = new Vector3[gridSize.x * 2 + gridSize.y * 2 + 5];


        for (int i = 0; i < gridSize.x; i++)
        {
            positions[2 * i] = new Vector3(cellWidth * i, VisualConfig.tileInstantiateHeight, i % 2 == 1 ? cellWidth * gridSize.y : 0);
            positions[2 * i + 1] = new Vector3(cellWidth * i, VisualConfig.tileInstantiateHeight, i % 2 == 0 ? cellWidth * gridSize.y : 0);
        }

        for (int j = 0; j < gridSize.y; j++)
        {
            positions[2 * j + gridSize.x * 2] = new Vector3(j % 2 == 1 ? cellWidth * gridSize.x : 0, VisualConfig.tileInstantiateHeight, cellWidth * j);
            positions[2 * j + gridSize.x * 2 + 1] = new Vector3(j % 2 == 0 ? cellWidth * gridSize.x : 0, VisualConfig.tileInstantiateHeight, cellWidth * j);
        }

        positions[2 * gridSize.x + 2 * gridSize.y] = new Vector3(0, VisualConfig.tileInstantiateHeight, 0);
        positions[2 * gridSize.x + 2 * gridSize.y + 1] = new Vector3(cellWidth * gridSize.x, VisualConfig.tileInstantiateHeight, 0);
        positions[2 * gridSize.x + 2 * gridSize.y + 2] = new Vector3(cellWidth * gridSize.x, VisualConfig.tileInstantiateHeight, cellWidth * gridSize.y);
        positions[2 * gridSize.x + 2 * gridSize.y + 3] = new Vector3(0, VisualConfig.tileInstantiateHeight, cellWidth * gridSize.y);
        positions[2 * gridSize.x + 2 * gridSize.y + 4] = new Vector3(0, VisualConfig.tileInstantiateHeight, 0);
        lr.positionCount = gridSize.x * 2 + gridSize.y * 2 + 5;
        lr.SetPositions(positions);
    }
}
