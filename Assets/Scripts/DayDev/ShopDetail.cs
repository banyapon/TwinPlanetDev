using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopDetail : MonoBehaviour
{
    public string shop_code;
    public string image_code;

    GameObject canvas;
    public static GameObject exist;

    void Start()
    {
        if (shop_code == "")
        {
            shop_code = "streeshop";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        #region OLD SHOP DETAIL
        if (GameObject.Find(shop_code))
        {
            Debug.Log("Exists");
        }
        else
        {
            if (exist == null)
            {
                float distance = Vector3.Distance(AD.GameplaySystem.main.characterControl.transform.position, this.transform.position);
                if (distance < 5)
                {
                    exist = (GameObject)Instantiate(Resources.Load("ui/" + shop_code, typeof(GameObject)));

                    
                    if(shop_code == "misslilly" || shop_code == "misslilly2" || shop_code == "misslilly3" ||
                    shop_code == "misslilly4" || shop_code == "misslilly5" || shop_code == "misslilly6" ||
                    shop_code == "misslilly5" || shop_code == "misslilly6" || shop_code == "misslilly7" ||
                    shop_code == "misslilly8") shop_code = "misslilly";

                    string refImage = "sample/"+ shop_code +"/"+ image_code;
                    var addSpite = Resources.Load<Sprite>(refImage);

                    Debug.Log(refImage);
                    Debug.Log(addSpite);
                    Sprite imageP = addSpite;
                    exist.GetComponent<ShopWindows>().SetImageProduct(imageP);
                }
            }

        }
        #endregion


    }
}
