using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun.Demo.PunBasics;

public class Menu_Character : MonoBehaviour
{
    public GameObject window_character, window_bio;
    public Text player_nickname, player_nicknamebio, player_fullname;
    public InputField player_bio;

    public string nickname, gender, fullname, bio;

/*    PlayerPrefs.SetString("authen_code", jsonNode["authen_code"]);
                PlayerPrefs.SetString("email", jsonNode["email"]);
                PlayerPrefs.SetString("nickname", jsonNode["nickname"]);
                PlayerPrefs.SetString("gender", jsonNode["gender"]);
                PlayerPrefs.SetString("fullname", jsonNode["fullname"]);*/

    void Start()
    {
        window_character.SetActive(true);
        window_bio.SetActive(false);
        LoadPlayerData();       
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMenu(bool toggle) 
    {
        if (toggle)
        {
            window_character.SetActive(false);
            window_bio.SetActive(true);
        }
        else 
        {
            window_character.SetActive(true);
            window_bio.SetActive(false);
        }

    }
}
