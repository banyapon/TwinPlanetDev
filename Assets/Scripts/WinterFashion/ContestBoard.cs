using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AD;

public class ContestBoard : MonoBehaviour
{
    [SerializeField] private GameObject _contestWindow;
    [SerializeField] private CharacterControl _characterControl;

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
            this._characterControl = other.GetComponent<CharacterControl>();

            if (_characterControl.photonView.IsMine)
            {
                ToggleContestWindow(true);
            }
        }
    }
}
