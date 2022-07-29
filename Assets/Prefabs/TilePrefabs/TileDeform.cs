using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDeform : MonoBehaviour
{

    public int xVertices = 10, zVertices = 10;
    public float tileSize;
    public int borderSize = 1;
    public float noiseIntensity = 1f;
    public float minMiddleHeight = 0.1f;
    public float maxMiddleHeight = 2f;
    public float yOffset = 1f;
    Mesh mesh;

    Vector3[] vertices;

    Vector2[] uv;

    int[] triangles;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        DeformMesh();
        UpdateMesh();
    }

    void DeformMesh() {
        int xVerticesPlusBorder = xVertices + 2 * borderSize;
        int zVerticesPlusBorder = zVertices + 2 * borderSize;
        vertices = new Vector3[(xVerticesPlusBorder + 1) * (zVerticesPlusBorder + 1)];

        triangles = new int[xVerticesPlusBorder * zVerticesPlusBorder * 6];

        int vert = 0, tris = 0;
        for (int z = 0; z < zVerticesPlusBorder; z++) {
            for (int x = 0; x < xVerticesPlusBorder; x++) {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + xVerticesPlusBorder + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xVerticesPlusBorder + 1;
                triangles[tris + 5] = vert + xVerticesPlusBorder + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }

        uv = new Vector2[(xVerticesPlusBorder + 1) * (zVerticesPlusBorder + 1)];


        Vector3[][] tempVerts = new Vector3[zVerticesPlusBorder + 1][];
        int j = 0;
        for (int z = 0; z <= zVerticesPlusBorder; z++) {
            tempVerts[z] = new Vector3[xVerticesPlusBorder + 1];
            for (int x = 0; x <= xVerticesPlusBorder; x++) {
                float y = Mathf.Clamp(Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * noiseIntensity, minMiddleHeight, maxMiddleHeight);
                tempVerts[z][x] = new Vector3((x - borderSize) * tileSize, y, (z - borderSize) * tileSize);
                uv[j] = new Vector2((float)x / xVerticesPlusBorder, (float)z / zVerticesPlusBorder);
                j++;
            }
        }

        int i = 0;
        for (int z = 0; z <= zVerticesPlusBorder; z++) {
            for (int x = 0; x <= xVerticesPlusBorder; x++) {

                float y = tempVerts[z][x].y;

                bool xInside = borderSize <= x && x <= xVerticesPlusBorder - borderSize;
                bool zInside = borderSize <= z && z <= zVerticesPlusBorder - borderSize;
                if (!xInside || !zInside) {
                    float yMult = 1f;
                    int xDistanceFromBorder = 0, zDistanceFromBorder = 0;
                    if (x < borderSize) {
                        xDistanceFromBorder = borderSize - x;
                    } 
                    else if (xVerticesPlusBorder - borderSize < x) {
                        xDistanceFromBorder = x - (xVerticesPlusBorder - borderSize);
                    }

                    if (z < borderSize) {
                        zDistanceFromBorder = borderSize - z;
                    } 
                    else if (zVerticesPlusBorder - borderSize < z) {
                        zDistanceFromBorder = z - (zVerticesPlusBorder - borderSize);
                    }

                    int closestX = x, closestZ = z;
                    if (!xInside) {
                        if (x < borderSize) closestX = borderSize;
                        else closestX = xVerticesPlusBorder - borderSize;
                    }
                    if (!zInside) {
                        if (z < borderSize) closestZ = borderSize;
                        else closestZ = zVerticesPlusBorder - borderSize;
                    }

                    if (xDistanceFromBorder == zDistanceFromBorder) {
                        yMult = 1 - (float)(xDistanceFromBorder * xDistanceFromBorder + zDistanceFromBorder * zDistanceFromBorder) / (2 * borderSize * borderSize);
                    }
                    else {
                        yMult = 1 - ((float)Mathf.Max(xDistanceFromBorder, zDistanceFromBorder) / borderSize);
                    }

                    Debug.Log($"xDistance: {xDistanceFromBorder}, zDistance: {zDistanceFromBorder}, yMult: {yMult}");

                    y = yMult * tempVerts[closestZ][closestX].y;
                }

                vertices[i] = new Vector3(tempVerts[z][x].x, y - yOffset, tempVerts[z][x].z);
                i++;
            }
        }
    }

    void UpdateMesh() {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.SetUVs(0, uv);
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void OnDrawGizmos() {
        
        if (vertices == null) return;

        foreach (Vector3 vertex in vertices) Gizmos.DrawSphere(vertex, .1f);
    }
}
