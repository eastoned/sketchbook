using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveImage : MonoBehaviour
{

    public RenderTexture camCap;
    public Material mat;

    void SaveTexture()
    {
        // Set the supplied RenderTexture as the active one
        GetComponent<Camera>().Render();
        RenderTexture.active = camCap;
        Texture2D tex = new Texture2D(camCap.width, camCap.height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, camCap.width, camCap.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;



        Debug.Log(tex.height + ":" + tex.width + ":" + tex.dimension);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            Debug.Log(camCap.height + ":" + camCap.width + ":" + camCap.dimension);
            StartCoroutine("TakingShot");
            //SaveTexture();
        }
    }

    IEnumerator TakingShot()
    {
        yield return new WaitForEndOfFrame();
        var scrnsht = new Texture2D(camCap.width, camCap.height);
        RenderTexture rt = new RenderTexture(scrnsht.width / 2, scrnsht.height / 2, 0);
        var cam = GetComponent<Camera>(); var prt = cam.targetTexture;
        cam.targetTexture = rt; cam.Render();
        scrnsht.ReadPixels(new Rect(0, 0, camCap.width, camCap.height), 0, 0); scrnsht.Apply();
        cam.targetTexture = prt;
        Debug.Log("assigned");
        byte[] bytes = scrnsht.EncodeToPNG();
        System.IO.File.WriteAllBytes("/Users/eastonself/Desktop/file3.png", bytes);
    }
}
