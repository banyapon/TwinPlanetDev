using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
    public int maxHp;
    private int hp;
    public AIShooter aIShooter;
    private GameObject player;
    [SerializeField] private int scoreForPlayer;

    public bool isAlive { get; private set; }
    public static Action<int,int> OnPlayerGetPoint = delegate { };

    void Start()
    {
        hp = maxHp;
        isAlive = true;
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.transform.GetComponent<Bullet>())
        {
            if(isAlive){
                Damage(Convert.ToInt32(c.name));
            }
        }
    }

    void Damage(int idPlayer)
    {
        Debug.Log("damage!");

        hp -= 1;
        if (hp <= 0)
        {
            OnPlayerGetPoint.Invoke(idPlayer,scoreForPlayer);
            Debug.Log("die");
            isAlive = false;
            GetComponent<BoxCollider>().enabled =false;
            StartCoroutine(WaitForReSpawn());
        }
    }

    IEnumerator WaitForReSpawn()
    {
        aIShooter.Deaded();
        yield return new WaitForSeconds(8.0f);
        GetComponent<BoxCollider>().enabled =true;
        isAlive = true;
        hp = maxHp;
        Debug.Log("reborn");
        aIShooter.ReSawpn();
    }
}
