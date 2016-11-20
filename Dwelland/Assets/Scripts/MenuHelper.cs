using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuHelper : MonoBehaviour
{

    public GameObject textContainer;
    public Text helperText;

    private string createTerrainInfo;
    private bool canUpdatePos = false;

    void Awake()
    {
        createTerrainInfo = "This is a helper";
    }

    public void Update()
    {
        // If we can update the position of the helper text, then update it
        if (canUpdatePos == true)
        {
            textContainer.transform.position = Input.mousePosition;
        }
    }

    // Change the text of the helper depending on the name of the UI that we are hovering over 
    //      Set the mouse position as the text position
    public void OnUIHoverEnter(string name)
    {
        if (name == "CreateTerrain")
        {
            createTerrainInfo = "Creates a new terrain";
        }
        else if (name == "MapSeedInput")
        {
            createTerrainInfo = "Using the same seed, creates the same terrain";
        }
        else if (name == "TerrainSize")
        {
            createTerrainInfo = "The width and height of the Terrain";
        }
        else if (name == "NoiseScale")
        {
            createTerrainInfo = "The size of the Noise from Perlin noise";
        }
        else if (name == "Octaves")
        {
            createTerrainInfo = "Number of Graphs for the Terrain";
        }
        else if (name == "Frequency")
        {
            createTerrainInfo = "Frequency of the graph";
        }
        else if (name == "Amplitude")
        {
            createTerrainInfo = "Amplitude of the graph";
        }
        else if (name == "xOffset")
        {
            createTerrainInfo = "Offset throgh the 'x' axis";
        }
        else if (name == "yOffset")
        {
            createTerrainInfo = "Offset throgh the 'y' axis";
        }
        else if (name == "WarpM")
        {
            createTerrainInfo = "The coordinate 'm' of the warped graph";
        }
        else if (name == "WarpN")
        {
            createTerrainInfo = "The coordinate 'n' of the warped graph";
        }
        else if (name == "LevelOfDetail")
        {
            createTerrainInfo = "Detail of coloring the terrain";
        }

        canUpdatePos = true;
        helperText.text = createTerrainInfo;
        textContainer.transform.position = Input.mousePosition;

        textContainer.SetActive(true);
    }

    // Disable the helper text
    public void OnUIHoverExit()
    {
        canUpdatePos = false;
        textContainer.SetActive(false);
    }

}
