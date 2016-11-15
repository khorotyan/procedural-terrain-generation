﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LiveLandModifier : MonoBehaviour
{
    public InputField seedInputField;
    public InputField terWidthInputField;
    public InputField terHeightInputField;
    public InputField noiseScaleInputField;
    public InputField octaveInputField;
    public Slider frequencySlider;
    public Text frequencyValueText;
    public Slider amplitudeSlider;
    public Text amplitudeValueText;
    public Slider xOffsetSlider;
    public Text xOffsetValueText;
    public Slider yOffsetSlider;
    public Text yOffsetValueText;
    public Slider warpMSlider;
    public Text warpMValueText;
    public Slider warpNSlider;
    public Text warpNValueText;
    public Toggle randTerToggle;
    public GameObject settingsPanelObj;

    public MeshGen meshGen; // Reference to the MeshGen script
    public LandGenCalc landGenCalc;

    private bool settingsPanelOpen = true;
    private bool canStartInterpolation = false;

    // Awake is called whenever the instance of the script is loaded
    void Awake ()
    {
        seedInputField.contentType = InputField.ContentType.Alphanumeric;
        seedInputField.characterLimit = 8;

        terWidthInputField.contentType = InputField.ContentType.IntegerNumber;
        terWidthInputField.characterLimit = 3;

        terHeightInputField.contentType = InputField.ContentType.IntegerNumber;
        terHeightInputField.characterLimit = 3;

        noiseScaleInputField.contentType = InputField.ContentType.IntegerNumber;
        noiseScaleInputField.characterLimit = 2;

        octaveInputField.contentType = InputField.ContentType.IntegerNumber;
        octaveInputField.characterLimit = 1;
    }
	
    // Update is called every frame
	void Update ()
    {
        OpenCloseSettingsPanel();
    }

    public void OnNewMapGenerate()
    {
        // Make a random offset if the random checkbox is enabled for terrain generation
        if (randTerToggle.isOn)
        {
            Vars.offset.x = UnityEngine.Random.Range(-100000, 100000);
            Vars.offset.y = UnityEngine.Random.Range(-100000, 100000);
        }

        CreateMap();
    }

    void CreateMap()
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

    public void OnRandGenToggleClick()
    {
        if (randTerToggle.isOn == true)
        {
            seedInputField.text = "";
        }
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnSeedValueChanged()
    {
        CreateMap();
    }

    public void OnWidthValueChanged()
    {
        int newWidth = Mathf.Clamp(int.Parse(terWidthInputField.text), 100, 250);
        Vars.terWidth = newWidth;
        terWidthInputField.text = newWidth.ToString();

        CreateMap();
    }

    public void OnHeightValueChanged()
    {
        int newHeight = Mathf.Clamp(int.Parse(terHeightInputField.text), 100, 250);
        Vars.terHeight = newHeight;
        terHeightInputField.text = newHeight.ToString();

        CreateMap();
    }

    public void OnNoiseScaleValueChanged()
    {
        int newNoiseScale = Mathf.Clamp(int.Parse(noiseScaleInputField.text), 10, 99);
        Vars.scale = newNoiseScale;
        noiseScaleInputField.text = newNoiseScale.ToString();

        CreateMap();
    }

    public void OnOctaveValueChanged()
    {
        int newOctave = Mathf.Clamp(int.Parse(octaveInputField.text), 1, 10);
        Vars.octaves = newOctave;
        octaveInputField.text = newOctave.ToString();

        CreateMap();
    }

    public void OnFrequencyValueChanged()
    {
        Vars.lacunarity = frequencySlider.value;
        frequencyValueText.text = frequencySlider.value.ToString();

        CreateMap();
    }

    public void OnAmplitudeValueChanged()
    {
        Vars.persistance = amplitudeSlider.value;
        amplitudeValueText.text = amplitudeSlider.value.ToString();

        CreateMap();
    }

    public void OnXOffsetValueChanged()
    {
        Vars.offset.x = xOffsetSlider.value;
        xOffsetValueText.text = xOffsetSlider.value.ToString();

        CreateMap();
    }

    public void OnYOffsetValueChanged()
    {
        Vars.offset.y = yOffsetSlider.value;
        yOffsetValueText.text = yOffsetSlider.value.ToString();

        CreateMap();
    }

    public void OnWarpMValueChanged()
    {
        Vars.warpFuncM = warpMSlider.value;
        warpMValueText.text = warpMSlider.value.ToString();

        CreateMap();
    }

    public void OnWarpNValueChanged()
    {
        Vars.warpFuncN = warpNSlider.value;
        warpNValueText.text = warpNSlider.value.ToString();

        CreateMap();
    }

    public void OpenCloseSettingsPanel()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            canStartInterpolation = true;
        }

        if (canStartInterpolation == true)
        {
            if (settingsPanelOpen == true)
            {
                openSettings();

                // If tab is pressed again, stop opening the panel (stop interpolation), close it
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    canStartInterpolation = false;
                    settingsPanelOpen = false;

                    closeSettings();
                }
            }
            else
            {
                closeSettings();

                // If tab is pressed again, stop closing the panel (stop interpolation), open it
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    canStartInterpolation = false;
                    settingsPanelOpen = true;

                    openSettings();
                }
            }
        }   
    }

    void openSettings()
    {
        settingsPanelObj.transform.localScale = Vector3.Lerp(settingsPanelObj.transform.localScale, new Vector3(1.7f, 1, 1), 2f * Time.deltaTime);

        if (settingsPanelObj.transform.localScale.x > Mathf.Abs(1.699f) && settingsPanelObj.transform.localScale.x < Mathf.Abs(1.701f))
        {
            canStartInterpolation = false;
            settingsPanelOpen = false;
        }
    }

    void closeSettings()
    {
        settingsPanelObj.transform.localScale = Vector3.Lerp(settingsPanelObj.transform.localScale, new Vector3(1, 1, 1), 2f * Time.deltaTime);

        if (settingsPanelObj.transform.localScale.x > Mathf.Abs(0.99f) && settingsPanelObj.transform.localScale.x < Mathf.Abs(1.01f))
        {
            canStartInterpolation = false;
            settingsPanelOpen = true;
        }
    }
}
