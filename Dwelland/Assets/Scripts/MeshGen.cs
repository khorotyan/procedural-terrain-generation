using System;
using UnityEngine;
using System.Collections;

public class MeshGen : MonoBehaviour
{
    public Renderer rer;
    public Material defMat;
    private GameObject terrainObj;

    private Vector3[] vertices;

    public float[,] depthValues = new float[Vars.terWidth + 1, Vars.terHeight + 1];
    public LandGenCalc landGenCalc;
    public ColorInfo[] heightColors;

    public void Awake()
    {
        int width = Vars.terWidth;
        int height = Vars.terHeight;
        float scale = Vars.scale;
        float multiplier = Vars.depthMultiplier;
        int oct = Vars.octaves;
        float lac = Vars.lacunarity;
        float pers = Vars.persistance;

        float[,] zValues = landGenCalc.GenerateLand(width, height, scale, oct, lac, pers); // Gets the revised 'z' values from 'LandGenCalc' class

        ConstructTerrain(width, height, multiplier, zValues);
    }

    public void ConstructTerrain(int _w, int _h, float _multiplier, float[,] _noiseValues)
    {
        int newH = 2 * _h;
        int newW = 2 * _w;

        depthValues = _noiseValues;

        terrainObj = new GameObject("Terrain");
        MeshFilter mf = terrainObj.AddComponent<MeshFilter>() as MeshFilter;
        terrainObj.AddComponent<MeshRenderer>();
        Mesh mesh = mf.mesh;

        vertices = new Vector3[(_w + 1) * (_h + 1)];
        int[] triangles = new int[(2 * _w * _h) * 3]; // Triangles contain 3 points, determines which side of the mesh is visible
        Vector2[] uvs = new Vector2[(_w + 1) * (_h + 1)];

        for (int y = 0; y <= _h; y++)
        {
            for (int x = 0; x <= _w; x++)
            {     
                vertices[y * (_w + 1) + x] = new Vector3(x, _noiseValues[x, y] * _multiplier, y); // works like assigning vertices[x, y]

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

        /*
         * Update the collider info
        if (terrainObj.GetComponent<MeshCollider>() != null)
            Destroy(terrainObj.GetComponent<MeshCollider>());
        terrainObj.AddComponent<MeshCollider>();
        */

        terrainObj.GetComponent<Renderer>().material = defMat; // Set the terrain material

        terrainObj.transform.position = new Vector3(-Vars.terWidth / 2, 0, -Vars.terHeight / 2); // Center the terrain

        // Update the colors of the terrain
        UpdateColors();
    }

    // Get the colors depending on the height of the vertex
    //      Skip every second and third vertex for interpolation
    void UpdateColors()
    {
        int divider = (int) Mathf.Pow(2, Vars.colorDetailLvl);

        int width = divider * Vars.terWidth;
        int height = divider * Vars.terHeight;

        Color[] colorcontainer = new Color[width * height];

        for (int y = 0; y <= height; y+=3 + divider)
        {
            for (int x = 0; x <= width; x+=3 + divider)
            {
                float terDepth = depthValues[x / divider, y / divider];

                for (int i = 0; i < heightColors.Length; i++)
                {
                    if (terDepth <= heightColors[i].height)
                    {
                        colorcontainer[y * width + x] = heightColors[i].color;
                        break;
                    }               
                }
            }
        }
        
        // Color the remaining vertices
        for (int y = 0; y <= height - 3; y += 3 + divider)
        {
            for (int x = 0; x <= width - 3; x += 3 + divider)
            {
                Color y1 = colorcontainer[y * width + x];
                Color yNext = colorcontainer[y * width + x + (3 + divider)];

                for (int i = 1; i <= 3 + divider; i++)
                {
                    colorcontainer[y * width + x + i] = y1 + i * (yNext - y1) / (3 + divider);
                }
            }
        }

        for (int y = 0; y <= height - divider; y += 3 + divider)
        {
            for (int x = 0; x <= width - divider; x++)
            {
                Color y1 = colorcontainer[y * width + x];
                Color yNext = colorcontainer[(y + 3 + divider) * width + x];

                for (int i = 1; i <= 3 + divider; i++)
                {
                    colorcontainer[(y + i) * width + x] = y1 + i * (yNext - y1) / (3 + divider);
                }

                //colorcontainer[(y + 1) * width + x] = y1 + 1 * (y4 - y1) / (3 + divider);
                //colorcontainer[(y + 2) * width + x] = y1 + 2 * (y4 - y1) / (3 + divider);
            }
        }

        // End of -  Color the remaining vertices

        // Apply the texture
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorcontainer);
        texture.Apply();

        Renderer rend = GameObject.Find("Terrain").GetComponent<Renderer>();
        rend.sharedMaterial.mainTexture = texture;
        // End of - Apply the texture
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

// Make the ColorInfo struct visible in the inspector for manual height insertion
[System.Serializable]
public struct ColorInfo
{
    public float height;
    public Color color;
}

