using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointWalking : MonoBehaviour
{
    public static System.Action OnChangWay = delegate{};

    public void OnWalkTo(){
        OnChangWay.Invoke();
    }

    public void OnTriggerEnter(Collider c){
        if(c.tag == "AI Shooter"){
            Debug.Log("hell");
            OnWalkTo();
        }
    }
}
