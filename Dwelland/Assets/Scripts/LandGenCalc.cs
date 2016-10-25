using System;
using UnityEngine;
using System.Collections;

public class LandGenCalc : MonoBehaviour
{
    // Makes changes the depth values in a way to make the terrain more natural looking
    // Returns the  'z' values of the noise (the depth of the terrain)
    public static float[,] GenerateLand(int _width, int _height, float _scale)
    {
        float[,] noiseValues = new float[_width + 1, _height + 1];

        _scale = _scale == 0 ? 0.0001f : _scale; // Prevents getting division by zero error

        for (int y = 0; y <= _height; y++)
        {
            for (int x = 0; x <= _width; x++)
            {
                // Scales the x and y coordinates of the perlin noise (we do not have to take integer values)
                // Taking closer x and y values will make the transition between coordinates smoother
                float tempX = x / _scale;
                float tempY = y / _scale;

                float zCoord = Mathf.PerlinNoise(tempX, tempY); // Gets the perlin values

                noiseValues[x, y] = zCoord;

            }
        }

        return noiseValues;
    }

}
