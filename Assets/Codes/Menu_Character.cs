using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun.Demo.PunBasics;

public class Menu_Character : MonoBehaviour
{
    public CanvasGroup window_character, window_bio;
    public Text player_nickname, player_nicknamebio, player_fullname;
    public InputField player_bio;
    public GameObject _nextToggle, _backToggle;
    public GameObject[] _catagories;

    public string nickname, gender, fullname, bio;

/*    PlayerPrefs.SetString("authen_code", jsonNode["authen_code"]);
                PlayerPrefs.SetString("email", jsonNode["email"]);
                PlayerPrefs.SetString("nickname", jsonNode["nickname"]);
                PlayerPrefs.SetString("gender", jsonNode["gender"]);
                PlayerPrefs.SetString("fullname", jsonNode["fullname"]);*/

    void Start()
    {
        //window_character.SetActive(true);
        window_bio.blocksRaycasts = false;
        ChangeCatagory(0);
        LoadPlayerData();

        //_nextToggle.onValueChanged.AddListener((v) => NextValue(v));
        //_backToggle.onValueChanged.AddListener((v) => BackValue(v));
    }

    public void LoadPlayerData() 
    {
        nickname = PlayerPrefs.GetString("nickname");
        fullname = PlayerPrefs.GetString("fullname");
        gender = PlayerPrefs.GetString("gender");
        bio = PlayerPrefs.GetString("bio");
        player_nickname.text = nickname;
        player_nicknamebio.text = nickname;
        player_fullname.text = fullname;
        player_bio.text = bio;
    }

    public void SaveBio() 
    {
        PlayerPrefs.SetString("bio", player_bio.text);
    }

    void Update()
    {
        
    }

    #region CUSTOMIZE CATAGORIES
    public void ChangeCatagory(int index)
    {
        if (_catagories[index] == null) return;

        for (int i = 0; i < _catagories.Length; i++)
        {
            _catagories[i].SetActive(false);
        }
        _catagories[index].SetActive(true);
    }
    #endregion

    #region TOGGLE UI
    void NextValue(float value)
    {
        window_character.alpha = 1f - value;
        window_bio.alpha = value;

        if (value > 0.9f)
        {
            ToggleMenu(true);
            //_nextToggle.value = 0;
            window_character.alpha = 0f;
            window_bio.alpha = 1f;
        }           
    }

    //public void ReturnNext(int value) => _nextToggle.value = value;

    void BackValue(float value)
    {       
        window_bio.alpha = value;
        window_character.alpha = 1f - value;

        if (value < 0.1f)
        {
            ToggleMenu(false);
            //_backToggle.value = 1;
            window_bio.alpha = 0f;
            window_character.alpha = 1f;
        }
    }

    //public void ReturnBack(int value) => _backToggle.value = value;

    public void ToggleMenu(bool toggle) 
    {
        if (toggle)
        {
            _nextToggle.gameObject.SetActive(false);
            _backToggle.gameObject.SetActive(true);
            window_bio.blocksRaycasts = true;
            window_character.alpha = 0;
            window_bio.alpha = 1;           
        }
        else 
        {
            _nextToggle.gameObject.SetActive(true);
            _backToggle.gameObject.SetActive(false);
            window_bio.blocksRaycasts = false;
            window_character.alpha = 1;
            window_bio.alpha = 0;
        }
    }
    #endregion
}
