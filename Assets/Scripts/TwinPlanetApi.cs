using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TwinPlanetApi
{
    public static string currentUserID = "user1";
    public const string baseUrl = "http://147.50.231.95:8110/";
    public const string getShopUrl = "api/RequestShop.ashx";
    public const string getProductByShopUrl = "api/RequestProductByShop.ashx?CODE=";
    public const string getProductTextureUrl = "api/DownloadImageByCode.ashx?CODE=";
    public const string addToBasketUrl = "api/RequestAddBasket.ashx?";
    public static IEnumerator GetShopListRequest(Action<ShopData[]> callBack)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(baseUrl + getShopUrl))
        {
            // Request and wait for the desired page.
            webRequest.SetRequestHeader("Content-Type", "application/*+json");
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.downloadHandler.text);
                string rawData = webRequest.downloadHandler.text;
                GetShopMessage getShopData = JsonUtility.FromJson<GetShopMessage>(rawData);
                Debug.Log(getShopData.Message);
                callBack(getShopData.Message);
            }
        }
    }

    public static IEnumerator RequestProductByShop(string shopCode, Action<ProductData[]> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(baseUrl + getProductByShopUrl + shopCode))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.downloadHandler.text);
                string rawData = webRequest.downloadHandler.text;
                GetProductMessage getProductMessage = JsonUtility.FromJson<GetProductMessage>(rawData);
                callback(getProductMessage.Message);
            }
        }
    }

    public static IEnumerator RequestProductImage(string code, Action<Texture2D> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(baseUrl + getProductTextureUrl + code))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D rawData = (webRequest.downloadHandler as DownloadHandlerTexture).texture;
                callback(rawData);
            }
        }
    }

    public static IEnumerator AddToBasket(string code, string userID, Action<AddToBasketResponse> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(baseUrl + addToBasketUrl + "PRODUCTCODE=" + code + "&USERLOGIN=" + userID))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.downloadHandler.text);
                string rawData = webRequest.downloadHandler.text;
                AddToBasketResponse addTOBasketResp = JsonUtility.FromJson<AddToBasketResponse>(rawData);
                callback(addTOBasketResp);
            }
        }
    }

    public static void OpenBasket()
    {
        Application.OpenURL($"http://147.50.231.95/Pay.aspx?USERLOGIN={currentUserID}");
    }
}
