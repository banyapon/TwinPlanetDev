using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PanoramaCapture : MonoBehaviour
{
    public Camera MainCamera;
    public RenderTexture cubeMap;
    public RenderTexture resultRect;
 
    void Update()
    {
        if (Input.GetMouseButton(0)){
            MainCamera.RenderToCubemap(cubeMap);
            cubeMap.ConvertToEquirect(resultRect);
            savePhoto(resultRect);
        }
    }
 
    public void savePhoto(RenderTexture rt){
        Texture2D texture2D = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        texture2D.ReadPixels(new Rect(0,0,rt.width,rt.height),0,0);
        RenderTexture.active = null;
        byte[] bytes = texture2D.EncodeToJPG();
        string pathImage = Application.dataPath + "/Result" + ".jpg";
        File.WriteAllBytes(pathImage, bytes);
    }
}
