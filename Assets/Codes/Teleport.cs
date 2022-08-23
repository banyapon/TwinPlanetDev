using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using AD.Network;

public class Teleport : MonoBehaviour
{
    public string sceneName = "Network";
    public Vector3 targetPoint = Vector3.zero;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(other.GetComponent<PhotonView>().IsMine)
            {
                NetworkManager.main.spawnPoint = targetPoint;
                NetworkManager.main.Join(sceneName);
            }
        }
    }
}
