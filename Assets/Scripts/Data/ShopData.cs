using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopData
{
    public string code;
    public string name;
}
[System.Serializable]
public class GetShopMessage
{
    public string Code;
    public ShopData[] Message;
}

