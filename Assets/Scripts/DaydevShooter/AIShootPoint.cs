using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShootPoint : MonoBehaviour
{
    public AIShooter aIShooter;
    public void OnTriggerEnter(Collider c){
        if(c.tag == "Player"){
            Debug.Log("Hit Player");
            StartCoroutine(DeleBox());
        }
    }

    IEnumerator DeleBox(){
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2.0f);
        GetComponent<BoxCollider>().enabled = true;
    }
}
