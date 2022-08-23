using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    public const string basketUrl = "http://147.50.231.95/Pay.aspx?ProductCode=2RS2124N13&USERLOGIN=yutbuu";
    public Transform contentShopTransform;
    public GameObject btnShopPrefab;
    public Transform contentProductTransform;
    public GameObject btnProductPrefab;
    public TextMeshProUGUI nameTag;
    public TextMeshProUGUI priceTag;
    public TextMeshProUGUI pathFileTag;
    public Image productImage;
    public Button addToBasket;

    public Animator notificationAnim;
    public TextMeshProUGUI respText;
    private void Start()
    {
        InitializeShopData();
    }
    public void InitializeShopData()
    {
        StartCoroutine(TwinPlanetApi.GetShopListRequest((shopData) =>
        {
            for (int i = 0; i < shopData.Length; i++)
            {
                int x = i;
                var btn = Instantiate(btnShopPrefab, contentShopTransform);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = shopData[i].name;
                btn.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(TwinPlanetApi.RequestProductByShop(shopData[x].code, InitializeProductsFromShopData)
                    ));
            }
        }));
    }

    public void InitializeProductsFromShopData(ProductData[] products)
    {
        Transform[] allChild = contentProductTransform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < allChild.Length; i++)
        {
            if (allChild[i] != contentProductTransform)
                Destroy(allChild[i].gameObject);
        }
        for (int i = 0; i < products.Length; i++)
        {
            int x = i;
            var btn = Instantiate(btnProductPrefab, contentProductTransform);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = products[i].name;
            btn.GetComponent<Button>().onClick.AddListener(() => InitializeSingleProduct(products[x]));
        }
    }

    public void InitializeSingleProduct(ProductData p)
    {
        nameTag.text = p.name;
        priceTag.text = p.price;
        pathFileTag.text = p.pathfile;
        StartCoroutine(TwinPlanetApi.RequestProductImage(p.code, SetProductImage));
        addToBasket.onClick.RemoveAllListeners();
        addToBasket.onClick.AddListener(() =>
        {
            StartCoroutine(TwinPlanetApi.AddToBasket(p.code, TwinPlanetApi.currentUserID, OnAddToBasketResp));
        });
    }

    public void SetProductImage(Texture2D tex)
    {
        Sprite sp = Sprite.Create(tex, new Rect(new Vector2(0, 0), new Vector2(tex.width, tex.height)), new Vector2(0.5f, 0.5f));
        productImage.sprite = sp;
    }
    public void OnAddToBasketResp(AddToBasketResponse resp)
    {
        Debug.Log(resp.Message);
        notificationAnim.SetTrigger("Trigger");
        respText.text = resp.Message;
    }

    public void OpenBasket()
    {
        TwinPlanetApi.OpenBasket();
    }
}

