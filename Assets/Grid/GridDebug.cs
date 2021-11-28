using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDebug : MonoBehaviour
{

    public Grid grid;

    public Mesh arrowMesh; 

    public void AddGrid(Grid _grid) {
        this.grid = _grid;
    }

    // Update is called once per frame
    void OnDrawGizmos()
    {
        //draws a wire cube around each grid cell on the grid. 

        if (grid.isActive) {
            //draw green
            Gizmos.color = Color.green;
        }
        else {
            //draw yellow
            Gizmos.color = Color.yellow;
        }

        Vector3 wireBoxSize = new Vector3(this.grid.cellWidth, this.grid.cellWidth, this.grid.cellWidth);

        for (int x = 0; x < this.grid.gridSize.x; x++) {
            for (int y = 0; y < this.grid.gridSize.y; y++) {
                Gizmos.DrawWireCube(this.grid.grid[x, y].worldPos, wireBoxSize);
            }
        }
        
    }
}
