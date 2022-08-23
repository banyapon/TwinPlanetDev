using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ProductData: MonoBehaviour
{
    public string code;
    public string name;
    public string price;
    public string pathfile;
}

[System.Serializable]
public class GetProductMessage: MonoBehaviour
{
    public string Code;
    public ProductData[] Message;
}
[System.Serializable]
public class AddToBasketResponse : MonoBehaviour
{
    public string Code;
    public string Message;
}

