using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public PlayerWeapon weapon;
    private PhotonView photonView;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    LayerMask mask;

    //Bullet
    public float bulletSpeed = 25;
    public Rigidbody bullet;
    [SerializeField] private Transform posBullet;
    private List<GameObject> bulletList = new List<GameObject>();
    private bool allowFire = true;
    [SerializeField] private GameObject effect;

    public GameObject gunOther;
    public Animator aniGun;

    public SkinnedMeshRenderer changColorShirt;
    public Material[] colorShirt;
    [SerializeField]
    private GameObject avatarSongkarn;
    public GameObject[] hideAllOld;

    private void Awake()
    {
        PlayerController.OnPlayerShoot += HandlePlayerShoot;
        PlayerController.OnPhotonFire += PhotonFire;
    }
    private void OnDestroy()
    {
        PlayerController.OnPlayerShoot -= HandlePlayerShoot;
        PlayerController.OnPhotonFire -= PhotonFire;
    }

    private void HandlePlayerShoot()
    {
        if (allowFire)
        {
            StartCoroutine(Fire(true));
        }
    }

    void Start()
    {
        for (int i = 0; i < hideAllOld.Length; i++)
        {
            hideAllOld[i].SetActive(false);
        }

        avatarSongkarn.SetActive(true);
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera reference!");
            this.enabled = false;
            gunOther.SetActive(true);
        }
        photonView = GetComponent<PhotonView>();

        if(photonView != null){
            SentColor();
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButton("Fire1") && allowFire)
            {
                StartCoroutine(Fire(true));
            }
        }
    }

    IEnumerator Fire(bool photonBullet)
    {
        allowFire = false;
        effect.SetActive(true);
        Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, posBullet.position, posBullet.rotation);
        bulletClone.velocity = posBullet.forward * bulletSpeed;
        bulletClone.GetComponent<BoxCollider>().enabled = photonBullet;
        photonView.RPC("Fire", RpcTarget.All);
        bulletClone.name = photonView.ViewID.ToString();
        bulletList.Add(bulletClone.gameObject);

        if(aniGun != null) aniGun.SetBool("shooting",true);

        if (bulletList.Count > 10)
        {
            Destroy(bulletList[0]);
            bulletList.RemoveAt(0);
        }
        effect.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        if(aniGun != null) aniGun.SetBool("shooting",false);
        allowFire = true;
        StopCoroutine(Fire(true));
    }
    private void PhotonFire()
    {
        if (!photonView.IsMine && GetComponent<PlayerShoot>())
        {
            Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, posBullet.position, posBullet.rotation);
            bulletClone.velocity = posBullet.forward * bulletSpeed;
            //bulletClone.GetComponent<BoxCollider>().enabled = false;
            bulletClone.name = photonView.ViewID.ToString();
            bulletList.Add(bulletClone.gameObject);
            if (bulletList.Count > 10)
            {
                Destroy(bulletList[0]);
                bulletList.RemoveAt(0);
            }
        }
    }

    void SentColor(){
        photonView.RPC("SetColor", RpcTarget.All);
    }

    [PunRPC]
    void SetColor(){

        changColorShirt.material = colorShirt[1];
    }

    public void SetAvatar(bool s)
    {
        avatarSongkarn.SetActive(s);
    }
}

