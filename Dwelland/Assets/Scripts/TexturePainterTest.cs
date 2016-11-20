using UnityEngine;
using System.Collections;

public class TexturePainterTest : MonoBehaviour {

    // Source texture and the rectangular area we want to extract.
    public Texture2D sourceTex;

    void Start()
    {
        Color[] pix = sourceTex.GetPixels(0, 0, sourceTex.width, sourceTex.height);
        Texture2D destTex = new Texture2D(sourceTex.width, sourceTex.width);
        destTex.SetPixels(pix);
        destTex.Apply();

        // Set the current object's texture to show the extracted rectangle from the image
        //Graphics.DrawTexture(sourceRect, destTex);
        GetComponent<Renderer>().material.mainTexture = destTex;
    }
}
