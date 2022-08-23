using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMic : MonoBehaviour
{
    public bool toggleStage = false;

    public void MicToggle(bool stage) 
    {
        toggleStage = stage;
        Debug.Log(stage);
    }

    public void Start()
    {
        toggleStage = false;
    }
}
