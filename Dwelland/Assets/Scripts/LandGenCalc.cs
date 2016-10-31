using System;
using UnityEngine;
using System.Collections;
using System.Text;

public class LandGenCalc : MonoBehaviour
{
    public LiveLandModifier llm;

    // Makes changes the depth values in a way to make the terrain more natural looking
    // Returns the  'z' values of the noise (the depth of the terrain)
    public float[,] GenerateLand(int _width, int _height, float _scale, int _octaves, float _lacunarity, float _persistance)
    {
        float[,] noiseValues = new float[_width + 1, _height + 1];

        _scale = _scale == 0 ? 0.0001f : _scale; // Prevents getting division by zero error
        float minDepth = 1000f;
        float maxDepth = -1000f;

        Vector2 randOffset = Vector2.zero;
        string seed = llm.seedInputField.text;

        // If a seed is entered, translate the seed into an offset and disable random terrain generation
        if (seed != "")
        {
            llm.randTerToggle.isOn = false;

            randOffset = Vector2.zero;

            // Treat the first half of the seed text as 'x' value
            for (int i = 0; i < seed.Length / 2; i++)
            {
                int n;
                int singleDigit = Int32.TryParse(seed[i].ToString(), out n) ? Convert.ToInt32(seed[i].ToString()) : seed[i] % 30;
                randOffset.x += singleDigit * (int) Mathf.Pow(10, seed.Length / 2 - i - 1);
            }

            // Treat the second half of the seed text as 'y' value
            for (int j = seed.Length / 2, i = 0; j < seed.Length; j++, i++)
            {
                int n;
                int singleDigit = Int32.TryParse(seed[j].ToString(), out n) ? Convert.ToInt32(seed[j].ToString()) : seed[j] % 30;
                randOffset.y += singleDigit * (int) Mathf.Pow(10, seed.Length / 2 - i - (seed.Length % 2 == 0 ? 1 : 0));
            }
            Debug.Log(randOffset);
        }

        // Make a random offset if the random checkbox is enabled for terrain generation
        if (llm.randTerToggle.isOn)
        {
            randOffset.x = UnityEngine.Random.Range(-100000, 100000);
            randOffset.y = UnityEngine.Random.Range(-100000, 100000);
        }

        for (int y = 0; y <= _height; y++)
        {
            for (int x = 0; x <= _width; x++)
            {
                float frequency = 1;
                float amplitude = 1;
                float depth = 0;

                for (int i = 0; i < _octaves; i++)
                {
                    // Scales the x and y coordinates of the perlin noise (we do not have to take integer values)
                    // Taking closer x and y values will make the transition between coordinates smoother
                    float tempX = x / _scale * frequency + randOffset.x;
                    float tempY = y / _scale * frequency + randOffset.y;

                    float zCoord = 2 * Mathf.PerlinNoise(tempX, tempY) - 1f; // Gets the perlin values in the range [-1, 1]
                    depth += zCoord * amplitude;

                    frequency *= _lacunarity;
                    amplitude *= _persistance;
                }

                if (depth > maxDepth)
                {
                    maxDepth = depth;
                }
                else if (depth < minDepth)
                {
                    minDepth = depth;
                }

                noiseValues[x, y] = depth;
            }
        }

        for (int y = 0; y <= _height; y++)
        {
            for (int x = 0; x <= _width; x++)
            {
                // minDepth becomes - 0, maxDepth - 1 (clamps min and max depths between [0, 1]
                // and returns a value between [0, 1]
                noiseValues[x, y] = Mathf.InverseLerp(minDepth, maxDepth, noiseValues[x, y]);
            }
        }

        return noiseValues;
    }

}
