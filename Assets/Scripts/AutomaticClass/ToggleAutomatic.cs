using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAutomatic : MonoBehaviour
{
    GameController gameController;
    void Start()
    {
        if (gameController == null)
        {
            GameObject _gameController = GameObject.FindGameObjectWithTag("GameController") as GameObject;
            gameController = _gameController.GetComponent<GameController>();
        }
    }

    public void PressIntGender(int gender){
        gameController.gender_value = gender;
    }
}
