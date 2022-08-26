using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopData: MonoBehaviour
{
    public string code;
    public string name;
}
[System.Serializable]
public class GetShopMessage: MonoBehaviour
{
    public string Code;
    public ShopData[] Message;
}

