using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerToShooter : MonoBehaviour
{
    public AIShooter aIShooter;
    public void OnTriggerEnter(Collider c){
        if(c.tag == "Player"){
            if(c.GetComponent<PlayerController>().isAlive){
                aIShooter.ShootTo(c.transform);
            }
        }
    }

     public void OnTriggerExit(Collider c){
        if(c.tag == "Player"){
            aIShooter.WalkTo();
        }
    }
}
