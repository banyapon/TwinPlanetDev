using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
public class SessionData : MonoBehaviour
{
    public string base_url;
    public string authen_code, nickname, jsonData,glb_url;
    private string status_code;
    private string param, api_url;
    void Start()
    {
        authen_code = PlayerPrefs.GetString("authen_code");
        nickname = PlayerPrefs.GetString("nickname");
        glb_url = PlayerPrefs.GetString("glb_url");
        param = "authen_code=" + authen_code;
        api_url = base_url + "usedavatar.php" + "?" + param;
        UnityWebRequest www = new UnityWebRequest(api_url);
        StartCoroutine(ResponseData(api_url));
    }

    IEnumerator ResponseData(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();
        if(www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            jsonData = www.downloadHandler.text;
            JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonData);
            status_code = jsonNode["status_code"];
            if (status_code == "200")
            {
                Debug.Log("message:"+jsonNode["message"]);
            }
        }
    }

    public void UpdateGLB_URL(string glb_source){
            string param_glb, api_url_glb;
            param_glb = "glb_url=" + glb_source+"&authen_code="+authen_code;
            api_url_glb = base_url+"usedavatar.php" + "?" + param_glb;
            UnityWebRequest www = new UnityWebRequest(api_url_glb);
            StartCoroutine(ResponseData(api_url_glb));
    }

}
