using UnityEngine;

// Contains most of the variables of the project in order to make it easier to call the variables
public class Vars : MonoBehaviour
{
    public static int size = 200;
    public static int colorDetailLvl = 2;
    public static float scale = 50f;
    public static float depthMultiplier = 30f;
    public static int octaves = 3;
    public static float lacunarity = 2.5f;
    public static float persistance = 0.5f;
    public static int seed;
    public static Vector2 offset = Vector2.zero;

    public static float warpFuncM = 0.75f;
    public static float warpFuncN = 0.15f;
}
