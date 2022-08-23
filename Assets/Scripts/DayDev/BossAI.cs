using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float timeRemaining = 10f;
    void Start()
    {
        transform.position = new Vector3(Random.Range(-2f,52f), 1.89f, Random.Range(-40f,65f));
    }

    void Update()
    {
        
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }else if(timeRemaining < 0){
            StartCoroutine(WaitAndPrint(1.0f));
            timeRemaining = 10f;
        }
    }

    void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "npc"){
            transform.position = new Vector3(Random.Range(-2f,52f), 1.89f, Random.Range(-40f,65f));
        }
    }

    IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        transform.position = new Vector3(Random.Range(-2f,52f), 1.89f, Random.Range(-40f,65f));
    }
}
