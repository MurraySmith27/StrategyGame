using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDeform : MonoBehaviour
{

    public int xVertices = 10, zVertices = 10;

    public float noiseIntensity = 1f;
    Mesh mesh;

    Vector3[] vertices;

    int[] triangles;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        DeformMesh();
        UpdateMesh();
    }

    void DeformMesh() {
        vertices = new Vector3[(xVertices + 1) * (zVertices + 1)];

        triangles = new int[xVertices * zVertices * 6];

        int vert = 0, tris = 0;
        for (int z = 0; z < zVertices; z++) {
            for (int x = 0; x < xVertices; x++) {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + xVertices + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xVertices + 1;
                triangles[tris + 5] = vert + xVertices + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
    

        int i = 0;
        for (int z = 0; z <= zVertices; z++) {
            for (int x = 0; x <= zVertices; x++) {
                float y = x == 0 || z == 0 || x == xVertices || z == zVertices ? 0 : Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * noiseIntensity;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

    }

    void UpdateMesh() {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void OnDrawGizmos() {
        
        if (vertices == null) return;

        foreach (Vector3 vertex in vertices) Gizmos.DrawSphere(vertex, .1f);
    }
}
