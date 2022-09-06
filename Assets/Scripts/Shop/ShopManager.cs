using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private string _shopCode;
    private string _authenCode;

    [Header("USER INTERFACE")]
    [SerializeField] private GameObject _btnProductPrefab;
    [SerializeField] private Transform _productListTransform;
    [SerializeField] private Text _nameTag;
    [SerializeField] private Text _priceTag;
    [SerializeField] private Image _productImage;
    [SerializeField] private Button _addToBasket;

    void Start()
    {
        _authenCode = PlayerPrefs.GetString("authen_code");

        _nameTag.text = null;
        _priceTag.text = null;
        _productImage.sprite = null;

        StartCoroutine(TwinPlanetApi.RequestProductByShop(_shopCode, InitializeProductFromShopData));
    }

    public void InitializeProductFromShopData(ProductData[] products)
    {
        for (int i = 0; i < products.Length; i++)
        {
            int x = i;
            var btn = Instantiate(_btnProductPrefab, _productListTransform);
            btn.GetComponentInChildren<Text>().text = products[i].name;
            btn.GetComponent<Button>().onClick.AddListener(() => InitializeSingleProduct(products[x]));
        }
    }

    public void InitializeSingleProduct(ProductData product)
    {
        _nameTag.text = product.name;
        _priceTag.text = product.price;
        _productImage.sprite = null;
        StartCoroutine(TwinPlanetApi.RequestProductImage(product.code, SetProductImage));
        _addToBasket.onClick.RemoveAllListeners();
        _addToBasket.onClick.AddListener(() => 
        {
            StartCoroutine(TwinPlanetApi.AddToBasket(product.code, "user1#" + _authenCode, OnAddToBasketResp));
        });
    }

    public void SetProductImage(Texture2D tex)
    {
        Sprite sp = Sprite.Create(tex, new Rect(new Vector2(0, 0), new Vector2(tex.width, tex.height)), new Vector2(0.5f, 0.5f));
        _productImage.sprite = sp;
    }

    public void OnAddToBasketResp(AddToBasketResponse resp)
    {
        Debug.Log(resp.Message);
    }

    public void OpenBasket()
    {
        Application.OpenURL("http://147.50.231.95/Pay.aspx?USERLOGIN=user1#" + _authenCode);
    }
}
