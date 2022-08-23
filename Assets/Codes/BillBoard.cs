using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private new Camera camera;
    void Start()
    {
        camera = Camera.main;
        if (camera == null) camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    private void LateUpdate() => transform.rotation = camera.transform.rotation;
}
