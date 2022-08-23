using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class ShopWindows : MonoBehaviour
{
    public string base_url = "http://147.50.231.95:8110/api/RequestAddBasket.ashx";
    public string authen_code,jsonData,status_code;
    public string payment_url;
    public Image imageProduct;
    

    void Start()
    {
        authen_code = PlayerPrefs.GetString("authen_code");
        payment_url = base_url+"?PRODUCTCODE="+this.gameObject.name+"&USERLOGIN="+authen_code;
        
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
            //jsonData = www.downloadHandler.text;
            //JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonData);
            //Debug.Log(jsonNode);
            /*status_code = jsonNode["Code"];
            if (status_code == "OK")
            {
                Debug.Log("message:"+jsonNode["message"]);
            }*/
        }
    }

    public void CloseWindows(){
        Destroy(this.gameObject);
    }

    public void CheckOut(){
        UnityWebRequest www = new UnityWebRequest(payment_url);
        StartCoroutine(ResponseData(payment_url));
        Application.OpenURL("http://147.50.231.95/Pay.aspx?USERLOGIN=user1#"+authen_code);
    }

    public void SetImageProduct(Sprite spriteProduct){
        imageProduct.sprite = spriteProduct;
    }
}
