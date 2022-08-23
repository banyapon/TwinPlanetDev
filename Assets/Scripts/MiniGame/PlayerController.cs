using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Base
    [SerializeField] private GameObject minigameCamera;
    public bool miniGame;
    public Animator animator;
    public PhotonView photonView;
    public CharacterController controller;

    //Movement
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;
    public float mouseSensitivity = 200f;
    [SerializeField] private Transform _camera;
    public float xRotation = 0f;
    private bool damaged;

    //Gameplay
    private int health = 15;
    public static Action OnPlayerShoot = delegate { };
    public static Action OnPhotonFire = delegate { };

    public bool isAlive {get; private set;}
    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
        isAlive = true;
        AIHealth.OnPlayerGetPoint += HandlePlayerGetPoint;
        //Bullet.OnPlayerTakeDamage += HandlePlayerTakeDamage;
        AD.GameplaySystem.OnPlayerDead += HandlePlayerDead;
    }
    private void OnDestroy()
    {
        AIHealth.OnPlayerGetPoint -= HandlePlayerGetPoint;
        //Bullet.OnPlayerTakeDamage -= HandlePlayerTakeDamage;
        AD.GameplaySystem.OnPlayerDead -= HandlePlayerDead;
    }

    private void HandlePlayerDead()
    {
        if (photonView.IsMine)
        {
            isAlive = false;
            GetComponent<PlayerShoot>().SetAvatar(false);
            GetComponent<Collider>().enabled = false;
            GetComponent<PlayerShoot>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Bullet"))
        {
            int shooter = Convert.ToInt32(other.name);

            if (photonView.IsMine)
            {
                if (health > 0)
                {

                    health--;
                    AD.GameplaySystem.main.heartParent.GetChild(health).gameObject.SetActive(false);
                    print(health);
                    if (health <= 0)
                    {
                        photonView.RPC("GetPoint", RpcTarget.All, shooter, 30);
                        PlayerDead();
                        photonView.RPC("PlayDeath", RpcTarget.All, photonView.ViewID);
                    }
                }
            }
        }*/
        if (other.CompareTag("AIBullet"))
        {
            if (photonView.IsMine)
            {
                Debug.Log("GETHit");
                if (health > 0)
                {
                    print(health);
                    health--;
                    AD.GameplaySystem.main.heartParent.GetChild(health).gameObject.SetActive(false);
                    if (health <= 0)
                    {
                        PlayerDead();
                        photonView.RPC("PlayDeath", RpcTarget.All, photonView.ViewID);
                    }
                }
            }
        }
    }


    private void HandlePlayerGetPoint(int id,int score)
    {
        if (photonView.IsMine)
        {
            if (id == photonView.ViewID)
            {
                AD.GameplaySystem.main.GetScore(score);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // A small number instead of 0 forces player down on ground if any distance remains.
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            photonView.RPC("PlayerMove", RpcTarget.All, transform.position);

            
            //transform.localPosition = new Vector3(0, 1.5f, 0); 
            if (transform != null)
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                transform.Rotate(Vector3.up * mouseX);

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                _camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                photonView.RPC("PlayerRotate", RpcTarget.All, transform.rotation, _camera.rotation);
            }

            //Gamplay
        }
    }
    private void PlayerDead()
    {
        if (photonView.IsMine)
        {
            isAlive = false;
            AD.GameplaySystem.main.EndGame();
            GetComponent<PlayerShoot>().SetAvatar(false);
            GetComponent<Collider>().enabled = false;
            GetComponent<PlayerShoot>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
        }
        //GetComponent<Animator>().SetTrigger("Die");
        /*if (photonView.IsMine)
        {
            AD.GameplaySystem.main.Respawn(true);
        }
        StartCoroutine(delaySpawn());*/
    }
    /*IEnumerator delaySpawn()
    {
        yield return new WaitForSeconds(5.0f);
        transform.position = new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
        photonView.RPC("PlayerMove", RpcTarget.All, transform.position);
        GetComponent<Collider>().enabled = true;
        GetComponent<PlayerShoot>().enabled = true;
        GetComponent<PlayerController>().enabled = true;
        //GetComponent<Animator>().SetTrigger("Idle");
        GetComponent<PlayerShoot>().SetAvatar(true);
        health = 8;
        isAlive =true;
        if (photonView.IsMine)
        {
            AD.GameplaySystem.main.Respawn(false);
        }
    }*/

    [PunRPC]
    public void SetupMiniGame()
    {
        if (!photonView.IsMine)
        {
            //gameObject.layer = LayerMask.NameToLayer("HIDE");
            GetComponent<PlayerShoot>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<CharacterController>().enabled = true;
        }
    }
    [PunRPC]
    public void PlayerMove(Vector3 pos)
    {
        if(!photonView.IsMine) transform.position = pos;
    }
    [PunRPC]
    public void PlayerRotate(Quaternion player, Quaternion camera)
    {
        if (!photonView.IsMine)
        {
            transform.rotation = player;
            minigameCamera.transform.rotation = camera;
        }
    }
    [PunRPC]
    public void GetPoint(int shooter,int score)
    {
        if (photonView.IsMine)
        {
            if (shooter == photonView.ViewID)
            {
                AD.GameplaySystem.main.GetScore(score);
            }
        }
    }
    [PunRPC]
    public void PlayDeath(int viewID)
    {
        if (!photonView.IsMine)
        {
            if (viewID == photonView.ViewID)
            {
                PlayerDead();
            }
        }
    }
    [PunRPC]
    public void Fire()
    {
        if (!photonView.IsMine && GetComponent<PlayerShoot>() && GetComponent<PlayerController>())
        {
            OnPhotonFire.Invoke();
        }
    }
}
