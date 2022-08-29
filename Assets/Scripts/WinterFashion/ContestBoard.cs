using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContestBoard : MonoBehaviour
{
    [SerializeField] private GameObject _contestWindow;

    void Start()
    {
        _contestWindow.SetActive(false);
    }

    public void ToggleContestWindow(bool isActive)
    {
        _contestWindow.SetActive(isActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController _playerController = other.GetComponent<PlayerController>();

            if (_playerController.photonView.IsMine)
            {                
                ToggleContestWindow(true);
            }
        }
    }
}
