using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIShooter : MonoBehaviour
{
    public Transform[] pointsWalk;
    public float speed = 5.0f;
    private NavMeshAgent ai;
    public Animator ani;
    public GameObject pointGun;

    public GameObject[] hideobjects;
    public GameObject effectDead;

    public bool onShoot;
    public bool onDeath;
    void Start(){
        ai = GetComponent<NavMeshAgent>();
        WalkTo();
        onShoot = false;
        pointGun.SetActive(true);
        PointWalking.OnChangWay += WalkTo;
    }
    void OnDestroy(){
        PointWalking.OnChangWay -= WalkTo;
    }


    public void WalkTo(){
        if (!onDeath)
        {
            Debug.Log("WalkTo");
            ani.SetBool("Shooting", false);
            ai.speed = speed;

            int ranDW = Random.Range(0, pointsWalk.Length - 1);
            ai.SetDestination(pointsWalk[ranDW].position);
        }
    }
    public void ShootTo(Transform target){
        if(onShoot) return;
        onShoot = true;

        ani.SetBool("Shooting",true);
        transform.LookAt(target.position);
        ai.speed = 0;

        StartCoroutine(Shooting());
    }
    public void EffectDead(){
        effectDead.SetActive(false);
        effectDead.SetActive(true);
    }
    public void Deaded(){
        pointGun.SetActive(false);
        ai.enabled = false;
        onDeath = true;

        EffectDead();

        for (int i = 0; i < hideobjects.Length; i++)
        {
            hideobjects[i].SetActive(false);
        }
    }
    public void ReSawpn(){
        pointGun.SetActive(true);
        ai.enabled = true;
        onDeath = false;

        for (int i = 0; i < hideobjects.Length; i++)
        {
            hideobjects[i].SetActive(true);
        }

        EffectDead();

        WalkTo();
    }

    IEnumerator Shooting(){
        int sC = 3;
        while(sC > 0){
            pointGun.SetActive(false);
            pointGun.SetActive(true);

            sC --;
            yield return new WaitForSeconds(2.3f);
        }

        onShoot = false;
        WalkTo();
    }
}
