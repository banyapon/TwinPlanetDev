using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceLoadScene : MonoBehaviour
{
    public void forceLoad(string sceneName){
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
