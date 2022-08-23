using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _Oldcamera;
    [SerializeField] private GameObject heart;
    [SerializeField] private Transform heartParent;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private Text ui_score;
    [SerializeField] private Text ui_scoreEnd;
    [SerializeField] private Text txt_time;
    [SerializeField] private GameObject[] deActiveUI;
    private int _health = 3;
    private int score;
    private bool setupCamera;
    private bool alive;
    private float timer = 180f;

    private void Awake()
    {
        //AD.CharacterControl.OnTakeDamage += HandleTakeDamage;
       // AD.CharacterControl.OnGetPoint += HandleGetPoint;
    }
    private void OnDestroy()
    {
        //AD.CharacterControl.OnTakeDamage -= HandleTakeDamage;
//AD.CharacterControl.OnGetPoint -= HandleGetPoint;
    }

    private void HandleGetPoint()
    {
        score++;
        ui_score.text = score.ToString();
    }

   /* private void HandleTakeDamage(int shooter)
    {
        _health--;
        Destroy(heartParent.GetChild(_health).gameObject);
        if (_health <= 0)
        {
            player.GetComponent<Collider>().enabled = false;
            player.GetComponent<Animator>().SetTrigger("Die");
            player.GetComponent<PlayerShoot>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled = false;
            AD.GameplaySystem.main.GetPoint(shooter);
            StartCoroutine(delaySpawn());
        }
    }*/

    private void Start()
    {
        //FindPlayers();
    }
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            txt_time.text = Convert.ToInt32(timer).ToString();
        }
        else
        {
            EndGame();
        }
    }
    private void EndGame()
    {
        Time.timeScale = 0;
        ui_scoreEnd = ui_score;
        endPanel.SetActive(true);
    }
    /*IEnumerator delaySpawn()
    {
        yield return new WaitForSeconds(5.0f);
        player.transform.position = new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
        player.GetComponent<Collider>().enabled = true;
        player.GetComponent<Animator>().SetTrigger("Idle");
        player.GetComponent<PlayerShoot>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
    }*/
    /*private void SetUp()
    {
        player.layer = LayerMask.NameToLayer("HIDE");
        player.GetComponent<PlayerShoot>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<AD.CharacterControl>().enabled = false;
        player.GetComponent<Rigidbody>().useGravity = true;
        player.GetComponent<CharacterController>().enabled = true;
        _Oldcamera.SetActive(false);
        for (int i = 0; i < deActiveUI.Length; i++)
        {
            deActiveUI[i].SetActive(false);
        }
        for (int i = 0; i < player.transform.childCount; i++)
        {
            player.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("HIDE");
            if (player.transform.GetChild(i).GetComponent<Camera>())
            {
                _camera = player.transform.GetChild(i).gameObject;
                _camera.SetActive(true);
            }
        }
        for (int i = 0; i < _health; i++)
        {
            Instantiate(heart, heartParent);
        }
    }
    private void FindPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in players)
        {
            if (item.GetComponent<PhotonView>().IsMine)
            {
                player = item;
                SetUp();
            }
        }
    }*/

}
