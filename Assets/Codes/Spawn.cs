using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using AD;
using AD.Network;

public class Spawn : MonoBehaviour
{
    public float min = -5, max = 5;
    public static System.Action OnPlayerJoinRoom = delegate { };
    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            Vector3 position = NetworkManager.main.spawnPoint;
            if (position == Vector3.zero)
            {
                position = transform.position + new Vector3(Random.Range(min, max), 0.0f, Random.Range(min, max));
                NetworkManager.main.spawnPoint = Vector3.zero;
            }
            GameObject go;
            if (PlayerPrefs.GetString("gender") == "1") go = PhotonNetwork.Instantiate("PlayerMP", position, Quaternion.identity);
            else go = PhotonNetwork.Instantiate("PlayerFM", position, Quaternion.identity);
            GameplaySystem.main.characterControl = go.GetComponent<CharacterControl>();
            OnPlayerJoinRoom.Invoke();
        }
    }
}
