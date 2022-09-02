using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AD;

public class ContestBoard : MonoBehaviour
{
    [SerializeField] private GameObject _contestWindow;
    private CharacterControl _characterControl;

    void Start()
    {
        _contestWindow.SetActive(false);
    }

    public void ToggleContestWindow(bool isActive)
    {
        _contestWindow.SetActive(isActive);
        _characterControl = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _characterControl = other.GetComponent<CharacterControl>();

            if (_characterControl.photonView.IsMine)
            {                
                ToggleContestWindow(true);
            }
        }
    }
}
