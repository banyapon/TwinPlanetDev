using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AD.Network;
using AD;

public class MusicTrigger : MonoBehaviour
{
    public AudioSource[] musicAudios;
    private AudioSource maintheme;
    //private AudioSource changesong;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.main.spawnPoint = Vector3.zero;
        musicAudios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource song in musicAudios)
        {
            if (song.name == "SongA") maintheme = song;
        }
        //changesong = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            if (IsMinePlayer(other.gameObject))
            {
                print("in");
                SoundControl.mode = 2;
                StartCoroutine(SoundControl.FadeOut(maintheme, 5f));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            if (IsMinePlayer(other.gameObject))
            {
                print("exit");
                SoundControl.mode = 1;
                StartCoroutine(SoundControl.FadeIn(maintheme, 5f));
            }
        }
    }

    private bool IsMinePlayer(GameObject other)
    {
        CharacterControl characterController = other.GetComponent<CharacterControl>();
        if (characterController != null && characterController.photonView.IsMine)
        {
            return true;
        }
        return false;
    }
}
