using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SelectDataShop : MonoBehaviour
{
    [SerializeField] private GameObject spawn_Prefabs;
    [SerializeField] private Transform spawn_Parent;
    [SerializeField] private int num_Data;
    private Sprite newSprite;
    [SerializeField] private string[] _ID;
    [SerializeField] private string[] _Info;
    [SerializeField] private string[] _Price;

    private void Start()
    {
        DataInt(num_Data);
    }
    private void DataInt(int num)
    {
        switch (num)
        {
            case 0:
                for (int i = 1; i < 41; i++)
                {
                    GameObject go = Instantiate(spawn_Prefabs);
                    go.transform.SetParent(spawn_Parent, false);
                    newSprite = Resources.Load<Sprite>("sample/art33/1-" + i);
                    Debug.Log(newSprite);
                    go.transform.GetChild(0).GetComponent<Image>().sprite = newSprite;
                    go.transform.GetChild(1).GetComponent<Text>().text = "Untitled";
                    go.transform.GetChild(2).GetComponent<Text>().text = "Acrylic on canvas";
                    go.transform.GetChild(3).GetComponent<Text>().text = "24,000";
                }
                break;
            case 1:
                for (int i = 0; i < 13; i++)
                {
                    GameObject go = Instantiate(spawn_Prefabs);
                    go.transform.SetParent(spawn_Parent, false);
                    go.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("sample/Gem/" + (i + 1));
                    go.transform.GetChild(1).GetComponent<Text>().text = _ID[i];
                    go.transform.GetChild(2).GetComponent<Text>().text = _Info[i];
                    go.transform.GetChild(3).GetComponent<Text>().text = _Price[i];
                }
                break;
            case 2:
                for (int i = 0; i < 12; i++)
                {
                    GameObject go = Instantiate(spawn_Prefabs);
                    go.gameObject.name = ""+_ID[i];
                    go.transform.SetParent(spawn_Parent, false);
                    go.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("sample/pom/" + (i + 1));
                    go.transform.GetChild(1).GetComponent<Text>().text = _ID[i];
                    go.transform.GetChild(2).GetComponent<Text>().text = _Info[i];
                    go.transform.GetChild(3).GetComponent<Text>().text = _Price[i];
                }
                break;
        }
    }
}
