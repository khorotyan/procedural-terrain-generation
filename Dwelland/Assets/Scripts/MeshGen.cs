using System;
using UnityEngine;
using System.Collections;

public class MeshGen : MonoBehaviour
{
    public Material defMat;

    private Vector3[] vertices;

    public void Awake()
    {
        int width = Vars.terWidth;
        int height = Vars.terHeight;
        float scale = Vars.scale;
        float[,] zValues = LandGenCalc.GenerateLand(width, height, scale); // Gets the revised 'z' values from 'LandGenCalc' class

        ConstructTerrain(width, height, scale, zValues);
    }

    public void ConstructTerrain(int _w, int _h, float _scale, float[,] _noiseValues)
    {
        GameObject terrainObj = new GameObject("Terrain");
        MeshFilter mf = terrainObj.AddComponent<MeshFilter>() as MeshFilter;
        MeshRenderer mr = terrainObj.AddComponent<MeshRenderer>() as MeshRenderer;
        Mesh mesh = mf.mesh;

        vertices = new Vector3[(_w + 1) * (_h + 1)];
        int[] triangles = new int[(2 * _w * _h) * 3]; // Triangles contain 3 points, determines which side of the mesh is visible
        Vector2[] uvs = new Vector2[(_w + 1) * (_h + 1)];

        for (int y = 0; y <= _h; y++)
        {
            for (int x = 0; x <= _w; x++)
            {
                vertices[y * (_w + 1) + x] = new Vector3(x, _noiseValues[x, y] * 30f, y); // works like assigning vertices[x, y]

                uvs[y * (_w + 1) + x] = new Vector2(x / (float)_w, y / (float)_h);
            }
        }

        for (int i = 0, ti = 0, y = 0; y < _h; y++, ti++)
        {
            for (int x = 0; x < _w; x++, i++, ti++)
            {
                triangles[6 * i] = ti;
                triangles[6 * i + 1] = triangles[6 * i + 4] = ti + _w + 1;
                triangles[6 * i + 2] = triangles[6 * i + 3] = ti + 1;
                triangles[6 * i + 5] = ti + _w + 2;
            }
        }

        

        // Update the mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.Optimize();
        mesh.RecalculateNormals();

        terrainObj.GetComponent<Renderer>().material = defMat; // Set the terrain material
    }

    /*
    // Shows the vertices
    public void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        Gizmos.color = Color.black;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
    */
}
