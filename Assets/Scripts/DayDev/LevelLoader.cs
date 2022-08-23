using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class LevelLoader : MonoBehaviour
{
    public string url, dataPath;
    public GameObject uiLevel;
    public string assetBundleName;
    private AssetBundle assetBundle;
    void Start()
    {
        uiLevel.SetActive(true);
        StartCoroutine(DownloadModel());
        url = dataPath + "/data/" + assetBundleName;
    }

    void Update() {
        if(GameObject.Find(assetBundleName+"(Clone)")){
            uiLevel.SetActive(false);
        }
    }
    IEnumerator DownloadModel()
    {
        string url = dataPath + "/data/" + assetBundleName;
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url, 0);
        yield return request.Send();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
        GameObject temple = bundle.LoadAsset<GameObject>(assetBundleName);
        Instantiate(temple);
    }
}
