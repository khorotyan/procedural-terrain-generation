using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LiveLandModifier : MonoBehaviour
{
    public InputField seedInputField;
    public Toggle randTerToggle;

    public MeshGen meshGen; // Reference to the MeshGen script
    public LandGenCalc landGenCalc;

    // Awake is called whenever the instance of the script is loaded
    void Awake ()
    {
        seedInputField.contentType = InputField.ContentType.Alphanumeric;
        seedInputField.characterLimit = 8;
	}
	
    // Update is called every frame
	void Update ()
    {
	    
	}

    public void OnNewMapGenerate()
    {
        // Delete Previous Terrain
        GameObject terrain = GameObject.Find("Terrain"); // Find the terrain gameObject and assign to "terrain" gameObject
        Destroy(terrain);

        // Construct the new Terrain
        int width = Vars.terWidth;
        int height = Vars.terHeight;
        float scale = Vars.scale;
        float multiplier = Vars.depthMultiplier;
        int oct = Vars.octaves;
        float lac = Vars.lacunarity;
        float pers = Vars.persistance;

        float[,] zValues = landGenCalc.GenerateLand(width, height, scale, oct, lac, pers); // Gets the revised 'z' values from 'LandGenCalc' class

        meshGen.ConstructTerrain(width, height, multiplier, zValues);
    }
}
