using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public static Action<int, int> OnPlayerTakeDamage = delegate { };
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //PhotonView view = other.transform.GetComponent<PhotonView>();
            //OnPlayerTakeDamage.Invoke(Convert.ToInt32(gameObject.name), view.ViewID);
            //view.RPC("TakeDamage", RpcTarget.All, Convert.ToInt32(gameObject.name), view.ViewID);
            //other.GetComponent<PlayerController>().GetDamage(Convert.ToInt32(gameObject.name));
            transform.GetChild(3).gameObject.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            Destroy(this, 0.2f);
        }
        else
        {
            if(other.GetComponent<CheckPlayerToShooter>() || other.GetComponent<PointWalking>() || other.GetComponent<AIShootPoint>()) return;
            transform.GetChild(3).gameObject.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            Destroy(this,0.2f);
        }
    }
}
