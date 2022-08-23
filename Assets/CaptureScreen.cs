using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureScreen : MonoBehaviour
{
    public static WebGLUpload _webGLUpload = null;
    public static WebGLDownload _webGLDownload = null;
    private void Awake()
    {
        
        _webGLUpload = GetComponent<WebGLUpload>();
        _webGLDownload = GetComponent<WebGLDownload>();
    }
    void OnMouseDown()
    {
        ScreenCapture.CaptureScreenshot("ParisRoad_short");
    }

    public void TakePhoto(){
        ScreenCapture.CaptureScreenshot("ParisRoad_short");
        _webGLDownload.GetScreenshot(WebGLDownload.ImageFormat.jpg,1,"ScreenShortTW");
    }
}
