using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDeform : MonoBehaviour
{

    public int xVertices = 10, zVertices = 10;
    public float tileSize;
    public int borderSize = 1;
    public float noiseIntensity = 1f;
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


        int i = 0;
        for (int z = 0; z <= zVerticesPlusBorder; z++) {
            for (int x = 0; x <= xVerticesPlusBorder; x++) {
                float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * noiseIntensity;;
                bool xInside = borderSize <= x && x <= xVerticesPlusBorder - borderSize;
                bool zInside = borderSize <= z && z <= zVerticesPlusBorder - borderSize;
                if (!xInside || !zInside) {
                    int xDistanceFromBorder = 0, zDistanceFromBorder = 0;
                    if (x < borderSize) {
                        xDistanceFromBorder = borderSize - x;
                    } 
                    else {
                        xDistanceFromBorder = x - (xVerticesPlusBorder - borderSize);
                    }

                    if (z < borderSize) {
                        zDistanceFromBorder = borderSize - z;
                    } 
                    else {
                        zDistanceFromBorder = z - (zVerticesPlusBorder - borderSize);
                    }

                    if (!xInside || z == borderSize || z == zVerticesPlusBorder - borderSize) {
                            y *= 1 - (xDistanceFromBorder / borderSize);
                    }
                    else if (!zInside || x == borderSize || x == xVerticesPlusBorder - borderSize) {
                        y *= 1 - (zDistanceFromBorder / borderSize);
                    }
                    else if (!xInside && !zInside)
                        y *= (1 - ((xDistanceFromBorder + zDistanceFromBorder) / (2 * borderSize)));

                    
                }
                else y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * noiseIntensity;
                vertices[i] = (new Vector3(x, y, z) - new Vector3(borderSize, 0, borderSize)) * tileSize;
                uv[i] = new Vector2((float)x / xVerticesPlusBorder, (float)z / zVerticesPlusBorder);
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
