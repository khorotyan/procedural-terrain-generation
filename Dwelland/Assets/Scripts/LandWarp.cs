using UnityEngine;
using System.Collections;

// Warps the land to make it look like an iseland
public class LandWarp : MonoBehaviour
{
    public static float[,] CreateWarpedLand(int width, int height)
    {
        float[,] warpDepth = new float[width + 1, height + 1];
        float[,] warpXVals = new float[width + 1, height + 1];

        for (int y = 0; y <= width; y++)
        {
            for (int x = 0; x <= height; x++)
            {
                float tempX = (x / (float)width) * 2 - 1; // makes the x value of the terrain in the range [-1, 1]
                float tempY = (y / (float)width) * 2 - 1;

                float funcX = Mathf.Max(Mathf.Abs(tempX), Mathf.Abs(tempY));

                warpXVals[x, y] = funcX;
            }
        }

        int size = Vars.size;
        int starter = 0;

        float depthValue;
        
        // An opimized algorithm which does only a single calculation per tier (read more at Game Design - Book 1 - page 24)
        // The warp amount is retrived from the created spline and applied to all the points from each tier
        for (int tier = 0; tier <= Vars.size / 2; tier++, size--, starter++)
        {
            depthValue = WarpFunction(warpXVals[tier, tier]);

            for (int x1 = starter, x2 = size, x3 = size, x4 = starter; x1 < size; x1++, x3--)
            {
                int y1 = x4, y2 = x1, y3 = x2, y4 = x3;

                warpDepth[x1, y1] = depthValue;
                warpDepth[x2, y2] = depthValue;
                warpDepth[x3, y3] = depthValue;
                warpDepth[x4, y4] = depthValue;
            }

        }

        warpDepth[Vars.size / 2 + 1, Vars.size / 2 + 1] = 0;

        return warpDepth;
    }
    
    // Checks which function we need for calculating warp amount and gets the warp value at the corresponding x value
	public static float WarpFunction(float x)
    {  
        float m = Vars.warpFuncM;
        float n = Vars.warpFuncN;

        float valY;
        float S0;
        float S1;

        float d0 = (n - m) / (2 * Mathf.Pow(m, 2) * (m - 1));
        float b0 = n / m - Mathf.Pow(m, 2) * d0;
        float a1 = n;
        float b1 = n / m + 2 * Mathf.Pow(m, 2) * d0;
        float c1 = 3 * m * d0;
        float d1 = m * d0 / (m - 1);

        // Use the S0 function when x value is in the range [0, m], and the S1 function when (m, 1]
        if (x <= m)
        {
            S0 = b0 * x + d0 * Mathf.Pow(x, 3);
            valY = S0;
        }
        else
        {
            S1 = a1 + b1 * (x - m) + c1 * Mathf.Pow(x - m, 2) + d1 * Mathf.Pow(x - m, 3);
            valY = S1;
        }

        return valY;
    }

}